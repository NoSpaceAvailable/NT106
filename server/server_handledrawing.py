import socket
import threading
import struct
import sys
from PIL import Image, ImageDraw
import io
import json
# import sqlite3

rooms = {}

HOST = '0.0.0.0'
IN_PORT = 9999
width, height = 1545, 722

clients = []
lock = threading.Lock()

def broadcast(message, sender_socket):
    with lock:
        for client in clients[:]:
            if client is not sender_socket:
                try:
                    client.sendall(message)
                except:
                    clients.remove(client)
                    client.close()

def apply_shape(draw, shape, start, end, color, width):
    x1, y1 = start
    x2, y2 = end
    bbox = [min(x1, x2), min(y1, y2), max(x1, x2), max(y1, y2)]

    if shape == 1:  # Circle
        draw.ellipse(bbox, outline=color, width=width)

    elif shape == 2:  # Rectangle
        draw.rectangle(bbox, outline=color, width=width)

    elif shape == 3:  # Triangle
        top = ((x1 + x2) // 2, min(y1, y2))
        left = (min(x1, x2), max(y1, y2))
        right = (max(x1, x2), max(y1, y2))
        draw.polygon([top, left, right], outline=color, width=width)

    elif shape == 4:  # Line
        draw.line([start, end], fill=color, width=width)

def apply_draw_packet_to_room(room, packet_bytes):
    try:
        packet_json = packet_bytes[4:].decode("utf-8")
        packet = json.loads(packet_json)

        draw_data = packet["Data"]
        color = packet["Color"]
        color_tuple = (color["R"], color["G"], color["B"], color["A"])

        draw = rooms[room]["draw"]

        for d in draw_data:
            event = d["Event"]
            shape = d["Shape"]
            pen_width = d["penWid"]
            x = d["X"]
            y = d["Y"]
            prev = (d["prevPoint"]["X"], d["prevPoint"]["Y"])
            current = (d["Location"]["X"], d["Location"]["Y"])

            if shape == 0:  # FreeDraw
                if event == 1:  # DOWN
                    draw.ellipse([
                        (x - pen_width // 2, y - pen_width // 2),
                        (x + pen_width // 2, y + pen_width // 2)
                    ], fill=color_tuple)

                elif event == 2:  # MOVE
                    draw.line([prev, current], fill=color_tuple, width=pen_width)
                    draw.ellipse([
                        (x - pen_width // 2, y - pen_width // 2),
                        (x + pen_width // 2, y + pen_width // 2)
                    ], fill=color_tuple)

            elif event == 3:  # UP and shape != FreeDraw
                apply_shape(draw, shape, prev, current, color_tuple, pen_width)

    except Exception as e:
        print("Error rendering to room:", e)

# def save_room_image_to_db(room_id, pil_image):
#     import io, sqlite3
#     buf = io.BytesIO()
#     pil_image.save(buf, format="PNG")
#     image_bytes = buf.getvalue()

#     conn = sqlite3.connect("rooms.sql")
#     c = conn.cursor()
#     c.execute("REPLACE INTO room_snapshots (room_id, image) VALUES (?, ?)", (room_id, image_bytes))
#     conn.commit()
#     conn.close()

# def load_room_image_from_db(room_id):
#     conn = sqlite3.connect("rooms.sql")
#     c = conn.cursor()
#     c.execute("SELECT image FROM room_snapshots WHERE room_id = ?", (room_id,))
#     row = c.fetchone()
#     conn.close()

#     if row:
#         return Image.open(io.BytesIO(row[0]))
#     else:
        # return None
    
def save_img(room_id):
    rooms[room_id]["canvas"].save(f"room_{room_id}.png")

def handle_client(client_socket, addr):
    print(f"[+] New connection from {addr}")
    with lock:
        room_id = client_socket.recv(4)
        if not room_id:
            print(f"[-] No room ID received from {addr}")
            client_socket.close()
            return
        room_id = struct.unpack('<I', room_id)[0]
        print(f"Room ID: {room_id}")
        if room_id not in rooms:
            # snapshot = load_room_image_from_db(room_id)
            snapshot = None
            if snapshot:
                canvas = snapshot
            else:
                canvas = Image.new("RGBA", (width, height), (255, 255, 255))
            
            rooms[room_id] = {
                "canvas": canvas,
                "draw": ImageDraw.Draw(canvas)
            }

        clients.append(client_socket)

    try:
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
            broadcast(full_message, client_socket)

    except Exception as e:
        print(f"[!] Error with {addr}: {e}")
    finally:
        print(f"[-] Client {addr} disconnected")
        with lock:
            if client_socket in clients:
                clients.remove(client_socket)
        client_socket.close()
        print(f"[-] Client {addr} removed from clients list of room {room_id}")
        save_img(room_id)

def start_server_listening():
    try:
        server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        server_socket.bind((HOST, IN_PORT))
        server_socket.listen()

        print(f"[*] Server listening client on {HOST}:{IN_PORT}")
        while True:
            client_sock, addr = server_socket.accept()
            thread = threading.Thread(target=handle_client, args=(client_sock, addr), daemon=True)
            thread.start()
    except KeyboardInterrupt:
        print("\n[!] Server shutting down...")
    except Exception as e:
        print(f"[!] Error: {e}")
    finally:
        server_socket.close()
        with lock:
            for c in clients:
                c.close()

def start_server_status():
    try:
        server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        server_socket.bind((HOST, OUT_PORT))
        server_socket.listen()

        print(f"[*] Server listening for load status on {HOST}:{OUT_PORT}")
        while True:
            client_sock, addr = server_socket.accept()
            with lock:
                client_sock.sendall(json.dumps({"load": len(clients)}).encode('utf-8'))
            client_sock.close()
    except KeyboardInterrupt:
        print("\n[!] Server shutting down...")
    except Exception as e:
        print(f"[!] Error: {e}")
    finally:
        server_socket.close()

#IN_PORT for handle client connect
#OUT_PORT for server load status

if __name__ == "__main__":
    if len(sys.argv) != 4:
        print("Usage: python server.py <HOST> <IN_PORT> <OUT_PORT>")
        exit()
    HOST = sys.argv[1]
    IN_PORT = int(sys.argv[2])
    OUT_PORT = int(sys.argv[3])
    print(f"[*] Starting server on {HOST}:{IN_PORT}\n[*] Get server load status through {HOST}:{OUT_PORT}")
    listening = threading.Thread(target=start_server_listening, args = (), daemon = True)
    listening.start()
    speaking = threading.Thread(target=start_server_status, args = (), daemon = True)
    speaking.start()
    listening.join()
    speaking.join()
