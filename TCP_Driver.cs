using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//************************************************************
//* TCP DRIVER FOR UNITY                                     *
//************************************************************
//*     BY ISAAC GARCIA PEVERI (IGP TECH BLOG)               *
//*     A DECENTRALIZED TCP DRIVER FOR UNITY THAT            *
//*     CAN RECEIVE STRING VALUES TO BE PASSED TO            *
//*     EVERYTHING YOU WANT: I.E. ANOTHER SCRIPT IN          *
//*     A DIFFERENT OR SAME GAME OBJECT.                     *
//************************************************************
//*     DATE WRITTEN: 10/17/2025                             *
//************************************************************

public class TCP_Driver : MonoBehaviour
{
    static Thread mainThread;
    static Thread tcpThread;
    static TcpListener listener;
    static int tcpPort;
    static TCP_Server tcpServer;
    static List<TCP_Server> clientList = new List<TCP_Server>();
    static string x = "0";
    static string y = "0";
    static string z = "0";
    public string xBuffer = string.Empty;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ReadSettings();
        Debug.Log("ReadSettings()");

        listener = new TcpListener(IPAddress.Any, tcpPort);
        listener.Start();
        Debug.Log("Liestener Started");

        tcpThread = new Thread(TCP_Thread);
        tcpThread.Start();
        Debug.Log("TCP_Thread Started");

        Debug.Log("TCP SERVER RUNNING");
    }

    /// <summary>
    /// Initialize Application Settings
    /// </summary>
    void ReadSettings()
    {
        tcpPort = 64000;
    }

    /// <summary>
    /// TCP thread handling tcp events, connections and so on
    /// </summary>
    /// <param name="arg"></param>
    void TCP_Thread(object arg)
    {
        try
        {
            listener.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), listener);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// A new Client connects to the server
    /// </summary>
    /// <param name="ar"></param>
    void AcceptClient(IAsyncResult ar)
    {
        try
        {
            if (listener.Server.IsBound)
            {
                tcpServer = new TCP_Server(listener.EndAcceptTcpClient(ar));

                tcpServer.getMessage += MessageReceived;
                tcpServer.clientLogout += ClientExited;
                clientList.Add(tcpServer);
                Debug.Log("Client Connected");

                listener.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), listener);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    /// <summary>
    /// Event handler for incoming messages from Client
    /// </summary>
    /// <param name="rClient"></param>
    /// <param name="str"></param>
    void MessageReceived(TcpClient rClient, string str)
    {
        if (str.StartsWith(((char)2).ToString()))
            xBuffer = string.Empty;
        else
            xBuffer += str.Split('\0')[0];

        //Here data will be handled properly by the second script in the controlled object
        //you don't have to put anything here
        //    if (xBuffer.EndsWith("\r"))
        //    {
        //        //Debug.Log("Incoming TCP message: " + xBuffer);
        //        //string[] values = xBuffer.Split(';');
        //        //
        //        //x = values[0];
        //        //y = values[1];
        //        //z = values[2];
        //        //
        //        //xBuffer = "";
        //
        //        if(f)
        //            xBuffer = "";
        //    }
        //}
        //catch (Exception ex)
        //{
        //    Debug.Log(ex.Message);
        //    //throw ex;
        //}
    }

    /// <summary>
    /// Sending data to the client
    /// </summary>
    /// <param name="rClient"></param>
    /// <param name="str"></param>
    void SendMessage(ref TcpClient rClient, string str)
    {
        try
        {
            byte[] myBytes = Encoding.ASCII.GetBytes(str);
            NetworkStream stream = rClient.GetStream();
            stream.Write(myBytes, 0, myBytes.Length);
            stream.Flush();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Disconnection
    /// </summary>
    /// <param name="client"></param>
    void ClientExited(TCP_Server client)
    {
        clientList.Remove(client);
        Debug.Log("... A client disconnected");
    }

    void Update()
    {
        // DO/NOTHING: The object gets the data from this scipt and hanles data properly
    }
}

public class TCP_Server
{
    public event Action<TcpClient, string> getMessage;
    public event Action<TCP_Server> clientLogout;
    private StreamWriter sendMessage;
    public TcpClient listClient;

    public TCP_Server(TcpClient forClient)
    {
        listClient = forClient;
        listClient.GetStream().BeginRead(new byte[] { 0 }, 0, 0, ReadAllClient, null);
    }

    private void ReadAllClient(IAsyncResult ar)
    {
        try
        {
            int bufferSize = 1024;
            byte[] myBufferBytes = new byte[bufferSize];

            if (listClient.Available > 0)
            {
                int bytesRead = listClient.GetStream().Read(myBufferBytes, 0, myBufferBytes.Length);
                string message = Encoding.ASCII.GetString(myBufferBytes, 0, bytesRead);
                getMessage?.Invoke(listClient, message);

                listClient.GetStream().BeginRead(new byte[] { 0 }, 0, 0, ReadAllClient, null);
            }
        }
        catch (Exception)
        {
            clientLogout?.Invoke(this);
        }
    }

    public void Send(string Messsage)
    {
        sendMessage = new StreamWriter(listClient.GetStream());
        sendMessage.WriteLine(Messsage);
        sendMessage.Flush();
    }

    public void Close()
    {
        listClient.GetStream().Close();
        listClient.Close();
    }
}
