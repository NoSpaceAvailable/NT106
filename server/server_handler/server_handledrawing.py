import socket
import threading
import struct
import sys
from PIL import Image, ImageDraw
import aggdraw
import io
import json
import psycopg2, time
from collections import defaultdict

rooms_clients = defaultdict(list)
rooms = {}

HOST = '0.0.0.0'
IN_PORT = 9999
width, height = 1545, 722

lock = threading.Lock()

time.sleep(1)  # Ensure the database is ready before connecting 

conn = psycopg2.connect(
    host="database",
    port=5432,
    dbname="rooms",
    user="postgresql",
    password="095f75fe10f6541b51d4a1ca84a993ac274ac14bee50a6c7a7df6c79cffd1946"
)

def broadcast_to_room(room_id, message, sender_socket):
    with lock:
        for client in rooms_clients[room_id][:]:
            if client is not sender_socket:
                try:
                    client.sendall(message)
                except:
                    rooms_clients[room_id].remove(client)
                    client.close()

def apply_draw_packet_to_room(room, packet_bytes):
    try:
        packet_json = packet_bytes[4:].decode("utf-8")
        packet = json.loads(packet_json)

        draw_data = packet["Data"]
        color = packet["Color"]
        color_tuple = (color["R"], color["G"], color["B"], color["A"])

        canvas = rooms[room]["canvas"]
        draw = aggdraw.Draw(canvas)

        for d in draw_data:
            event = d["Event"]
            shape = d["Shape"]
            pen_width = d["penWid"]
            x = d["X"]
            y = d["Y"]
            prev = (d["prevPoint"]["X"], d["prevPoint"]["Y"])
            current = (d["Location"]["X"], d["Location"]["Y"])

            brush = aggdraw.Brush(color_tuple)
            pen = aggdraw.Pen(color_tuple, pen_width)

            if shape == 0:  # FreeDraw
                if event == 1:  # DOWN
                    draw.ellipse(
                        [x - pen_width // 2, y - pen_width // 2,
                         x + pen_width // 2, y + pen_width // 2],
                        brush
                    )
                elif event == 2:  # MOVE
                    draw.line([prev, current], pen)
                    draw.ellipse(
                        [x - pen_width // 2, y - pen_width // 2,
                         x + pen_width // 2, y + pen_width // 2],
                        brush
                    )

            elif event == 3:  # UP with Shape
                apply_shape_agg(draw, shape, prev, current, pen)

        draw.flush()

    except Exception as e:
        print("Error rendering to room:", e, flush=True)

def apply_shape_agg(draw, shape, start, end, pen):
    x1, y1 = start
    x2, y2 = end
    bbox = [min(x1, x2), min(y1, y2), max(x1, x2), max(y1, y2)]

    if shape == 1:  # Circle
        draw.ellipse(bbox, pen)
    elif shape == 2:  # Rectangle
        draw.rectangle(bbox, pen)
    elif shape == 3:  # Triangle
        top = ((x1 + x2) // 2, min(y1, y2))
        left = (min(x1, x2), max(y1, y2))
        right = (max(x1, x2), max(y1, y2))
        draw.polygon([top, left, right], pen)
    elif shape == 4:  # Line
        draw.line([start, end], pen)

def save_room_image_to_db(room_id, pil_image):
    buf = io.BytesIO()
    pil_image.save(buf, format="PNG")
    image_bytes = buf.getvalue()

    cur = conn.cursor()
    cur.execute("""
        INSERT INTO room_snapshots (room_id, image)
        VALUES (%s, %s)
        ON CONFLICT (room_id) DO UPDATE SET image = EXCLUDED.image
    """, (room_id, psycopg2.Binary(image_bytes)))
    conn.commit()
    cur.close()
    conn.close()

def load_room_image_from_db(room_id):
    cur = conn.cursor()
    cur.execute("SELECT image FROM room_snapshots WHERE room_id = %s", (room_id,))
    row = cur.fetchone()
    cur.close()
    conn.close()

    if row:
        return Image.open(io.BytesIO(row[0]))
    else:
        return None

def send_canvas_to_client(client_socket, room_id):
    try:
        print(f"[+] Trying sent canvas snapshot of room {room_id} to client", flush=True)
        canvas = rooms[room_id]["canvas"]
        buf = io.BytesIO()
        canvas.save(buf, format="PNG")
        img_bytes = buf.getvalue()
        length_prefix = struct.pack('<I', len(img_bytes))
        client_socket.sendall(length_prefix + img_bytes)
        print(f"[+] Sent canvas snapshot of room {room_id} to client", flush=True)
    except Exception as e:
        print(f"[!] Failed to send canvas: {e}", flush=True)

def save_img(room_id):
    rooms[room_id]["canvas"].save(f"room_{room_id}.png")

def handle_client(client_socket, addr):
    print(f"[+] New connection from {addr}", flush=True)
    with lock:
        room_id = client_socket.recv(4)
        if not room_id:
            print(f"[-] No room ID received from {addr}", flush=True)
            client_socket.close()
            return
        room_id = struct.unpack('<I', room_id)[0]
        print(f"Room ID: {room_id}", flush=True)
        if room_id not in rooms:
            snapshot = load_room_image_from_db(room_id)
            
            if snapshot:
                canvas = snapshot
            else:
                canvas = Image.new("RGB", (width, height), (255, 255, 255))
            
            rooms[room_id] = {
                "canvas": canvas,
                "draw": ImageDraw.Draw(canvas)
            }

        print(f"[+] Client {addr} connected to room {room_id}", flush=True)
        rooms_clients[room_id].append(client_socket)
        save_room_image_to_db(room_id, rooms[room_id]["canvas"])
        send_canvas_to_client(client_socket, room_id)

    try:
        count = 0
        while True:
            raw_len = client_socket.recv(4)
            if not raw_len:
                break

            msg_len = struct.unpack('<I', raw_len)[0]

            data = b''
            while len(data) < msg_len:
                packet = client_socket.recv(msg_len - len(data))
                if not packet:
                    break
                data += packet

            if not data:
                break

            full_message = raw_len + data
            apply_draw_packet_to_room(room_id, full_message)
            broadcast_to_room(room_id, full_message, client_socket)

    except Exception as e:
        print(f"[!] Error with {addr}: {e}", flush=True)
    finally:
        print(f"[-] Client {addr} disconnected", flush=True)
        with lock:
            if client_socket in rooms_clients[room_id]:
                rooms_clients[room_id].remove(client_socket)
        client_socket.close()
        print(f"[-] Client {addr} removed from clients list of room {room_id}", flush=True)
        save_room_image_to_db(room_id, rooms[room_id]["canvas"])
        if not rooms_clients[room_id]:
            rooms.pop(room_id, None)

def start_server_listening():
    try:
        server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        server_socket.bind((HOST, IN_PORT))
        server_socket.listen()

        print(f"[*] Server listening client on {HOST}:{IN_PORT}", flush=True)
        while True:
            client_sock, addr = server_socket.accept()
            thread = threading.Thread(target=handle_client, args=(client_sock, addr), daemon=True)
            thread.start()
    except KeyboardInterrupt:
        print("\n[!] Server shutting down...", flush=True)
    except Exception as e:
        print(f"[!] Error: {e}", flush=True)
    finally:
        server_socket.close()
        with lock:
            for clients in rooms_clients.values():
                for c in clients:
                    c.close()

def start_server_status():
    try:
        server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        server_socket.bind((HOST, OUT_PORT))
        server_socket.listen()

        print(f"[*] Server listening for load status on {HOST}:{OUT_PORT}", flush=True)
        while True:
            client_sock, addr = server_socket.accept()
            with lock:
                total_clients = sum(len(clients) for clients in rooms_clients.values())
                client_sock.sendall(json.dumps({"load": total_clients}).encode('utf-8'))
            client_sock.close()
    except KeyboardInterrupt:
        print("\n[!] Server shutting down...", flush=True)
    except Exception as e:
        print(f"[!] Error: {e}", flush=True)
    finally:
        server_socket.close()

#IN_PORT for handle client connect
#OUT_PORT for server load status
if __name__ == "__main__":
    if len(sys.argv) != 4:
        print("Usage: python server.py <HOST> <IN_PORT> <OUT_PORT>", flush=True)
        exit()
    HOST = sys.argv[1]
    IN_PORT = int(sys.argv[2])
    OUT_PORT = int(sys.argv[3])
    print(f"[*] Starting server on {HOST}:{IN_PORT}\n[*] Get server load status through {HOST}:{OUT_PORT}", flush=True)
    listening = threading.Thread(target=start_server_listening, args = (), daemon = True)
    listening.start()
    speaking = threading.Thread(target=start_server_status, args = (), daemon = True)
    speaking.start()
    listening.join()
    speaking.join()