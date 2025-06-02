import socket
import json
import threading
import sys

SERVERS = []
LB_HOST = '0.0.0.0'
LB_PORT = 9000

def load_servers_from_json(path="servers.json"):
    global SERVERS
    try:
        with open(path, "r") as f:
            SERVERS = json.load(f)
            print(f"[+] Loaded {len(SERVERS)} servers from {path}")
    except Exception as e:
        print(f"[!] Failed to load server list from {path}: {e}")
        exit(1)

def get_server_load(server):
    try:
        with socket.create_connection((server["host"], server["out_port"]), timeout=2) as sock:
            data = sock.recv(1024)
            status = json.loads(data.decode())
            return status.get("load", float('inf'))
    except Exception as e:
        print(f"[!] Cannot get load from {server['host']}:{server['out_port']}: {e}")
        return float('inf')

def choose_best_server():
    best_server = None
    lowest_load = float('inf')

    for server in SERVERS:
        load = get_server_load(server)
        if load < lowest_load:
            lowest_load = load
            best_server = server

    return best_server

def handle_client(client_sock, addr):
    print(f"[+] Client {addr} connected to Load Balancer")
    best_server = choose_best_server()
    if best_server is None:
        print("[-] No available servers.")
        client_sock.close()
        return

    try:
        response = json.dumps({
            "host": best_server["host"],
            "port": best_server["in_port"]
        }).encode("utf-8")
        client_sock.sendall(response)
    except Exception as e:
        print(f"[!] Error sending redirect info to client: {e}")
    finally:
        client_sock.close()
        print(f"[-] Client {addr} handled and closed.")

def start_load_balancer():
    lb_sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    lb_sock.bind((LB_HOST, LB_PORT))
    lb_sock.listen()
    print(f"[*] Load Balancer listening on {LB_HOST}:{LB_PORT}")

    try:
        while True:
            client_sock, addr = lb_sock.accept()
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
