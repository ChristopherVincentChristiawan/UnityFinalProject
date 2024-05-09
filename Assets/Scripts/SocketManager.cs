using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Diagnostics;

public class SocketManager : MonoBehaviour
{
    #region Singleton

    public static SocketManager instance;
    void Awake()
    {
        instance = this;
    }

    #endregion

    public string ReceivedMove;
    Thread thread;
    public int connectionPort = 25001;
    TcpListener server;
    TcpClient client;
    NetworkStream nwStream;
    bool running;

    void Start()
    {


        // Start the process
        ThreadStart ts = new ThreadStart(GetData);
        thread = new Thread(ts);
        thread.Start();

        // Starts the python 
        string pythonPath = "Assets\\Scripts\\Python\\python\\python.exe"; // or provide the full path to the Python executable
        string scriptPath = "Assets/Scripts/Python/camera_input.py";
        
        ProcessStartInfo startInfo = new ProcessStartInfo(pythonPath, scriptPath);
        startInfo.UseShellExecute = true;
        startInfo.RedirectStandardOutput = false;
        
        // Hide the console window
        // startInfo.CreateNoWindow = true;
        // startInfo.WindowStyle = ProcessWindowStyle.Hidden;

        Process process = new Process();
        process.StartInfo = startInfo;
        process.Start();
    }
    
    void GetData()
    {
        // Create the server
        server = new TcpListener(IPAddress.Any, connectionPort);
        server.Start();

        // Create a client to get the data stream
        client = server.AcceptTcpClient();

        // Start listening
        running = true;
        while (running)
        {
            Connection();
        }
        ResetMove();
    }

    void Connection()
    {
        // Read data from the network stream
        nwStream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

        // Decode the bytes into a string
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        
        // Make sure we're not getting an empty string
        //dataReceived.Trim();
        if (dataReceived != null && dataReceived != "")
        {
            ReceivedMove = dataReceived;
        }
    }

    public void ResetMove()
    {
        ReceivedMove = "";
        ReplyBack("go");
    }

    public void StopConnection()
    {   
        running = false;
    }

    public void ReplyBack(string message)
    {
        byte[] myWriteBuffer = Encoding.ASCII.GetBytes(message);

        nwStream.Write(myWriteBuffer, 0, myWriteBuffer.Length);
    }
}