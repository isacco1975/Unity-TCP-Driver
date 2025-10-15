using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace TCPSerrialDriverC
{
    public class Controller2 : MonoBehaviour
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
        static string xBuffer = "";

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
        static void ReadSettings()
        {
            tcpPort = 64000;
        }

        /// <summary>
        /// TCP thread handling tcp events, connections and so on
        /// </summary>
        /// <param name="arg"></param>
        static void TCP_Thread(object arg)
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
        static void AcceptClient(IAsyncResult ar)
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
        static void MessageReceived(TcpClient rClient, string str)
        {
            if (str.StartsWith(((char)2).ToString()))
                xBuffer = string.Empty;
            else
                xBuffer += str.Split('\0')[0];

            try
            {
                if (xBuffer.EndsWith("\r"))
                {
                    Debug.Log("Incoming TCP message: " + xBuffer);
                    string[] values = xBuffer.Split(';');

                    x = values[0];
                    y = values[1];
                    z = values[2];

                    xBuffer = "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sending data to the client
        /// </summary>
        /// <param name="rClient"></param>
        /// <param name="str"></param>
        static void SendMessage(ref TcpClient rClient, string str)
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
        static void ClientExited(TCP_Server client)
        {
            clientList.Remove(client);
            Debug.Log("... A client disconnected");
        }

        void Update()
        {
            if (x != "")
                transform.localPosition = new Vector3(float.Parse(x), float.Parse(z), float.Parse(y));
        }
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
