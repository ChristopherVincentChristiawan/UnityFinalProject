import numpy as np
import socket
path = "./darrenmo"
def setup_model():
    device = torch.device('cuda' if torch.cuda.is_available() else 'cpu')
    model = torch.hub.load('ultralytics/yolov5', 'custom', path='Assets\Scripts\Python\yolov5s-cls.pt')
    model.load_state_dict(torch.load('Assets\Scripts\Python\darrenmodelv3.pt'))

    return device, model

def initialize_socket():
    host, port = "127.0.0.1", 25001

    # SOCK_STREAM means TCP socket
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    return sock, host, port

def capture_movements(cap, device, model, transform, classes):
    frames = []
    frame_count = 0
    for i in range(1, 31):
        ret, frame = cap.read()
        if not ret:
            break
        
        # increment frame count
        frame_count += 1
        
        # preprocess the frame
        frame = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
        frame = np.array(frame)
        frame = np.stack((frame,)*3, axis=-1)
        
        frames.append(transform(frame))
        
        # predict every 30 frames
        if frame_count % 30 == 0:
            # stack the frames into a batch
    #         batch = np.array(frames, dtype='float32')/255
            batch = torch.stack(frames).to(device)
            
            # make the prediction
            with torch.no_grad():
                output = model(batch)
                _, predicted = torch.max(output.data, 1)
            
            # print the predicted class
            predicted_move = classes[predicted[0]]    
            return predicted_move
        
        if cv2.waitKey(1) == ord('q'):
            break

# Connect Socket First
print("Socket is being initialized")
sock, host, port = initialize_socket()
sock.connect((host, port))
print("Socket has been connected")

# Define classes
classes = ['Block', 'Hook', 'Punch']

# Initialize variables
class_counts = {cls: 0 for cls in classes}
img_size = 320

print("torchvision being loaded")
import torchvision.transforms as transforms
transform = transforms.Compose([
    transforms.ToPILImage(),
    transforms.ToTensor()
])
print("transform object initialized")

# initialize webcam
print("initializing webcam")
import cv2
cap = cv2.VideoCapture(0)
cap.set(cv2.CAP_PROP_FRAME_WIDTH, 320)
cap.set(cv2.CAP_PROP_FRAME_HEIGHT, 320)
print("Webcam done setup")

print("Setup Model is Called")
import torch
device, model = setup_model()
print("Setup model done")


predicted_move = capture_movements(cap, device, model, transform, classes)

sock.sendall(predicted_move.encode("utf-8"))
response = sock.recv(1024).decode("utf-8")

while True:
    if response == "go":
        frames = []
        frame_count = 0
        for i in range(1, 31):
            ret, frame = cap.read()
            if not ret:
                break
            
            # increment frame count
            frame_count += 1
            
            # preprocess the frame
            frame = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
            frame = np.array(frame)
            frame = np.stack((frame,)*3, axis=-1)
            
            frames.append(transform(frame))
            
            # predict every 30 frames
            if frame_count % 30 == 0:
                # stack the frames into a batch
        #         batch = np.array(frames, dtype='float32')/255
                batch = torch.stack(frames).to(device)
                
                # make the prediction
                with torch.no_grad():
                    output = model(batch)
                    _, predicted = torch.max(output.data, 1)
                
                # print the predicted class
                predicted_move = classes[predicted[0]]    
                # return predicted_move
            
            if cv2.waitKey(1) == ord('q'):
                break

        print(predicted_move)
        sock.sendall(predicted_move.encode("utf-8"))
        response = sock.recv(1024).decode("utf-8")

    if response == "stop":
        break
cap.release()
cv2.destroyAllWindows()
