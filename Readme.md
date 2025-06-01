# Client activities

## Offline drawing

- Free draw
- Shape draw

## Online drawing

### Join room

- Have to join a room before online drawing
- The room id of client would be handled by server

### Send draw packages

- Send draw actions with format:
    - `(int) data_len` + `(byte []) data[data_len]`

### Receive draw packages
- Receive draw actions with format:
    - `(int) data_len` + `(byte []) data[data_len]`

- Receive draw area when join new room with format:
    - `(int) data_len` + `(byte []) data[data_len]`

# Server activities

## On client connect

- Create thread to handle client draw packages
- Add client to room_id client want to join
    - If no room_id yet, create new room that have room_id
    - If room_id exist, add client to that room
- Send client current room draw area data with format:
    - `(int) data_len` + `(byte []) data[data_len]`

## On client drawing

- Broadcast client draw packages to other client in room_id
- Draw the draw packages from client to room draw area

## On client disconnect

- Remove client from room

# Databases requirements:

- User credentials table
- Room draw area table