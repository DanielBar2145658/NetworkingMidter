using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;

using UnityEngine;
using TMPro;
using System;

public class chatbox : MonoBehaviour
{

    [SerializeField]
    TMP_Text chat_box;

    [SerializeField]
    TMP_InputField inputField;

    private static Socket client = new Socket(AddressFamily.InterNetwork,
    SocketType.Stream, ProtocolType.Tcp);
    private static byte[] buffer = new byte[1024];
    private static byte[] sendBuffer = new byte[1024];

    // Start is called before the first frame update
    void Start()
    {
        client.Connect(IPAddress.Parse("127.0.0.1"), 8888);
        Debug.Log("Connected to server!");


    }

    // Update is called once per frame
    void Update()
    {
        client.BeginReceive(buffer, 0, buffer.Length, 0,
        new AsyncCallback(ReceiveCallback), client);
        Send();

        
    }

    private void Send()
    {
        if (inputField.text != "") 
        {
            if (Input.GetKeyDown(KeyCode.Return)) 
            {
                sendBuffer = Encoding.ASCII.GetBytes(inputField.text);
                client.Send(sendBuffer);
                chat_box.text += inputField.text;
                Thread.Sleep(1000);
                inputField.text = "";

            }
        }

        
    }

    private void ReceiveCallback(IAsyncResult result)
    {
        Socket socket = (Socket)result.AsyncState;
        int rec = socket.EndReceive(result);
        string msg = Encoding.ASCII.GetString(buffer,
        0, rec);

        chat_box.text += msg;

        Debug.Log(msg);

        //Console.WriteLine("Received: " + msg);
        socket.BeginReceive(buffer, 0, buffer.Length, 0,
        new AsyncCallback(ReceiveCallback), socket);
    }
}
