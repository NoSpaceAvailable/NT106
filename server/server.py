import socket
import threading
import struct
import sys
from PIL import Image, ImageDraw
import io
import json

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

def draw_from_message(message, room_id):
    if room_id not in rooms:
        rooms[room_id]["canvas"] = Image.new('RGB', (width, height), color='white')
    
    img = rooms[room_id]["canvas"]
    data = message[4:]

    if len(data) < 4:
        print(f"[-] Invalid data length received for room {room_id}")
        return

    x, y, r, g, b = struct.unpack('<IIHHH', data[:12])
    draw = ImageDraw.Draw(img)
    draw.point((x, y), fill=(r, g, b))

    # Save the image to a buffer
    buffer = io.BytesIO()
    img.save(buffer, format='PNG')
    buffer.seek(0)


def handle_client(client_socket, addr):
    print(f"[+] New connection from {addr}")
    with lock:
        room_id = client_socket.recv(4)
        if not room_id:
            print(f"[-] No room ID received from {addr}")
            client_socket.close()
            return
        room_id = struct.unpack('<I', room_id)[0]
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
            draw_from_message(full_message, room_id)
            broadcast(full_message, client_socket)

    except Exception as e:
        print(f"[!] Error with {addr}: {e}")
    finally:
        print(f"[-] Client {addr} disconnected")
        with lock:
            if client_socket in clients:
                clients.remove(client_socket)
        client_socket.close()

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
            client_sock = server_socket.accept()
            with lock:
                client_sock.sendall(json.dumps({"load": len(clients)}).encode('utf-8'))
            client_sock.close()
    except KeyboardInterrupt:
        print("\n[!] Server shutting down...")
    except Exception as e:
        print(f"[!] Error: {e}")
    finally:
        server_socket.close()

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
