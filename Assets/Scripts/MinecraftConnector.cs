using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class MinecraftConnector : MonoBehaviour
{

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
#endif
}
