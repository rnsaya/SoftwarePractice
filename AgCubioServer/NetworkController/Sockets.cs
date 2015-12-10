using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;

using Model;

namespace NetworkController
{
    /// <summary>
    /// networking callback delegate
    /// </summary>
    /// <param name="o"></param>
    public delegate void NetCB(IAsyncResult s);

    /// <summary>
    /// Generic Socket library for handling sockets and network event callbacks
    /// </summary>
    public static class Sockets
    {
        /// <summary>
        /// stores connection state
        /// </summary>
        private static State state;

        /// <summary>
        /// This is the main lock to ensure that calls to Sockets 
        /// </summary>
        private static readonly Semaphore socketSemaphore = new Semaphore(1, 1);

        /// <summary>
        /// State.SentPackets property accessor
        /// </summary>
        public static int SentPackets
        {
            get { return state.SentPackets; }
        }

        /// <summary>
        /// State.RecvPackets property accessor
        /// </summary>
        public static int RecvPackets
        {
            get { return state.RecvPackets; }
        }

        /// <summary>
        /// Initiates a connection with a server.
        /// </summary>
        /// <param name="hostname">server hostname</param>
        /// <param name="port">server port</param>
        /// <param name="afterConnected">The </param>
        /// <returns></returns>
        public static Socket Connect(string hostname, int port, Action<bool> afterConnected)
        {
            state = new State();
            socketSemaphore.WaitOne();
            try
            {
                state.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                state.socket.BeginConnect(hostname, port, new AsyncCallback(Sockets.GetConnectedCallback(afterConnected)), state);
            }
            catch { }

            return state.socket;
        }

        /// <summary>
        /// This method composes a new function for when connected.
        /// </summary>
        /// <param name="userCallback">The callback to invoke indicating whether connecting succeeded or failed.</param>
        /// <returns>A new function for use with AsyncCallback</returns>
        private static NetCB GetConnectedCallback(Action<bool> userCallback)
        {
            return (callback) =>
            {
                State s = (State)callback.AsyncState;
                bool succ;
                try
                {
                    s.socket.EndConnect(callback);
                    succ = true;
                }
                catch
                {
                    succ = false;
                }
                finally { socketSemaphore.Release(); }

                userCallback(succ);
            };
        }

        /// <summary>
        /// This creates a function that will handle sending a packet
        /// </summary>
        /// <param name="userAction">action callback with EndSend status</param>
        /// <returns></returns>
        private static NetCB GetSentCallback(Action<bool> userAction)
        {
            return (result) =>
            {
                State s = (State)result.AsyncState;
                s.SentPackets++;

                int success = 0;
                try
                {
                    success = s.socket.EndSend(result);
                }
                catch { }
                finally { socketSemaphore.Release(); }

                userAction(success != 0);
            };
        }

        /// <summary>
        /// This creates a function that will handle receiving a packet
        /// </summary>
        /// <param name="userCallback">action callback with toSend data</param>
        /// <returns></returns>
        private static NetCB GetReceivedCallback(Action<string> userCallback)
        {
            return (callback) =>
            {
                State s = (State)callback.AsyncState;
                s.RecvPackets++;

                string toSend = null;
                try
                {
                    int packet = s.socket.EndReceive(callback);
                    s.stringBuffer.Append(Encoding.UTF8.GetString(s.buffer, 0, packet));
                    string fullStr = s.stringBuffer.ToString();
                    int cutoff = fullStr.LastIndexOf('\n');
                    toSend = fullStr.Substring(0, cutoff + 1);
                    s.stringBuffer.Remove(0, cutoff + 1);
                }
                catch { }
                finally { socketSemaphore.Release(); }

                userCallback(toSend);
            };
        }

        /// <summary>
        /// Called when more data is needed.
        /// </summary>
        /// <param name="callback">The function to be executed with the next message from the network.</param>
        public static void Moar(Action<string> callback)
        {
            try
            {
                socketSemaphore.WaitOne();
                state.socket.BeginReceive(state.buffer, 0, state.buffer.Length, SocketFlags.None, new AsyncCallback(GetReceivedCallback(callback)), state);
            }
            catch
            {
                callback(null);
                socketSemaphore.Release();
            }
        }

        /// <summary>
        /// Sends data over a socket.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="callback">This will be executed after data is sent. The input bool will be true if sent sucessfully.</param>
        public static void Send(String data, Action<bool> callback)
        {
            byte[] packet = Encoding.UTF8.GetBytes(data + '\n');
            socketSemaphore.WaitOne();
            try
            {
                // Begin sending the data to the remote device.
                state.socket.BeginSend(packet, 0, packet.Length, SocketFlags.None, new AsyncCallback(Sockets.GetSentCallback(callback)), state);
            }
            catch
            {
                callback(false);
                socketSemaphore.Release();
            }
        }

        /// <summary>
        /// Sanely cleans up the socket
        /// </summary>
        public static void Disconnect()
        {
            try
            {
                state.socket.Shutdown(SocketShutdown.Both);
                state.socket.Close();
            }
            catch { }
            finally { }
        }
    }
}
