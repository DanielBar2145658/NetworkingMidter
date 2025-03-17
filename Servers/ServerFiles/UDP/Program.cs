using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;


public class UDPserver
{

    public static void StartServer()
    {
        byte[] buffer = new byte[1024];

        byte[] buffer2 = new byte[1024];

//        string sendPosition = "";


        IPAddress ip = IPAddress.Parse("127.0.0.1");

        
        IPEndPoint localEP = new IPEndPoint(ip, 8889);
        //IPEndPoint localEPTCP = new IPEndPoint(ip, 8888);

        Socket Server = new Socket(ip.AddressFamily,
        SocketType.Dgram, ProtocolType.Udp);
       
        
        IPEndPoint client1 = new IPEndPoint(IPAddress.Any, 0);
        EndPoint remoteClient = (EndPoint)client1;

        IPEndPoint client2 = new IPEndPoint(IPAddress.Any, 0);
        EndPoint remoteClient2 = (EndPoint)client2;


        try
        {
            Server.Bind(localEP);
            Console.WriteLine("Waiting for data...");

            //sendBuffer = Encoding.ASCII.GetBytes(sendPosition);

            while (true)
            {
                int recv1 = Server.ReceiveFrom(buffer, ref remoteClient);

                int recv2 = Server.ReceiveFrom(buffer2, ref remoteClient2);

                Console.WriteLine("Received from: {0} ", remoteClient.ToString());
                
                Console.WriteLine("Data: {0}", Encoding.ASCII.GetString(buffer, 0,
                recv1));

                Console.WriteLine("Received from: {0} ", remoteClient2.ToString());

                Console.WriteLine("Data: {0}", Encoding.ASCII.GetString(buffer2, 0,
                recv2));

                //Server.BeginSend(sendBuffer, 0, sendBuffer.Length, 0,
                //    new AsyncCallback(SendCallback), recv1);

                //Server.SendTo(buffer2, 0, buffer2.Length, 0, client1);
                //Server.SendTo(buffer, 0, buffer.Length, 0, client2);


            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    /*
    private static void SendCallback(IAsyncResult result)
    {
        Socket socket = (Socket)result.AsyncState;
        socket.EndSend(result);
    }
    */
    public static int Main(String[] args)
    {
        StartServer();
        return 0;
    }
}