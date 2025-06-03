#!/bin/bash

SERVERS_FILE="servers.json"
COMPOSE_FILE="docker-compose.generated.yaml"

if [ ! -f "$SERVERS_FILE" ]; then
  echo "[!] $SERVERS_FILE not found."
  exit 1
fi

echo "version: '3.8'" > $COMPOSE_FILE
echo "services:" >> $COMPOSE_FILE

# Extract Load Balancer info
LB_HOST=$(jq -r '.[0].host' "$SERVERS_FILE")
LB_PORT=$(jq -r '.[0].port' "$SERVERS_FILE")

echo "  loadbalancer:" >> $COMPOSE_FILE
echo "    build:" >> $COMPOSE_FILE
echo "      context: ." >> $COMPOSE_FILE
echo "      dockerfile: Dockerfile.loadserver" >> $COMPOSE_FILE
echo "    ports:" >> $COMPOSE_FILE
echo "      - \"${LB_PORT}:${LB_PORT}\"" >> $COMPOSE_FILE
echo "    environment:" >> $COMPOSE_FILE
echo "      - HOST=${LB_HOST}" >> $COMPOSE_FILE
echo "      - PORT=${LB_PORT}" >> $COMPOSE_FILE
echo "    depends_on:" >> $COMPOSE_FILE

# Process draw servers
TOTAL_SERVERS=$(jq '. | length' "$SERVERS_FILE")
for i in $(seq 1 $((TOTAL_SERVERS - 1))); do
  NAME=$(jq -r ".[$i].host" "$SERVERS_FILE")
  echo "      - $NAME" >> $COMPOSE_FILE
done

echo "    networks:" >> $COMPOSE_FILE
echo "      - drawnet" >> $COMPOSE_FILE
echo "" >> $COMPOSE_FILE

# Add draw servers
for i in $(seq 1 $((TOTAL_SERVERS - 1))); do
  HOST=$(jq -r ".[$i].host" "$SERVERS_FILE")
  IN_PORT=$(jq -r ".[$i].in_port" "$SERVERS_FILE")
  OUT_PORT=$(jq -r ".[$i].out_port" "$SERVERS_FILE")

  echo "  $HOST:" >> $COMPOSE_FILE
  echo "    build:" >> $COMPOSE_FILE
  echo "      context: ." >> $COMPOSE_FILE
  echo "      dockerfile: Dockerfile.server" >> $COMPOSE_FILE
  echo "    environment:" >> $COMPOSE_FILE
  echo "      - IN_PORT=$IN_PORT" >> $COMPOSE_FILE
  echo "      - OUT_PORT=$OUT_PORT" >> $COMPOSE_FILE
  echo "    networks:" >> $COMPOSE_FILE
  echo "      - drawnet" >> $COMPOSE_FILE
  echo "" >> $COMPOSE_FILE
done

# Add network
echo "networks:" >> $COMPOSE_FILE
echo "  drawnet:" >> $COMPOSE_FILE

echo "[*] Generated $COMPOSE_FILE"
# docker-compose -f $COMPOSE_FILE up --build
