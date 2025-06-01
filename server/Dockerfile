FROM python:3.11-slim

WORKDIR /app
COPY server.py .

RUN pip install --upgrade pip

EXPOSE 1337

CMD ["python", "server.py", "localhost", "1337"]
