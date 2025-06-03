import socket
import json
import threading
import sys
import time

SERVERS = []
LB_HOST = '0.0.0.0'
LB_PORT = 9000
HEALTH_CHECK_INTERVAL = 30
SERVER_HEALTH = {}
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

def forward_data(source_sock, dest_sock, direction, stop_event):
    """Forward all available data in a single sendall call until stop_event is set."""
    try:
        while not stop_event.is_set():
            # Use select to check if data is available with a short timeout
            readable, _, _ = select.select([source_sock], [], [], 0.1)
            if not readable:
                continue  # No data available, loop again

            # Collect all available data
            buffer = b""
            while True:
                try:
                    # Set socket to non-blocking temporarily
                    source_sock.setblocking(False)
                    data = source_sock.recv(1024)
                    if not data:
                        print(f"[DEBUG] No more data from {direction}, signaling stop")
                        stop_event.set()  # Client disconnected
                        break
                    buffer += data
                except BlockingIOError:
                    # No more data available right now
                    break
                finally:
                    source_sock.setblocking(True)  # Restore blocking mode

            if buffer:
                print(f"[DEBUG] Forwarding {len(buffer)} bytes from {direction}: {buffer[:50].hex()}...")
                dest_sock.sendall(buffer)

            if stop_event.is_set():
                break

def handle_client(client_sock, addr):
    print(f"[+] Client {addr} connected to Load Balancer")
    best_server = choose_best_server()
    if best_server is None:
        print("[-] No available servers.")
        client_sock.close()
        return

    server_sock = None
    try:
        server_sock = socket.create_connection((best_server["host"], best_server["in_port"]), timeout=10)
        server_sock.setsockopt(socket.SOL_SOCKET, socket.SO_KEEPALIVE, 1)
        print(f"[+] Connected to server {best_server['host']}:{best_server['in_port']} for client {addr}")

        # Create a stop event to synchronize thread termination
        stop_event = threading.Event()

        client_to_server = threading.Thread(
            target=forward_data, args=(client_sock, server_sock, f"client {addr} to server", stop_event), daemon=True
        )
        server_to_client = threading.Thread(
            target=forward_data, args=(server_sock, client_sock, f"server to client {addr}", stop_event), daemon=True
        )
        client_to_server.start()
        server_to_client.start()

        client_to_server.join()
        server_to_client.join()

    except Exception as e:
        print(f"[!] Error handling client {addr}: {e}")
    finally:
        try:
            client_sock.close()
        except:
            pass
        if server_sock:
            try:
                server_sock.close()
            except:
                pass
        print(f"[-] Client {addr} connection closed.")

def start_load_balancer():
    health_thread = threading.Thread(target=background_health_check, daemon=True)
    health_thread.start()
    time.sleep(1)
    lb_sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    lb_sock.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    lb_sock.bind((LB_HOST, LB_PORT))
    lb_sock.listen()
    print(f"[*] Load Balancer listening on {LB_HOST}:{LB_PORT}")

    try:
        while True:
            client_sock, addr = lb_sock.accept()
            client_sock.setsockopt(socket.SOL_SOCKET, socket.SO_KEEPALIVE, 1)
            threading.Thread(target=handle_client, args=(client_sock, addr), daemon=True).start()
    except KeyboardInterrupt:
        print("[!] Load Balancer shutting down")
    finally:
        lb_sock.close()

if __name__ == "__main__":
    if len(sys.argv) < 3:
        print("Usage: python3 server_loadbalancing.py <LB_HOST> <LB_PORT> [<servers.json>]")
        sys.exit(1)

    LB_HOST = sys.argv[1]
    LB_PORT = int(sys.argv[2])
    json_path = sys.argv[3] if len(sys.argv) > 3 else "servers.json"

    load_servers_from_json(json_path)
    start_load_balancer()