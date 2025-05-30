import socket
import threading
import struct
import sys

HOST = '0.0.0.0'
PORT = 9999

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

def handle_client(client_socket, addr):
    print(f"[+] New connection from {addr}")
    with lock:
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
            broadcast(full_message, client_socket)

    except Exception as e:
        print(f"[!] Error with {addr}: {e}")
    finally:
        print(f"[-] Client {addr} disconnected")
        with lock:
            if client_socket in clients:
                clients.remove(client_socket)
        client_socket.close()

def start_server():
    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server_socket.bind((HOST, PORT))
    server_socket.listen()

    print(f"[*] Server listening on {HOST}:{PORT}")

    try:
        while True:
            client_sock, addr = server_socket.accept()
            thread = threading.Thread(target=handle_client, args=(client_sock, addr), daemon=True)
            thread.start()
    except KeyboardInterrupt:
        print("\n[!] Server shutting down...")
    finally:
        server_socket.close()
        with lock:
            for c in clients:
                c.close()

if __name__ == "__main__":
    if len(sys.argv) != 3:
        print("Usage: python server.py <HOST> <PORT>")
        exit()
    HOST = sys.argv[1]
    PORT = int(sys.argv[2])
    start_server()
