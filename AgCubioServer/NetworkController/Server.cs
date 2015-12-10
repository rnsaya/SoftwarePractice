// This code is licensed under the GPL v2.0 by Dyllon Gagnier and Rachel Saya.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace NetworkController
{
    /// <summary>
    /// This class creates a generic server that listens on some port.
    /// </summary>
    public class Server
    {
        /// <summary>
        /// This contains all servers that are currently running.
        /// </summary>
        private static readonly Dictionary<int, Server> servers = new Dictionary<int, Server>();

        /// <summary>
        /// This method creates a new object to lock getting servers in the case that a new server must be created.
        /// </summary>
        private static Object getServerLock = new Object();

        /// <summary>
        /// This is a factory method which returns the server listening at the given port.
        /// </summary>
        /// <param name="port">The port to listen on.</param>
        /// <returns>Either a new server or an existing server at the specified port.</returns>
        public static Server GetServer(int port)
        {
            lock(getServerLock)
            {
                if (!servers.ContainsKey(port))
                    servers[port] = new Server(port);
                return servers[port];
            }
        }

        /// <summary>
        /// This method will disconnect all servers.
        /// </summary>
        public static void DisconnectAllServers()
        {
            List<Server> allServers = servers.Aggregate<KeyValuePair<int, Server>, List<Server>>(new List<Server>(), (curr, pair) =>
            {
                curr.Add(pair.Value);
                return curr;
            });
            foreach (Server server in allServers)
                server.Disconnect();
        }

        /// <summary>
        /// This dictionary contains all open connections to the server.
        /// </summary>
        private readonly ConcurrentDictionary<int, Client> clients = new ConcurrentDictionary<int, Client>();

        /// <summary>
        /// This event is triggered whenever a message is received on the port of this server. The int is the
        /// id of the client and the string is the message.
        /// </summary>
        public event Action<int, string> ReceivedMessageEvent;

        /// <summary>
        /// This event is triggered whenever a new connection is made on the port of this object. The int parameter is the
        /// id of the new client. Data will not be received from the client until after this event completes, but data may be sent
        /// to the client.
        /// </summary>
        public event Action<int> NewConnectionEvent;

        /// <summary>
        /// This event is triggered whenever a client disconnects and is given
        /// the id of the disconnected client. No messages should be sent to the client after
        /// being disconnected.
        /// </summary>
        public event Action<int> DisconnectedEvent = (connId) => { };

        /// <summary>
        /// This function is called in order to generate ids for new connections which
        /// can then be used to reference that particular connection.
        /// </summary>
        public Func<int> GenerateConnectionID;

        /// <summary>
        /// This is the port that this server listens on.
        /// </summary>
        public readonly int Port;

        /// <summary>
        /// This is the listener listens on Port once connected.
        /// </summary>
        private TcpListener listener;

        /// <summary>
        /// This semaphore is used in order to ensure that new requests are not made on the server until the in progress request is made.
        /// </summary>
        private readonly Semaphore primaryLock = new Semaphore(1, 1);

        /// <summary>
        /// This initializes a new server object at the given port but does not connect or listen
        /// at the port until Connect is called.
        /// </summary>
        /// <param name="port">The port to listen on.</param>
        protected Server(int port)
        {
            this.Port = port;

            // Ensures events initializes
            NewConnectionEvent = (arg) => {};
            ReceivedMessageEvent = (arg, arg2) => { };

        }

        /// <summary>
        /// This tries to connect the server to the port of this server.
        /// </summary>
        /// <param name="afterConnect"></param>
        public void Connect(Action<bool> afterConnect)
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, this.Port);
                listener.Start();
                listener.BeginAcceptSocket(this.AcceptNewClient, null);
                afterConnect(true);
            }
            catch
            {
                afterConnect(false);
                throw;
            }
        }

        /// <summary>
        /// This method sends the specified message only to the listed client.
        /// </summary>
        /// <param name="clientId">The client to send a message to.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="afterSend">This is invoked after the send is complete. The parameter is true if the data was sent succesfully and false if not.</param>
        public void SendMessage(int clientId, string message, Action<bool> afterSend)
        {
            Client client = this.clients[clientId];
            try
            {
                client.SendMessage(message, afterSend);
            }
            catch { afterSend(false); }
        }

        /// <summary>
        /// This method broadcasts the specified message to all clients.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <param name="afterSend">This is invoked after each send completes for each client with the first parameter
        /// being the id of the client and the second being true if and only if the message was successfully sent.</param>
        public void BroadcastMessage(string message, Action<int, bool> afterSend)
        {
            foreach(KeyValuePair<int, Client> client in this.clients)
            {
                try
                {
                    client.Value.SendMessage(message, (succ) => afterSend(client.Key, succ));
                }
                catch { afterSend(client.Key, false); }
            }
        }

        /// <summary>
        /// This method is a callback for when a new client connects.
        /// </summary>
        /// <param name="result"></param>
        private void AcceptNewClient(IAsyncResult result)
        {
            Socket sock = this.listener.EndAcceptSocket(result);

            int clientId = this.GenerateConnectionID();
            Client newClient = new Client(sock, (message) => this.ReceivedMessageEvent(clientId, message),
                () => this.DisconnectedEvent(clientId));
            this.clients[clientId] = newClient;

            newClient.BeginReceiving();

            this.NewConnectionEvent(clientId);

            this.listener.BeginAcceptSocket(this.AcceptNewClient, null);
        }

        /// <summary>
        /// This method disconnects all clients from the server and stops listening to the port of this server.
        /// </summary>
        public void Disconnect()
        {
            foreach(KeyValuePair<int, Client> client in this.clients)
            {
                try
                {
                    client.Value.Disconnect();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
            this.listener.Stop();
        }

        /// <summary>
        /// This method disconnects the client but does not stop the server.
        /// </summary>
        /// <param name="clientId">The id of the client to disconnect.</param>
        public void Disconnect(int clientId)
        {
            lock(Server.servers)
            {
                try
                {
                    Client client;
                    if (this.clients.TryRemove(clientId, out client))
                        client.Disconnect();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }

                try
                {
                    Server.servers.Remove(clientId);
                }
                catch { }
            }
        }
    }

    /// <summary>
    /// This class manages communications to a client.
    /// </summary>
    internal class Client
    {
        /// <summary>
        /// The socket that is used to communicate with this client.
        /// </summary>
        private readonly Socket sock;

        /// <summary>
        /// This is the buffer size to be used in this class for byte arrays.
        /// </summary>
        private const int BufferSize = 1024;

        /// <summary>
        /// This is the byte array that stores the received bytes as they come in.
        /// </summary>
        private readonly byte[] receivedBufer = new byte[BufferSize];

        /// <summary>
        /// This is the string builder that is used to store the bytes from the receivedBuffer while waiting for the end of message (\n).
        /// </summary>
        private String currentMessage = "";

        /// <summary>
        /// This lock is used to ensure that multiple sends don't corrupt the receivedBuffer.
        /// </summary>
        private Semaphore sendLock = new Semaphore(1, 1);

        /// <summary>
        /// This is the callback for handling any incoming messages.
        /// </summary>
        private Action<string> MessageHandler;

        /// <summary>
        /// This method is called after the client is disconnected.
        /// </summary>
        private Action AfterDisconnect;

        /// <summary>
        /// This property indicates whether the client is currently receiving data or not.
        /// </summary>
        public bool IsReceiving { get; private set; }

        /// <summary>
        /// This creates a new client object without allowing it to receive data.
        /// </summary>
        /// <param name="sock">The socket for use with this client.</param>
        /// <param name="messageHandler">The method to invoke when a message is received.</param>
        /// <param name="afterDisconnect">This method is invoked after the client disconnects. 
        /// This class handles the socket entirely so it is not necessary for outside code to
        /// manually disconnect the managed socket.</param>
        public Client(Socket sock, Action<string> messageHandler, Action afterDisconnect)
        {
            this.sock = sock;
            this.MessageHandler = messageHandler;
            this.IsReceiving = false;
            this.AfterDisconnect = afterDisconnect;
        }

        /// <summary>
        /// This method allows data to begin to be received from this client.
        /// </summary>
        public void BeginReceiving()
        {
            lock(this)
            {
                if (!this.IsReceiving)
                    this.sock.BeginReceive(this.receivedBufer, 0, this.receivedBufer.Length, SocketFlags.None, this.ReceivedCallback, null);
                this.IsReceiving = true;
            }
        }

        /// <summary>
        /// This method is a callback for when data is received on the server.
        /// </summary>
        /// <param name="result"></param>
        private void ReceivedCallback(IAsyncResult result)
        {
            try
            {
                int receivedBytes = this.sock.EndReceive(result);

                if (receivedBytes == 0)
                {
                    throw new Exception("Client disconnected.");
                }

                this.currentMessage += Encoding.UTF8.GetString(this.receivedBufer, 0, receivedBytes);
                int nPos = this.currentMessage.LastIndexOf('\n');
                if (nPos >= 0)
                {
                    string toSend = this.currentMessage.Substring(0, nPos + 1);
                    if (nPos + 1 >= this.currentMessage.Length)
                        this.currentMessage = "";
                    else
                        this.currentMessage = this.currentMessage.Substring(nPos + 1, this.currentMessage.Length);
                    this.MessageHandler(toSend);
                }
            }
            catch
            {
                Console.WriteLine("Client disconnected.");
                this.Disconnect();
                return;
            }

            this.sock.BeginReceive(this.receivedBufer, 0, this.receivedBufer.Length, SocketFlags.None, this.ReceivedCallback, null);
        }

        /// <summary>
        /// This method disconnects this client.
        /// </summary>
        public void Disconnect()
        {
            try
            {
                this.sock.Shutdown(SocketShutdown.Both);
                this.sock.Close();
                this.AfterDisconnect();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// This method sends a messgae to this client.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <param name="afterSend">Invoked after the message is sent. The parameter is true if and only if the message was successfully sent.</param>
        public void SendMessage(string message, Action<bool> afterSend)
        {
            message = message + '\n';
            byte[] packet = Encoding.UTF8.GetBytes(message);
            sendLock.WaitOne();
            try
            {
                this.sock.BeginSend(packet, 0, packet.Length, SocketFlags.None, this.SendCallback, afterSend);
            }
            catch
            {
                sendLock.Release();
                this.Disconnect();
                Console.WriteLine("Client disconnected.");
                afterSend(false);
            }
        }

        /// <summary>
        /// This is the callback for after a send operation completes on a socket.
        /// </summary>
        /// <param name="result">The method to invoke after sending data.</param>
        private void SendCallback(IAsyncResult result)
        {
            Action<bool> afterSend = (Action<bool>)result.AsyncState;
            bool success = true;
            try
            {
                int bytesSent = sock.EndSend(result);
            }
            catch
            {
                success = false;
                this.Disconnect();
            }
            finally
            {
                sendLock.Release();            
                afterSend(success);
            }
        }
    }
}
