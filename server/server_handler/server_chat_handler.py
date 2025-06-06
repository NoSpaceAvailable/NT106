import socket
import threading

HOST = '0.0.0.0'
PORT = 10001

AUTH_SERVER_HOST = '127.0.0.1'
AUTH_SERVER_PORT = 10000

# Action Codes
Login = '0'
Register = '1'
Verify = '2'
SendMessage = '3'
Broadcast = '4'

# Status Codes
Success = '0'
AuthenticationSuccessful = '1'
UserAlreadyExists = '2'
InvalidCredentials = '3'
NetworkError = '4'
BadRequest = '5'
UnexpectedError = '6'

# Message Types
Text = '0'
Image = '1'


delimiter = '|'
PACK_SIZE = 1024
clients = []  # List to store (client_socket, username) for broadcasting


def check_auth(username, token):
    try:
        sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        sock.connect((AUTH_SERVER_HOST, AUTH_SERVER_PORT))
        sock.send(f'{Verify}{delimiter}{username}{delimiter}{token}'.encode())
        response = sock.recv(PACK_SIZE).decode()
        sock.close()
        if response.split(delimiter)[0] == Success:
            return True
        return False
    except Exception as e:
        print(f"[-] Error checking auth: {str(e)}")
        return False

def broadcast_message(message_payload, message_type, message_sender_username, originating_socket):
    """
    Broadcasts a message to all connected and authenticated clients except the sender.
    The message_payload should be the actual content (example: "Hahahahaha lmao ez ez").
    The server will construct the full protocol message for broadcasting.
    """
    clients_copy = list(clients) # Iterate over a copy
    for client_sock, client_user in clients_copy:
        if client_sock != originating_socket:
            try:
                # Construct the message as per client expectation for a received message
                # Structure of a broadcast message:
                #       action | username | type | message |
                # Where action is Broadcast
                broadcast_msg_str = f'{Broadcast}|{message_sender_username}|{message_type}|{message_payload}|'
                print(f'[+] Broadcasting payload: {broadcast_msg_str}')
                client_sock.sendall(broadcast_msg_str.encode())
            except Exception as e:
                print(f"[-] Error broadcasting to {client_user}: {str(e)}. Removing client.")
                clients.remove((client_sock, client_user)) # Remove problematic client
                client_sock.close()


def handle_client(client_socket: socket.socket, client_address):
    print(f"[+] Accepted connection from {client_address}")
    current_session_username = None
    is_session_authenticated = False

    try:
        while True:
            try:
                client_socket.settimeout(3) # Timeout for recv
                data = b''
                counter = 0
                while True:
                    print(counter)
                    pack = client_socket.recv(PACK_SIZE)
                    print(pack)
                    data += pack
                    counter += pack.count(b'|')     # receive until the total of delimiter found is >= 5
                    if counter >= 5:
                        break

                print('Done receiving!')

                if not data:
                    print(f"[-] Client {client_address} (User: {current_session_username}) disconnected (received empty data).")
                    break

                data = data.strip(b'\0').decode()

                parts = data.split(delimiter, maxsplit=5)
                if len(parts) != 6:     # action | username | token | type | message |
                    print(len(parts)) 
                    client_socket.send(f'{BadRequest}'.encode())
                    print(f"[-] Sent BadRequest to {client_address} (incorrect parts).")
                    continue

                action, username, token, msg_type, message, _ = parts

                if msg_type == Text:
                    print(f"[+] Received from {client_address} (User: {current_session_username}): {data}")
                    msg_type = Text
                elif msg_type == Image:
                    print(f"[+] Received an image from {client_address} (User: {current_session_username})")
                    msg_type = Image
                else:
                    print("[-] Invalid message type!")
                    client_socket.send(f'{BadRequest}'.encode())
                    continue

                if action == SendMessage:
                    if not check_auth(username, token):
                        client_socket.send(f'{InvalidCredentials}'.encode())
                        print(f"[-] Sent InvalidCredentials to {username}@{client_address}.")
                        continue
                    
                    # Authenticated for this message
                    if not is_session_authenticated:
                        current_session_username = username
                        is_session_authenticated = True
                        # Add to clients list for broadcasting if not already there
                        if not any(sock == client_socket for sock, _ in clients):
                            clients.append((client_socket, current_session_username))
                        print(f"[+] User '{current_session_username}' from {client_address} session authenticated.")

                    if not message: # Empty message content
                        print(f"[-] User '{current_session_username}' sent an empty message, dropping...")
                        client_socket.send(f'{Success}'.encode()) # Do nothing else
                        continue
                    
                    # Process and acknowledge the message
                    print(f"[+] Message from '{current_session_username}': {message}")
                    client_socket.send(f'{Success}'.encode()) # Send '0' for success
                    
                    # Broadcast the message
                    print(f'[+] Broadcasting message...')
                    broadcast_message(
                        message_payload = f"{message}", 
                        message_type = msg_type,
                        message_sender_username = current_session_username, 
                        originating_socket = client_socket
                    )

                else:
                    client_socket.send(f'{BadRequest}'.encode()) # Unknown action
                    print(f"[-] Sent BadRequest to {client_address} (unknown action: {action}).")
            
            except socket.timeout:
                # print(f"[?] Timeout waiting for data from {client_address}. Still connected.")
                # You can send a PING here or just continue
                continue 
            except (ConnectionResetError, ConnectionAbortedError) as e:
                print(f"[-] Client {client_address} (User: {current_session_username}) connection error: {e}")
                break
            except Exception as e:
                print(f"[-] Error processing message from {client_address} (User: {current_session_username}): {str(e)}")
                try:
                    client_socket.send(f'{UnexpectedError}'.encode())
                except Exception as send_e:
                    print(f"[-] Failed to send UnexpectedError to {client_address}: {send_e}")
                break # Exit loop on other critical errors

    finally:
        print(f"[-] Closing connection for {client_address} (User: {current_session_username}).")
        # Remove client from list
        clients[:] = [(sock, user) for sock, user in clients if sock != client_socket]
        client_socket.close()


def run_chat_server():
    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.bind((HOST, PORT))
    server.listen(5)
    print(f"[+] Server is listening on {HOST}:{PORT}")
    
    try:
        while True:
            server.settimeout(1.0) # Timeout for accept, to allow KeyboardInterrupt
            try:
                client_socket, client_address = server.accept()
                # No timeout on individual client sockets here, set in handle_client
                client_thread = threading.Thread(target=handle_client, args=(client_socket, client_address))
                client_thread.daemon = True # Allow main program to exit even if threads are running
                client_thread.start()
            except socket.timeout:
                continue # Go back to server.accept()
    except KeyboardInterrupt:
        print("\n[!] Server shutting down...")
    finally:
        for sock, _ in clients: # Close all active client connections
            sock.close()
        server.close()
        print("[!] Server closed")

if __name__ == "__main__":
    run_chat_server()