import socket
import threading
import struct
from collections import defaultdict

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

CURRENT_CLIENT = b'\x01'
OTHER_CLIENT = b'\x02'

delimiter = '|'
PACK_SIZE = 1024
clients = {}  # Dict to store (client_socket, username) for broadcasting

def check_auth(username, token):
    try:
        sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        sock.connect((AUTH_SERVER_HOST, AUTH_SERVER_PORT))
        sock.send(f'{Verify}{delimiter}{username}{delimiter}{token}'.encode())
        response = sock.recv(PACK_SIZE).decode()[4:]
        sock.close()
        if response.split(delimiter)[0] == Success:
            return True
        return False
    except Exception as e:
        print(f"[-] Error checking auth: {str(e)}")
        return False

def broadcast_message(message_payload, message_type, message_sender_username, originating_socket, room):
    """
    Broadcasts a message to all clients in the specified room_id, excluding the sender.
    """
    if room not in clients:
        print(f"[-] Room ID {room} not found.")
        return

    clients_copy = list(clients[room])
    for client_sock, client_user in clients_copy:
        if client_sock != originating_socket:
            try:
                # Construct broadcast message
                broadcast_msg_str = f'{Broadcast}|{message_sender_username}|{message_type}|{message_payload}|'
                print(f'[+] Broadcasting payload to room {room}: {broadcast_msg_str}')
                client_sock.sendall(OTHER_CLIENT + broadcast_msg_str.encode())
            except Exception as e:
                print(f"[-] Error broadcasting to {client_user}: {str(e)}. Removing client.")
                clients[room].remove((client_sock, client_user))
                client_sock.close()

    # Clean up empty room
    if not clients[room]:
        del clients[room]

def remove_client_from_all_rooms(client_socket):
    for room_id in list(clients.keys()):
        original_len = len(clients[room_id])
        clients[room_id] = [(sock, user) for sock, user in clients[room_id] if sock != client_socket]

        if len(clients[room_id]) < original_len:
            print(f"[-] Removed client from room {room_id}")

        if not clients[room_id]:
            del clients[room_id]

def handle_client(client_socket: socket.socket, client_address):
    print(f"[+] Accepted connection from {client_address}")
    current_session_username = None
    is_session_authenticated = False
    room_id = None
    try:
        while True:
            try:
                room_id = client_socket.recv(4)
                if not room_id:
                    print(f"[-] Missing client room id")
                    break
                room_id = struct.unpack("<I", room_id)[0]
                print(f"[+] Received room {room_id}")
            except:
                print(f"[-] Error while receiving room id")
                break
            
            try:
                data = b''
                counter = 0
                while True:
                    pack = client_socket.recv(PACK_SIZE)
                    data += pack
                    counter += pack.count(b'|')     # receive until the total of delimiter found is >= 5
                    if counter >= 5:
                        break

                print('Done receiving!')

                if not data:
                    print(f"[-] Client {client_address} (User: {current_session_username}) disconnected (received empty data).")
                    break
                print(f"[+] Received data from {client_address} (User: {current_session_username}): {data}")
                data = data.strip(b'\0').decode()

                parts = data.split(delimiter, maxsplit=5)
                if len(parts) != 6:     # action | username | token | type | message |
                    print(len(parts)) 
                    client_socket.send(CURRENT_CLIENT + f'{BadRequest}'.encode())
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
                        client_socket.send(CURRENT_CLIENT + f'{InvalidCredentials}'.encode())
                        print(f"[-] Sent InvalidCredentials to {username}@{client_address}.")
                        continue
                    print("Authenticated user:", username)
                    # Authenticated for this message
                    if not is_session_authenticated:
                        current_session_username = username
                        is_session_authenticated = True
                        # Add to clients list for broadcasting if not already there
                        if room_id not in clients:
                            clients[room_id] = []
                        clients[room_id].append((client_socket, current_session_username))
                        print(f"[+] User '{current_session_username}' from {client_address} session authenticated.")

                    if not message: # Empty message content
                        print(f"[-] User '{current_session_username}' sent an empty message, dropping...")
                        client_socket.send(CURRENT_CLIENT + f'{Success}'.encode()) # Do nothing else
                        continue
                    
                    # Process and acknowledge the message
                    print(f"[+] Message from '{current_session_username}': {message}")
                    client_socket.send(CURRENT_CLIENT + f'{Success}'.encode()) # Send '0' for success
                    
                    # Broadcast the message
                    print(f'[+] Broadcasting message...')

                    broadcast_message(
                        message_payload = f"{message}", 
                        message_type = msg_type,
                        message_sender_username = current_session_username, 
                        originating_socket = client_socket,
                        room = room_id
                    )

                else:
                    client_socket.send(CURRENT_CLIENT + f'{BadRequest}'.encode()) # Unknown action
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
        remove_client_from_all_rooms(client_socket)
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