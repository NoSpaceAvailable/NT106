import socket
import json
import threading
import sys
import time
import select
import struct

SERVERS = []
LB_HOST = '0.0.0.0'
LB_PORT = 9000

CHAT_SERVER = {"host": "0.0.0.0", "in_port": 10001}
AUTH_SERVER = {"host": "0.0.0.0", "in_port": 10000}

AUTH_HOST = '0.0.0.0'
AUTH_PORT = 10000

HEALTH_CHECK_INTERVAL = 30
SERVER_HEALTH = {}

#Already byte, just append to package
DRAW_FIRST_CONNECT = b'\x01'    # First time client connect to load balancing server must have DRAW_FIRST_CONNECT byte header
DRAW_ALREADY_CONNECT = b'\x02'  # Later package from client to draw server must have DRAW_ALREADY_CONNECT byte header
DRAW_CHANGE_SERVER = b'\x03'    # When a server dead, send a package DRAW_CHANGE_SERVER to draw server     
                                # The package must follow this format: DRAW_CHANGE_SERVER byte | room_id 4 byte    

AUTH_HEADER = b'\x01'
DRAW_HEADER = b'\x02'
CHAT_HEADER = b'\x03'

AUTH_PACK_SIZE = 256

CHAT_CURRENT = b'\x01'
CHAT_OTHER = b'\x02'
CHAT_PACK_SIZE = 1024

CONNECTED_IP = {}

ROOMS = {}

CHANGE_SERVER = {}

round_robin_index = 0
health_lock = threading.Lock()  # Lock for SERVER_HEALTH
selection_lock = threading.Lock()  # Lock for server selection and round_robin_index

def load_servers_from_json(path="servers.json"):
    global SERVERS, SERVER_HEALTH
    try:
        with open(path, "r") as f:
            SERVERS = json.load(f)
            print(f"[+] Loaded {len(SERVERS)} servers from {path}")
            with health_lock:
                SERVER_HEALTH = { f"{server['host']}:{server['out_port']}": {"healthy": True, "last_checked": 0 } for server in SERVERS }
    except Exception as e:
        print(f"[!] Failed to load server list from {path}: {e}")
        sys.exit(1)

def check_health(server):
    with health_lock:
        server_key = f"{server['host']}:{server['out_port']}"
        if server_key not in SERVER_HEALTH:
            SERVER_HEALTH[server_key] = {"healthy": False, "last_checked": 0}
        return SERVER_HEALTH[server_key]["healthy"]

def background_health_check():
    while True:
        current_time = time.time()
        for server in SERVERS:
            server_key = f"{server['host']}:{server['out_port']}"
            load = get_server_load(server)
            is_healthy = load != float('inf')
            status_text = "healthy" if is_healthy else "unhealthy"
            with health_lock:
                if server_key not in SERVER_HEALTH:
                    SERVER_HEALTH[server_key] = {"healthy": False, "last_checked": 0}
                old_status = SERVER_HEALTH[server_key]["healthy"]
                SERVER_HEALTH[server_key]["healthy"] = is_healthy
                SERVER_HEALTH[server_key]["last_checked"] = current_time
                if old_status != is_healthy:
                    print(f"[!] Server {server_key} status changed to: {status_text}")
                else:
                    print(f"[+] Server {server_key}: {status_text} (load: {load if load != float('inf') else 'unreachable'})")
        print(f"[DEBUG] Current health status: {SERVER_HEALTH}")
        time.sleep(HEALTH_CHECK_INTERVAL)

def choose_best_server():
    global round_robin_index
    with selection_lock:
        if not SERVERS:
            print("[-] No servers configured.")
            return None
        healthy_servers = [server for server in SERVERS if check_health(server)]
        if not healthy_servers:
            print("[-] No healthy servers available.")
            return None
        selected_server = healthy_servers[round_robin_index % len(healthy_servers)]
        round_robin_index = (round_robin_index + 1) % len(healthy_servers)
        print(f"[+] Selected server: {selected_server['host']}:{selected_server['in_port']}")
        return selected_server

def get_server_load(server):
    try:
        with socket.create_connection((server["host"], server["out_port"]), timeout=2) as sock:
            data = sock.recv(1024)
            if not data:
                print(f"[!] No data received from {server['host']}:{server['out_port']}")
                return float('inf')
            status = json.loads(data.decode())
            return status.get("load", float('inf'))
    except Exception as e:
        print(f"[!] Cannot get load from {server['host']}:{server['out_port']}: {e}")
        return float('inf')

def forward_data(source_sock, dest_sock, direction, stop_event, type_header):
    try:
        is_client_to_server = "to server" in direction
        first_time = True
        while not stop_event.is_set():
            readable, _, _ = select.select([source_sock], [], [], 1.0)
            if not readable:
                continue
                
            if is_client_to_server:
                if first_time:
                    first_time = False
                else:
                    source_sock.recv(1)

                if type_header == DRAW_HEADER:
                    if CHANGE_SERVER.get(source_sock.getpeername(), 0):
                        data_to_send = DRAW_CHANGE_SERVER + struct.pack('<I', ROOMS[source_sock.getpeername()])
                        dest_sock.sendall(data_to_send)
                        del CHANGE_SERVER[source_sock.getpeername()]
                        continue

                    raw_room_id = source_sock.recv(4)
                    data_to_send = DRAW_FIRST_CONNECT if not CONNECTED_IP.get(source_sock.getpeername(), 0) else DRAW_ALREADY_CONNECT
                    
                    if len(raw_room_id) < 4:
                        if raw_room_id:
                            print(f"[DEBUG] Partial room ID ({len(raw_room_id)} bytes) from {direction}, signaling stop", flush=True)
                        else:
                            print(f"[DEBUG] No more data from {direction}, signaling stop", flush=True)
                        stop_event.set()
                        break

                    data_to_send += raw_room_id
                    room_id = struct.unpack('<I', raw_room_id)[0]
                    print(f"[DEBUG] Room ID: {room_id} from {direction}", flush=True)
                    ROOMS[source_sock.getpeername()] = room_id
                    if CONNECTED_IP.get(source_sock.getpeername(), 0):
                        raw_len = source_sock.recv(4)
                        print(f"[DEBUG] Received length prefix {raw_len.hex()} from {direction}", flush=True)
                        if len(raw_len) < 4:
                            print(f"[DEBUG] Partial length prefix ({len(raw_len)} bytes) from {direction}, signaling stop", flush=True)
                            stop_event.set()
                            break
                        data_to_send += raw_len
                        msg_len = struct.unpack('<I', raw_len)[0]
                        print(f"[DEBUG] Message length: {msg_len} bytes from {direction}", flush=True)

                        data = b''
                        while len(data) < msg_len:
                            packet = source_sock.recv(msg_len - len(data))
                            if not packet:
                                break
                            data += packet
                        
                        if not data:
                            print(f"[DEBUG] No more data from {direction}, signaling stop", flush=True)
                            stop_event.set()
                            break

                        print(f"[DEBUG] Forwarding {len(data)} bytes payload from {direction}: {data[:50].hex()}...", flush=True)
                        data_to_send += data
                        print(f"[DEBUG] Total data to send: {len(data_to_send)} bytes", flush=True)
                        print(f"[DEBUG] Data to send: {data_to_send[:50].hex()}...", flush=True)
                    dest_sock.sendall(data_to_send)
                    CONNECTED_IP[source_sock.getpeername()] = 1

                elif type_header == CHAT_HEADER:
                    data = b''
                    counter = 0
                    while True:
                        pack = source_sock.recv(CHAT_PACK_SIZE)
                        data += pack
                        counter += pack.count(b'|')
                        if counter >= 5:
                            break
                    if not data:
                        print(f"[DEBUG] CHAT No more data from {direction}, signaling stop", flush=True)
                        stop_event.set()
                        break
                    dest_sock.sendall(data)
                
                elif type_header == AUTH_HEADER:
                    data = source_sock.recv(AUTH_PACK_SIZE)
                    
                    if not data:
                        data = source_sock.recv(AUTH_PACK_SIZE)
                        print(f"[DEBUG] AUTH No more data from {direction}, signaling stop", flush=True)
                        stop_event.set()
                        break
                    dest_sock.sendall(data)

            else:
                if type_header == DRAW_HEADER:
                    buffer = b""
                    source_sock.setblocking(False)
                    room_id = source_sock.recv(4)
                    if len(room_id) < 4:
                        if room_id:
                            print(f"[DEBUG] Partial room ID ({len(room_id)} bytes) from {direction}, signaling stop", flush=True)
                        else:
                            print(f"[DEBUG] No more data from {direction}, signaling stop", flush=True)
                        stop_event.set()
                        break
                    raw_len = source_sock.recv(4)
                    msg_len = struct.unpack('<I', raw_len)[0]
                    print(f"[DEBUG] Message length: {msg_len} bytes from {direction}", flush=True)
                    buffer += raw_len
                    packet = b""
                    while len(packet) < msg_len:
                        try:
                            data = source_sock.recv(msg_len - len(packet))
                            if not data:
                                print(f"[DEBUG] No more data from {direction}, signaling stop", flush=True)
                                stop_event.set()
                                break
                            packet += data
                            more_readable, _, _ = select.select([source_sock], [], [], 0.01)
                            if not more_readable:
                                break
                        except BlockingIOError:
                            break
                    source_sock.setblocking(True)

                    if packet:
                        print(f"[DEBUG] Forwarding {len(buffer)} bytes from {direction}: {buffer[:50].hex()}...", flush=True)
                        buffer += packet
                        dest_sock.sendall(buffer)

                    if stop_event.is_set():
                        break
                
                elif type_header == CHAT_HEADER:
                    type_chat = source_sock.recv(1)
                    if type_chat == CHAT_OTHER:
                        data = b""
                        counter = 0
                        while True:
                           pack = source_sock.recv(CHAT_PACK_SIZE)
                           data += pack
                           counter += pack.count(b'|')
                           if counter >= 4:
                               break
                        dest_sock.sendall(buffer)
                    if type_chat == CHAT_CURRENT:
                        dest_sock.sendall(source_sock.recv(1))
                 
                elif type_header == AUTH_HEADER:
                    length = source_sock.recv(4)
                    if len(length) < 4:
                        if length:
                            print(f"[DEBUG] AUTH Partial length prefix ({len(length)} bytes) from {direction}, signaling stop", flush=True)
                        else:
                            print(f"[DEBUG] AUTH No more data from {direction}, signaling stop", flush=True)
                        stop_event.set()
                        break
                    data_to_send = source_sock.recv(struct.unpack('<I', length)[0])
                    if not data_to_send:
                        print(f"[DEBUG] AUTH No more data from {direction}, signaling stop", flush=True)
                        stop_event.set()
                        break
                    dest_sock.sendall(data_to_send)

    except Exception as e:
        print(f"[!] Error forwarding data ({direction}): {e}", flush=True)
        stop_event.set()

def handle_client(client_sock, addr):
    print(f"[+] Client {addr} connected to Load Balancer", flush=True)
    
    max_retries = 3
    retry_count = 0
    server_sock = None
    Type = client_sock.recv(1) #Header 2 byte
    client_sock.recv(1)
    best_server = None
    print(Type)
    while retry_count < max_retries:
        if Type == DRAW_HEADER:
            best_server = choose_best_server()
            if best_server is None:
                print("[-] No available servers.", flush=True)
                if retry_count < max_retries - 1:
                    print(f"[*] Retrying to find server (attempt {retry_count + 1}/{max_retries})")
                    time.sleep(2)
                    retry_count += 1
                    continue
                client_sock.close()
                return
        elif Type == CHAT_HEADER:
            best_server = CHAT_SERVER
        elif Type == AUTH_HEADER:
            best_server = AUTH_SERVER

        try:
            server_sock = socket.create_connection((best_server["host"], best_server["in_port"]), timeout=10)
            server_sock.setsockopt(socket.SOL_SOCKET, socket.SO_KEEPALIVE, 1)
            if Type == DRAW_HEADER:
                if addr not in CONNECTED_IP:
                    CONNECTED_IP[addr] = 0
                    print(f"[+] New client connected: {addr}", flush=True)
                
                if addr in ROOMS:
                    CHANGE_SERVER[addr] = 1
                    print(f"[+] Change {addr} to server {best_server['host']}:{best_server['in_port']} for client {addr}", flush=True)

            print(f"[+] Connected to server {best_server['host']}:{best_server['in_port']} for client {addr}", flush=True)

            stop_event = threading.Event()

            client_to_server = threading.Thread(
                target=forward_data, args=(client_sock, server_sock, f"client {addr} to server", stop_event, Type), daemon=True
            )
            server_to_client = threading.Thread(
                target=forward_data, args=(server_sock, client_sock, f"server to client {addr}", stop_event, Type), daemon=True
            )
            client_to_server.start()
            server_to_client.start()

            # Monitor the connection
            while not stop_event.is_set():
                client_to_server.join(timeout=1.0)
                server_to_client.join(timeout=1.0)
                
                # Check if server is still healthy
                if Type == DRAW_HEADER and not check_health(best_server):
                        print(f"[!] Server {best_server['host']}:{best_server['in_port']} became unhealthy")
                        stop_event.set()
                        break
                    
                if not (client_to_server.is_alive() and server_to_client.is_alive()):
                    break
                    
            if not stop_event.is_set():
                return
                
            print(f"[!] Connection to server lost, attempting to reconnect (attempt {retry_count + 1}/{max_retries})")
            retry_count += 1
            
        except Exception as e:
            print(f"[!] Connection error: {e}")
            if retry_count < max_retries - 1:
                print(f"[*] Retrying connection (attempt {retry_count + 1}/{max_retries})")
                retry_count += 1
                time.sleep(2)
                continue
            break

    print(f"[!] Error handling client {addr}: Max retries exceeded", flush=True)
    try:
        client_sock.close()
        del CONNECTED_IP[addr]
        del ROOMS[addr]
    except:
        pass
    if server_sock:
        try:
            server_sock.close()
        except:
            pass
    print(f"[-] Client {addr} connection closed.", flush=True)

def start_load_balancer():
    health_thread = threading.Thread(target=background_health_check, daemon=True)
    health_thread.start()
    time.sleep(1)
    lb_sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    lb_sock.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    lb_sock.bind((LB_HOST, LB_PORT))
    lb_sock.listen()
    print(f"[*] Load Balancer listening on {LB_HOST}:{LB_PORT}", flush=True)

    try:
        while True:
            client_sock, addr = lb_sock.accept()
            client_sock.setsockopt(socket.SOL_SOCKET, socket.SO_KEEPALIVE, 1)
            threading.Thread(target=handle_client, args=(client_sock, addr), daemon=True).start()
    except KeyboardInterrupt:
        print("[!] Load Balancer shutting down", flush=True)
    finally:
        lb_sock.close()

if __name__ == "__main__":
    if len(sys.argv) < 3:
        print("Usage: python3 server_loadbalancing.py <LB_HOST> <LB_PORT> [<servers.json>]", flush=True)
        sys.exit(1)

    LB_HOST = sys.argv[1]
    LB_PORT = int(sys.argv[2])
    json_path = sys.argv[3] if len(sys.argv) > 3 else "servers.json"

    load_servers_from_json(json_path)
    start_load_balancer()