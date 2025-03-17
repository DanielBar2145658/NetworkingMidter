using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;

public class TCPserver
{

    private static byte[] buffer = new byte[1024];
    private static byte[] sendBuffer = new byte[1024];
    private static Socket server;
    private static string sendMsg = "";
    private static List<Socket> clientSockets = new List<Socket>();
    static void Main(string[] args)
    {
        Console.WriteLine("Starting Server...");
        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
        ProtocolType.Tcp);
        server.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888));
        server.Listen(32);
        server.BeginAccept(new AsyncCallback(AcceptCallback), null);
        Thread sendThread = new Thread(new ThreadStart(SendLoop));
        sendThread.Start();
        Console.ReadLine();
    }
    private static void AcceptCallback(IAsyncResult result)
    {
        Socket socket = server.EndAccept(result);
        Console.WriteLine("Client connected!!");
        clientSockets.Add(socket);
        socket.BeginReceive(buffer, 0, buffer.Length, 0,
        new AsyncCallback(ReceiveCallback), socket);
        server.BeginAccept(new AsyncCallback(AcceptCallback), null);
    }
    private static void ReceiveCallback(IAsyncResult result)
    {
        Socket socket = (Socket)result.AsyncState;
        int rec = socket.EndReceive(result);
        string msg = Encoding.ASCII.GetString(buffer, 0, rec);
        Console.WriteLine("Recv: " + msg);

        sendMsg += " " + msg;
        socket.BeginReceive(buffer, 0, buffer.Length, 0,
        new AsyncCallback(ReceiveCallback), socket);
    }
    private static void SendCallback(IAsyncResult result)
    {
        Socket socket = (Socket)result.AsyncState;
        socket.EndSend(result);
    }
    private static void SendLoop()
    {
        while (true)
        {
            //Protect this with a mutex
            sendBuffer = Encoding.ASCII.GetBytes(sendMsg);
            // Send updates to all clients in the list
            foreach (var socket in clientSockets)
            {
                Console.WriteLine("Sent to: " +
                socket.RemoteEndPoint.ToString());
                socket.BeginSend(sendBuffer, 0, sendBuffer.Length, 0,
                new AsyncCallback(SendCallback), socket);
            }
            sendMsg = "";
            Thread.Sleep(1000);
        }
    }
}