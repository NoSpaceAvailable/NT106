import socket
import threading
HOST = '0.0.0.0'
PORT = 10001

AUTH_SERVER_HOST = '127.0.0.1'
AUTH_SERVER_PORT = 10000

Login = '0'
Register = '1'
Verify = '2'
Message = '3'

Success = '0'
AuthenticationSuccessful = '1'
UserAlreadyExists = '2'
InvalidCredentials = '3'
NetworkError = '4'
BadRequest = '5'
UnexpectedError = '6'

delimiter = '|'

PACK_SIZE = 256

clients = []

def check_auth(username, token):
    try:
        conn = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        conn.connect((AUTH_SERVER_HOST, AUTH_SERVER_PORT))
        conn.send(f'{Verify}{delimiter}{username}{delimiter}{token}'.encode())
        response = conn.recv(PACK_SIZE).decode()
        conn.close()
        print(response)
        return response.split(delimiter)[0] == Success
    except Exception as e:
        print(f"[-] Error checking auth: {str(e)}")
        return False
    

def broadcast(message, username_to_exclude):
    pass

    
def handle_client(client_socket, client_address):
    try:
        print(f"[+] Accepted connection from {client_address}")
        data = client_socket.recv(PACK_SIZE)
        print(f"[+] Received: {data.decode()}")

        data = data.decode().split(delimiter)
        if len(data) < 2:
            response = f'{BadRequest}'

        # check authentication
        username = data[0]
        token = data[1]

        if not check_auth(username, token):
            response = f'{InvalidCredentials}'
            client_socket.send(response.encode())
            client_socket.close()
            print(f"[-] Unauthorized for {username} {token}")
            return
    
        # authentication passed
        if (client_socket, username) not in clients:
            # use this info to broadcast message to all users
            clients.append((client_socket, username))

        response = '0'
        client_socket.send(response.encode())

        

        print(f"[+] Sent: {response}")
    except Exception as e:
        print(f"[-] Error handling client {client_address}: {str(e)}")
    finally:
        client_socket.close()
        print(f"[-] Connection closed for {client_address}")


def run_chat_server():
    server = socket.socket(
        family=socket.AF_INET,
        type=socket.SOCK_STREAM,
        proto=socket.IPPROTO_TCP
    )
    server.bind((HOST, PORT))
    server.listen(5)
    print(f"[+] Server is listening on {HOST}:{PORT}")
    
    try:
        while True:
            # Set a timeout to allow checking for KeyboardInterrupt
            server.settimeout(1) 
            try:
                client_socket, client_address = server.accept()
                client_thread = threading.Thread(
                    target=handle_client,
                    args=(client_socket, client_address)
                )
                client_thread.start()
            except socket.timeout:
                continue
    except KeyboardInterrupt:
        print("\n[!] Server shutting down...")
    finally:
        server.close()
        print("[!] Server closed")

if __name__ == "__main__":
    run_chat_server()