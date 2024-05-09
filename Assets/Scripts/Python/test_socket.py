import socket, time, random

host, port = "127.0.0.1", 25001
list_of_moves = ["Punch", "Hook", "Block"]

# SOCK_STREAM means TCP socket
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

sock.connect((host, port))


for i in range(10):
    # Connect to the server and send the data
    sock.sendall(random.choice(list_of_moves).encode("utf-8"))
    response = sock.recv(1024).decode("utf-8")
    print (response)
    time.sleep(6)

sock.close()