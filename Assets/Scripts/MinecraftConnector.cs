using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class MinecraftConnector : MonoBehaviour
{
    public delegate void ReceivedCallback(JSONObject json);
    public static event ReceivedCallback ReceivedEvent = null;

    [Tooltip("The connection port on the machine to use.")]
    public int ConnectionPort = 25566;

#if UNITY_EDITOR

    /// <summary>
    /// Listens for network connections over TCP.
    /// </summary> 
    private TcpListener networkListener;

    /// <summary>
    /// Keeps client information when a connection happens.
    /// </summary>
    private TcpClient networkClient;

    /// <summary>
    /// Tracks if a client is connected.
    /// </summary>
    private bool ClientConnected = false;


    // Use this for initialization
    void Start()
    {
        // Setup the network listener.
        IPAddress localAddr = IPAddress.Parse("127.0.0.1");
        networkListener = new TcpListener(localAddr, ConnectionPort);
        networkListener.Start();

        // Request the network listener to wait for connections asynchronously.
        AsyncCallback callback = new AsyncCallback(OnClientConnect);
        networkListener.BeginAcceptTcpClient(callback, this);
    }

    // Update is called once per frame
    void Update()
    {
        if (ClientConnected == false)
        {
            return;
        }

        var stream = networkClient.GetStream();
        if (stream.DataAvailable == false)
        {
            return;
        }

        int size = ReadSize(stream);
        Debug.Log("Data Size: " + size);

        byte[] data = new byte[size];
        int readSize = 0;
        while (readSize != size)
        {
            readSize += stream.Read(data, readSize, size - readSize);
        }
        var text = System.Text.Encoding.Default.GetString(data);
        Debug.Log(text);

        if (ReceivedEvent != null)
        {
            ReceivedEvent(new JSONObject(text));
        }

        ClientConnected = false;
        networkClient.Close();

        // And wait for the next connection.
        AsyncCallback callback = new AsyncCallback(OnClientConnect);
        networkListener.BeginAcceptTcpClient(callback, this);
    }

    /// <summary>
    /// Called when a client connects.
    /// </summary>
    /// <param name="result">The result of the connection.</param>
    void OnClientConnect(IAsyncResult result)
    {
        if (result.IsCompleted)
        {
            networkClient = networkListener.EndAcceptTcpClient(result);
            if (networkClient != null)
            {
                Debug.Log("Connected");
                ClientConnected = true;
            }
        }
    }

    private int ReadSize(Stream stream)
    {
        byte[] bytes = new byte[4];
        stream.Read(bytes, 0, 4);
        //byte t = bytes[0];
        //bytes[0] = bytes[3];
        //bytes[3] = t;

        //t = bytes[1];
        //bytes[1] = bytes[2];
        //bytes[2] = t;

        return BitConverter.ToInt32(bytes, 0);
    }
#endif
}
