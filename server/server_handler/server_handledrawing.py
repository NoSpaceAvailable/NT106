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

SERVER_PATH = "servers.json"
HOST = '0.0.0.0'
IN_PORT = 1337
OUT_PORT = 1339
width, height = 1545, 722

SERVER = []
FIRST_CONNECT = b'\x01' 
ALREADY_CONNECTED = b'\x02'
CHANGE_SERVER = b'\x03'
PACKAGE_FROM_ANOTHER_SERVER = b'\x04'
SYNC_CLIENT = b'\x05'
DONE = b'\x06'


lock = threading.Lock()
SERVERS_LOCK = threading.Lock()
UNHEALTHY_SERVERS = []

time.sleep(1)  # Ensure the database is ready before connecting 

conn = psycopg2.connect(
    host="database",
    port=5432,
    dbname="rooms",
    user="postgresql",
    password="095f75fe10f6541b51d4a1ca84a993ac274ac14bee50a6c7a7df6c79cffd1946"
)

def load_servers_from_json(path="servers.json"):
    global SERVERS
    try:
        with open(path, "r") as f:
            SERVERS = json.load(f)
        global UNHEALTHY_SERVERS
        UNHEALTHY_SERVERS = []
    except Exception as e:
        print(f"[!] Failed to load server list from {path}: {e}")
        sys.exit(1)

def broadcast_to_room(room_id, message, sender_socket):
    for client in rooms_clients[room_id][:]:
        if client is not sender_socket:
            try:
                client.sendall(message)
            except:
                rooms_clients[room_id].remove(client)
                client.close()

def broadcast_to_other_servers(package, _sync = False):
    global SERVERS, UNHEALTHY_SERVERS
    servers_to_remove = []
    with SERVERS_LOCK:
        for server in SERVERS[:]:
            try:
                print(f"[+] Broadcasting to {server['host']}:{server['in_port']}", flush=True)
                sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
                sock.settimeout(2)
                sock.connect((server["host"], server["in_port"]))
                sock.sendall(package)
                if _sync:
                    sock.recv(1)
                sock.close()
            except Exception as e:
                print(f"[Error] Could not send to {server['host']}:{server['in_port']} - {e}")
                servers_to_remove.append(server)
            finally:
                pass
        for server in servers_to_remove:
            if server in SERVERS:
                SERVERS.remove(server)
                if server not in UNHEALTHY_SERVERS:
                    UNHEALTHY_SERVERS.append(server)
                print(f"[!] Removed {server['host']}:{server['in_port']} from SERVERS due to failure.", flush=True)

def apply_draw_packet_to_room(room, packet_bytes):
    try:
        # print(packet_bytes, flush=True)
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
                    dx = current[0] - prev[0]
                    dy = current[1] - prev[1]
                    steps = max(abs(dx), abs(dy))

                    if steps == 0:
                        # Single point — draw one circle
                        draw.ellipse(
                            [current[0] - pen_width // 2, current[1] - pen_width // 2,
                            current[0] + pen_width // 2, current[1] + pen_width // 2],
                            brush
                        )
                    else:
                        for i in range(steps + 1):
                            t = i / steps
                            interp_x = int(prev[0] + dx * t)
                            interp_y = int(prev[1] + dy * t)
                            draw.ellipse(
                                [interp_x - pen_width // 2, interp_y - pen_width // 2,
                                interp_x + pen_width // 2, interp_y + pen_width // 2],
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

def load_room_image_from_db(room_id):
    cur = conn.cursor()
    cur.execute("SELECT image FROM room_snapshots WHERE room_id = %s", (room_id,))
    row = cur.fetchone()
    cur.close()

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
        client_socket.sendall(struct.pack("<I", room_id) + length_prefix + img_bytes)
        print(f"[+] Sent canvas snapshot of room {room_id} to client", flush=True)
    except Exception as e:
        print(f"[!] Failed to send canvas: {e}", flush=True)

def handle_client(client_socket, addr):
    print(f"[+] New connection from {addr}", flush=True)
    room_id = None
    try:
        while True:

            State = client_socket.recv(1)    
            while not State:
                State = client_socket.recv(1)
                if client_socket.fileno() == -1:
                    print(f"[-] Client {addr} disconnected", flush=True)
                    return

            room_id = client_socket.recv(4)
            while not room_id:
                room_id = client_socket.recv(4)
                if client_socket.fileno() == -1:
                    print(f"[-] Client {addr} disconnected", flush=True)
                    return
                
            room_id = struct.unpack('<I', room_id)[0]
            print(f"[+] Client {addr} requested room {room_id} with state {State}", flush=True)

            if (State == PACKAGE_FROM_ANOTHER_SERVER or State == SYNC_CLIENT) and room_id not in rooms:
                print(f"[!] Room {room_id} not found in local server, ignoring package from another server", flush=True)
                client_socket.close()
                return
            
            if State == SYNC_CLIENT:
                if room_id in rooms:
                    with lock:
                        save_room_image_to_db(room_id, rooms[room_id]["canvas"])
                client_socket.sendall(struct.pack("<B", DONE))
                client_socket.close()
                return

            if State == FIRST_CONNECT or State == CHANGE_SERVER:
                if State == FIRST_CONNECT:
                    broadcast_to_other_servers(SYNC_CLIENT + struct.pack("<I", room_id), True)
                if room_id not in rooms:
                    with lock:
                        snapshot = load_room_image_from_db(room_id)
                    
                    if snapshot:
                        canvas = snapshot
                    else:
                        canvas = Image.new("RGB", (width, height), (255, 255, 255, 1))
                    
                    rooms[room_id] = {
                        "canvas": canvas,
                        "draw": ImageDraw.Draw(canvas)
                    }

                print(f"[+] Client {addr} connected to room {room_id}", flush=True)
                if room_id not in rooms_clients:
                    rooms_clients[room_id] = []
                rooms_clients[room_id].append(client_socket)
                with lock:
                    save_room_image_to_db(room_id, rooms[room_id]["canvas"])

                if State != CHANGE_SERVER:
                    send_canvas_to_client(client_socket, room_id)

            if State == ALREADY_CONNECTED or State == PACKAGE_FROM_ANOTHER_SERVER:
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
                print(f"[+] Received data from {addr} for room {room_id}", flush=True)
                full_message = raw_len + data
                apply_draw_packet_to_room(room_id, full_message)
                with lock:
                    save_room_image_to_db(room_id, rooms[room_id]["canvas"])
                print(f"[+] Applied draw packet to room {room_id}", flush=True)
                broadcast_to_room(room_id, struct.pack("<I", room_id) + full_message, client_socket)
                print(f"[+] Broadcasted draw packet to room {room_id}", flush=True)
                if State != PACKAGE_FROM_ANOTHER_SERVER:
                    print(f"[+] Sending draw packet to other servers for room {room_id}", flush=True)
                    full_message = PACKAGE_FROM_ANOTHER_SERVER + struct.pack("<I", room_id) + full_message
                    broadcast_to_other_servers(full_message)
                    with lock:
                        save_room_image_to_db(room_id, rooms[room_id]["canvas"])
                else:
                    return

    except Exception as e:
        print(f"[!] Error with {addr}: {e}", flush=True)
    finally:
        client_socket.close()
        print(f"[-] Client {addr} disconnected", flush=True)
        if State == PACKAGE_FROM_ANOTHER_SERVER or State == SYNC_CLIENT:
            return
        if room_id is not None:
            if room_id in rooms_clients and client_socket in rooms_clients[room_id]:
                rooms_clients[room_id].remove(client_socket)
                print(f"[-] Client {addr} removed from clients list of room {room_id}", flush=True)
            if room_id in rooms:
                with lock:
                    save_room_image_to_db(room_id, rooms[room_id]["canvas"])
            if room_id in rooms_clients and not rooms_clients[room_id]:
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

# Health check for unhealthy servers
def background_health_check():
    global SERVERS, UNHEALTHY_SERVERS
    while True:
        time.sleep(5)
        with SERVERS_LOCK:
            for server in UNHEALTHY_SERVERS[:]:
                try:
                    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
                    sock.settimeout(2)
                    sock.connect((server["host"], server["in_port"]))
                    sock.close()
                    SERVERS.append(server)
                    UNHEALTHY_SERVERS.remove(server)
                    print(f"[+] Server {server['host']}:{server['in_port']} is healthy again and added back to SERVERS.", flush=True)
                except Exception as e:
                    print(f"[HealthCheck] {server['host']}:{server['in_port']} still unhealthy: {e}", flush=True)

#IN_PORT for handle client connect
#OUT_PORT for server load status
if __name__ == "__main__":
    if len(sys.argv) < 4 or len(sys.argv) > 5:
        print("Usage: python server.py <HOST> <IN_PORT> <OUT_PORT> [servers.json]", flush=True)
        exit()
    HOST = sys.argv[1]
    IN_PORT = int(sys.argv[2])
    OUT_PORT = int(sys.argv[3])
    if(len(sys.argv) == 5):
        load_servers_from_json(sys.argv[4])
    for i, server in enumerate(SERVERS):
        if server["in_port"] == IN_PORT and server["out_port"] == OUT_PORT:
            popped = SERVERS.pop(i)
            break
    print(f"[*] Starting server on {HOST}:{IN_PORT}\n[*] Get server load status through {HOST}:{OUT_PORT}", flush=True)
    # Start health check thread
    health_thread = threading.Thread(target=background_health_check, daemon=True)
    health_thread.start()
    listening = threading.Thread(target=start_server_listening, args = (), daemon = True)
    listening.start()
    speaking = threading.Thread(target=start_server_status, args = (), daemon = True)
    speaking.start()
    listening.join()
    speaking.join()