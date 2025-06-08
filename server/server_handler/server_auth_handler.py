import socket
import sqlite3
from hashlib import md5
import threading
import struct
import psycopg2

HOST = '0.0.0.0'
PORT = 10000
lock = threading.Lock()


conn = psycopg2.connect(
    host="database",
    port=5432,
    dbname="rooms",
    user="postgresql",
    password="095f75fe10f6541b51d4a1ca84a993ac274ac14bee50a6c7a7df6c79cffd1946"
)

secret = b'0b6825b36ec7459e37d7e463a19daa44a9b5cc63dd98146fccb525552dd086ca'

Login = '0'
Register = '1'
Verify = '2'

Success = '0'
AuthenticationSuccessful = '1'
UserAlreadyExists = '2'
InvalidCredentials = '3'
NetworkError = '4'
BadRequest = '5'
UnexpectedError = '6'

delimiter = '|'


PACK_SIZE = 256


def sign_token(username: str):
    return md5(username.encode() + secret).hexdigest()


def start_server_auth():
    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.bind((HOST, PORT))
    server.listen(5)
    print(f"[+] Server is listening on {HOST}:{PORT}")

def login(username, password):
    hashed_pw = md5(password.encode()).hexdigest()
    try:
        with conn.cursor() as cursor:
            cursor.execute('SELECT * FROM user_creds WHERE username=%s AND hashed_pw=%s', (username, hashed_pw))
            data = cursor.fetchone()
            return data is not None
    except Exception as e:
        print(f"[-] Login DB error: {e}")
        return False

def register(username, password):
    hashed_pw = md5(password.encode()).hexdigest()
    try:
        with conn.cursor() as cursor:
            cursor.execute('SELECT * FROM user_creds WHERE username=%s', (username,))
            result = cursor.fetchall()
            if result:
                return False  # Username exists

            cursor.execute('INSERT INTO user_creds (username, hashed_pw) VALUES (%s, %s)', (username, hashed_pw))
            conn.commit()
            return True
    except Exception as e:
        print(f"[-] Register DB error: {e}")
        conn.rollback()
        return False

def handle_client(client_socket, client_address):
    try:
        print(f"[+] Accepted connection from {client_address}")
        data = client_socket.recv(PACK_SIZE)
        print(f"[+] Received: {data.decode()}")

        data = data.decode().split(delimiter)
        if len(data) < 3:
            response = f'{BadRequest}'
        else:
            if data[0] == Login:
                username = data[1]
                password = data[2]
                status = login(username, password)
                if status:
                    response = f'{AuthenticationSuccessful}{delimiter}{sign_token(username)}'
                else:
                    response = f'{InvalidCredentials}'
            elif data[0] == Register:
                username = data[1]
                password = data[2]
                result = register(username, password)
                if result:
                    response = f'{Success}'
                else:
                    response = f'{UserAlreadyExists}'
            elif data[0] == Verify:
                username = data[1]
                token = data[2]
                if sign_token(username) == token:
                    response = f'{Success}'
                else:
                    response = f'{InvalidCredentials}'
            else:
                response = f'{BadRequest}'

        client_socket.send(struct.pack("<I", len(response)) + response.encode())
        print(f"[+] Sent: {response}")
    except Exception as e:
        print(f"[-] Error handling client {client_address}: {str(e)}")
    finally:
        client_socket.close()
        print(f"[-] Connection closed for {client_address}")

def run_auth_server():
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
            # Set a timeout to allow checking for CTRL + C
            # please remove this line on prod
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

if __name__ == '__main__':
    run_auth_server()
