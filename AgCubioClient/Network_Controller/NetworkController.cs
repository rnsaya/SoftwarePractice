// Written by Ken Bonar and Rachel Saya for CS 3500, November 2015

#region

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

#endregion

namespace Network_Controller
{
    /// <summary>
    ///     The NetworkController class opens the socket between the client and the server
    ///     and provides helper functions for sending and receiving data.
    /// </summary>
    public static class NetworkController
    {
        //==================================================================================================================================================
        //                                                                        Networking Code
        //==================================================================================================================================================

        public const int Port = 11000;


        /// <summary>
        ///     This function attempts to connect to the server via a provided hostname. It
        ///     saves the callback function (in a state object) for use when data arrives.
        ///     It opens a socket and then uses the BeginConnect method. Note this method
        ///     take the "state" object and "regurgitates" it back to you when a connection is made,
        ///     thus allowing "communication" between this function and the ConnectedToServer function.
        /// </summary>
        /// <param name="call"> a function inside the view to be called when a connection is made</param>
        /// <param name="hostName">The name of the server to connect to.</param>
        public static Socket ConnectToServer(Action<PreservedStateObject> call, string hostName)
        {
            PreservedStateObject state = new PreservedStateObject();
            try
            {
                IPAddress address = null;

                // Get the IP address
                try
                {
                    address = Dns.GetHostEntry(hostName).AddressList[0];
                }
                catch (Exception)
                {
                    MessageBox.Show("Entered incorrect server value");
                }

                // Establish the remote endpoint for the socket.
                IPEndPoint remoteEp = new IPEndPoint(address, 11000);

                // Create a TCP/IP  socket.
                Socket sender = new Socket(address.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                state.GameSocket = sender;
                state.Call = call;

                // Attempt to establish connection
                sender.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
                sender.BeginConnect(remoteEp, ConnectedToServer, state);

                return sender;
            }

                // Shows an error message if unsuccessful
            catch (Exception e)
            {
                state.ErrorMessage = e.ToString();
                state.ErrorHappened = true;

                Console.WriteLine("Error: " + e);
                return null;
            }
        }

        /// <summary>
        ///     This function is "called" by the OS when the socket connects to the server.
        ///     Once a connection is established the "saved away" callback function is called.
        ///     Additionally, the network connection executes "BeginReceive" expecting more data to arrive
        ///     (and provides the ReceiveCallback function for this purpose)
        /// </summary>
        /// <param name="stateInAnArObject"> Contains the "state" object saved away in the ConnectToServer function</param>
        public static void ConnectedToServer(IAsyncResult stateInAnArObject)
        {
            PreservedStateObject state = (PreservedStateObject) stateInAnArObject.AsyncState;
            try
            {
                // Ends connection request
                state.GameSocket.EndConnect(stateInAnArObject);
                state.Call(state);

                try
                {
                    state.GameSocket.BeginReceive(state.Buffer, 0, 1024, SocketFlags.None, ReceiveCallback, state);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

                // Shows an error message if unsuccessful
            catch (Exception e)
            {
                state.ErrorMessage = e.ToString();
                state.ErrorHappened = true;
                state.Call(state);
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        ///     The ReceiveCallback method is called by the OS when new data arrives.
        ///     This method checks to see how much data has arrived. If 0, the connection
        ///     has been closed (presumably by the server). On greater than zero data,
        ///     this method should call the callback function provided above.
        ///     For our purposes, this function does not request more data.
        /// </summary>
        /// <param name="stateInAnArObject"></param>
        public static void ReceiveCallback(IAsyncResult stateInAnArObject)
        {
            try
            {
                PreservedStateObject preservedSocketState = (PreservedStateObject) stateInAnArObject.AsyncState;

                // Check to see if connection has been closed
                int count = preservedSocketState.GameSocket.EndReceive(stateInAnArObject);
                if (count <= 0)
                    return;
                preservedSocketState.StringBuilder.Append(Encoding.UTF8.GetString(preservedSocketState.Buffer, 0, count));
                try
                {
                    preservedSocketState.Call(preservedSocketState);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e);
            }
        }

        /// <summary>
        ///     The client View code calls this function whenever it wants more data.
        /// </summary>
        /// <param name="socketState"></param>
        public static void i_want_more_data(PreservedStateObject socketState)
        {
            try
            {
                socketState.GameSocket.BeginReceive(socketState.Buffer, 0, 1024, SocketFlags.None, ReceiveCallback,
                    socketState);
            }
            catch (Exception e)
            {
                Console.WriteLine("socket error");
            }
        }

        /// <summary>
        ///     This function (along with it's helper 'SendCallback') allows a
        ///     program to send data over a socket. This function converts
        ///     the data into bytes and then send them using socket.BeginSend.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        public static bool Send(Socket socket, string data)
        {
            // Converts data into bytes.
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, SendCallback, socket);
                return true;
            }
            catch (Exception)
            {
                // Close socket
                try
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch (Exception)
                {
                    // ignored
                }
                return false;
            }
        }


        /// <summary>
        ///     Function that helps to send data over a socket.
        /// </summary>
        /// <param name="ar"></param>
        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                ((Socket) ar.AsyncState).EndSend(ar);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }

    //==================================================================================================================================================
    //                                                                        State Object Class
    //==================================================================================================================================================

    /// <summary>
    ///     The PreservedStateObject class provides a contract for a preserved socket state
    /// </summary>
    public class PreservedStateObject
    {
        // name object to be sent
        public string name = "";

        // Client Socket
        public Socket GameSocket;

        // buffer for network
        public const int BufferSize = 1024;

        // Receive Buffer
        public byte[] Buffer = new byte[BufferSize];

        // Received Data String 
        public StringBuilder StringBuilder = new StringBuilder();

        // Error Message code
        public string ErrorMessage = "";
        public bool ErrorHappened;

        // unique id
        public long Uid;

        public Action<PreservedStateObject> Call;

    } // End of State Object class

} // End of Network Controller Namespace