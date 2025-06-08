CREATE TABLE IF NOT EXISTS room_snapshots (
    room_id INTEGER PRIMARY KEY,
    image BYTEA NOT NULL
);

CREATE TABLE IF NOT EXISTS user_creds (
    id SERIAL PRIMARY KEY,
    username VARCHAR(255) UNIQUE NOT NULL,
    hashed_pw VARCHAR(255) NOT NULL
);
