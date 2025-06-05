import socket
import sqlite3
from hashlib import md5
import threading

HOST = '0.0.0.0'
PORT = 9999
lock = threading.Lock()

db = 'users.db'

Login = '0'
Register = '1'

Success = '0'
AuthenticationSuccessful = '1'
UserAlreadyExists = '2'
InvalidCredentials = '3'
NetworkError = '4'
BadRequest = '5'
UnexpectedError = '6'


delimiter = '|'


def get_conn():
    conn = sqlite3.connect(db)
    return conn


def start_server_auth():
    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.bind((HOST, PORT))
    server.listen(5)
    print(f"[+] Server is listening on {HOST}:{PORT}")


def init_db():
    conn = get_conn()
    cursor = conn.cursor()
    cursor.execute('''
        CREATE TABLE IF NOT EXISTS users (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            username TEXT NOT NULL UNIQUE,
            hashed_pw TEXT NOT NULL
        );
    ''')
    conn.commit()


def login(username, password):
    hashed_pw = md5(password.encode()).hexdigest()
    print(username, hashed_pw)
    conn = get_conn()
    cursor = conn.cursor()
    data = cursor.execute('SELECT * FROM users WHERE username=? AND hashed_pw=?', (username, hashed_pw)).fetchone()
    if data:
        conn.close()
        return True
    return False


def register(username, password):
    conn = get_conn()
    cursor = conn.cursor()
    result = cursor.execute('SELECT * FROM users WHERE username=?', (username, )).fetchall()
    if result:
        conn.close()
        return False
    hashed_pw = md5(password.encode()).hexdigest()
    print(username, hashed_pw)
    result = cursor.execute('INSERT INTO users (username, hashed_pw) VALUES (?, ?)', (username, hashed_pw)).lastrowid
    if result:
        conn.commit()
        conn.close()
        return True
    conn.close()
    return False

def handle_client(client_socket, client_address):
    try:
        print(f"[+] Accepted connection from {client_address}")
        data = client_socket.recv(128)
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
                    response = f'{AuthenticationSuccessful}{delimiter}authenticated'
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
            else:
                response = f'{BadRequest}'

        client_socket.send(response.encode())
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

if __name__ == '__main__':
    init_db()
    run_auth_server()
