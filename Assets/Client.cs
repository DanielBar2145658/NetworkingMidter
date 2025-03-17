using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Network
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;



public class Client : MonoBehaviour
{
    public GameObject player;
    public GameObject ghost;

    private static byte[] buffer = new byte[1024];
    //private static byte[] sendBuffer = new byte[1024];
    private IPEndPoint remoteEP;
    private static Socket clientSocket;

    public Transform spawnPoint;

    public void StartClient()
    {
        try
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            remoteEP = new IPEndPoint(ip, 8889);

            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);



        }
        catch (Exception e)
        {
            Debug.LogWarning(e.ToString());

        }




    }





    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Cube");
        ghost = GameObject.Find("Ghost");
        StartClient();



    }

    // Update is called once per frame
    void Update()
    {
        buffer = Encoding.ASCII.GetBytes(player.transform.position.ToString());
        clientSocket.SendTo(buffer, remoteEP);
        //clientSocket.Send(sendBuffer);

        clientSocket.Receive(buffer,0,buffer.Length,0);
        Receive();
        //clientSocket.BeginReceive(buffer, 0, buffer.Length, 0,
        //new AsyncCallback(ReceiveCallback), clientSocket);
        

        //Thread.Sleep(1000);
        


    }



    void Receive()
    {
        

        string position = Encoding.ASCII.GetString(buffer,0,buffer.Length);

        if (position.StartsWith("(") && position.EndsWith(")"))
        {
            position = position.Substring(1, position.Length - 2);
        }

        


        string[] stringArray = position.Split(",");

        Vector3 r = new Vector3(float.Parse(stringArray[0]), float.Parse(stringArray[1]), float.Parse(stringArray[2]));

        Debug.Log(r);

        ghost.transform.position = r;







        /*socket.BeginReceive(buffer, 0, buffer.Length, 0,
        new AsyncCallback(ReceiveCallback), socket);
        */
    }

}
