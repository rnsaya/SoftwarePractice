using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkController
{
    public class State
    {
        /// <summary>
        /// client socket
        /// </summary>
        public Socket socket = null;
        
        /// <summary>
        /// Receive buffer
        /// </summary>
        public byte[] buffer = new byte[1024 * 1024];

        /// <summary>
        /// Packet data (UTF)
        /// </summary>
        public StringBuilder stringBuffer = new StringBuilder();

        /// <summary>
        /// Count of sent packets
        /// </summary>
        public volatile int SentPackets;

        /// <summary>
        /// Count of receive packets
        /// </summary>
        public volatile int RecvPackets;
    }
}
