// This code is licensed under the GPL v2.0 by Dyllon Gagnier and Rachel Saya.
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Model;
using NetworkController;
using View;
using System.Diagnostics;

namespace AgCubioConsole
{
    class Program
    {
        public volatile bool isCalled = false;

        static void Main(string[] args)
        {
            //Sockets.Connect("localhost", 11000, connected);
            //Server server = Server.GetServer(11000);
            //server.GenerateConnectionID = () => 0;
            //server.NewConnectionEvent += (connId) => Console.WriteLine("New connection");
            //server.ReceivedMessageEvent += (connId, mess) => Console.WriteLine("Received message:" + mess);
            //server.Connect((succ) => Console.WriteLine(succ ? "Connected" : "Not connected"));
            //server.NewConnectionEvent += cubeSend;

            //Console.ReadLine();
            Console.ReadLine();
            Console.WriteLine("Starting the client.");
            Sockets.Connect("localhost", 11000, (succ) => Console.WriteLine(succ?"Connected.":"Not connected"));
            Sockets.Send("name", succ => Console.WriteLine(succ ? "Sent name." : "Not sent name."));
            while(true)
            {
                Sockets.Moar((str) => Console.WriteLine(str));
                Thread.Sleep(10000);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cubeID"></param>
        public static void cubeSend(int cubeID)
        {
            string CubeTwoString = "{ \"loc_x\":500.0, \"loc_y\":600.0, \"argb_color\":-42, \"uid\":222, \"food\":true, \"Name\":\"Dyllon\", \"Mass\":50.0, \"team_id\":2 }\n";
            Server server = Server.GetServer(11000);
            server.SendMessage(cubeID, CubeTwoString, (succ) => Console.WriteLine(succ ? "Sent" : "Not sent"));
            
        }

        public static void connected(bool succ)
        {
            if (succ)
            {
                Debug.WriteLine("Connected");
                Sockets.Send("foo", (succ2) => SafePrint("sent name" + succ));
                int time = 10000;
                Sockets.Moar((str) => SafePrint("First moar:" + str));
                Thread.Sleep(time);
                Sockets.Moar((str) => SafePrint("Second moar:" + str));
                Thread.Sleep(time);
                Sockets.Moar((str) => SafePrint("third moar:" + str));
            }
            else
            {
                Debug.WriteLine("Failed to connect.");
            }
        }

        private static readonly Object mutex = new Object();

        public static void SafePrint(string str)
        {
            lock(mutex)
            {
                Debug.WriteLine(str);
            }
        }
    }
}
