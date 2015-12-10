// This code is licensed under the GPL v2.0 by Dyllon Gagnier and Rachel Saya
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using NetworkController;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Drawing;

namespace AgCubio.ServerProgram
{
    /// <summary>
    /// This is the main server class.
    /// </summary>
    public class AgCubioServer : IDisposable
    {
        /// <summary>
        /// This is the main server object.
        /// </summary>
        private Server server;

        /// <summary>
        /// This is a dictionary of current players.
        /// </summary>
        private Dictionary<int, Player> playerLookup = new Dictionary<int, Player>();

        /// <summary>
        /// This represents the clients who need to updated with any world changes.
        /// </summary>
        private HashSet<int> subscribedClients = new HashSet<int>();

        /// <summary>
        /// This is the object periodically updating the world.
        /// </summary>
        private PeriodicTaskExecutor worldUpdater;

        /// <summary>
        /// Returns the player with the given uid.
        /// </summary>
        /// <param name="uid">The uid of the player.</param>
        /// <returns>The player with the given uid.</returns>
        public Player GetPlayer(int uid)
        {
            return playerLookup[uid];
        }

        /// <summary>
        /// Sets the player with the given uid to the input player using the input
        /// player's uid.
        /// </summary>
        /// <param name="player">The player to add to the server.</param>
        public void SetPlayer(Player player)
        {
            this.playerLookup[player.UID] = player;
            World.ServerSingleton.UpdateCubes(new Cube[] { player.PlayerCube });
        }

        /// <summary>
        /// Runs the server.
        /// </summary>
        /// <param name="args">Unused.</param>
        public static void Main(string[] args)
        {
            AgCubioServer server = new AgCubioServer();
            AppDomain.CurrentDomain.ProcessExit += (arg1, arg2) => server.Dispose();
        }

        /// <summary>
        /// Starts listening on the default port and initializes the world state.
        /// </summary>
        public AgCubioServer()
        {
            World.ServerSingleton.InitializeWorld();

            this.NetworkStart();
            this.worldUpdater = new PeriodicTaskExecutor(this.UpdateWorld, GlobalConstants.WorldUpdatePeriod);
        }

        /// <summary>
        /// Starts listening for traffic on port.
        /// </summary>
        private void NetworkStart()
        {
            server = Server.GetServer(GlobalConstants.DefaultPort);
            server.GenerateConnectionID = this.NewPlayer;
            server.NewConnectionEvent += (connId) => { };
            server.DisconnectedEvent += this.ClientDisconnect;
            server.ReceivedMessageEvent += this.ProcessMessages;
            server.Connect((succ) => Console.WriteLine(succ ? "Connected to server on port " + server.Port : "Could not connect to port " + server.Port));
            World.ServerSingleton.WorldChangedEvent += SendUpdatedCubes;
        }

        /// <summary>
        /// This method sends all changed and deleted cubes to all listening parties.
        /// </summary>
        /// <param name="origin">The origin world</param>
        /// <param name="changed">The changed cubes.</param>
        /// <param name="deleted">The deleted cubes.</param>
        private void SendUpdatedCubes(World origin, IEnumerable<Cube> changed, IEnumerable<Cube> deleted)
        {
            IEnumerable<Cube> allCubes = changed.Concat<Cube>(deleted);
            lock(this.subscribedClients)
            {
                foreach (int client in this.subscribedClients)
                {
                    foreach (Cube cube in allCubes)
                        server.SendMessage(client, cube.Json, (succ) => Debug.WriteIf(succ, "Could not cube to client:" + client));
                }
            }
        }

        /// <summary>
        /// This creates a new player and returns the UID of that player.
        /// </summary>
        /// <returns>A new player's UID.</returns>
        private int NewPlayer()
        {
            Player newPlayer = Player.GetNewPlayer();
            this.SetPlayer(newPlayer);
            return newPlayer.UID;
        }

        /// <summary>
        /// This method processes messages from clients.
        /// </summary>
        /// <param name="connId">The client id.</param>
        /// <param name="message">The message received.</param>
        private void ProcessMessages(int connId, string message)
        {
            Player currentPlayer = this.GetPlayer(connId);

            if (!currentPlayer.ReceivedName)
            {
                // Get length - 1 to avoid newline.
                currentPlayer.Name = message.Substring(0, message.Length - 1);

                // Send the player his cube and then the whole world.
                string playerJson = currentPlayer.PlayerCube.Json;
                this.server.SendMessage(connId, currentPlayer.PlayerCube.Json, this.SendWorld(connId));
            }
            else
            {

                //'(move, dest_x, dest_y)\n';
                Player player = this.GetPlayer(connId);
                if (message.Contains("move"))
                {
                    Match moveMatch = Regex.Match(message, @"\(move, (.*), (.*)\)");
                    Double xLoc = Double.Parse(moveMatch.Groups[1].ToString());
                    Double yLoc = Double.Parse(moveMatch.Groups[2].ToString());
                    player.DesiredLocation = new Point((int)xLoc, (int)yLoc);
                }
                else if (message.Contains("split"))
                {
                    Match splitMatch = Regex.Match(message, @"\(split, (.*), (.*)\)");
                    Double xLoc = Double.Parse(splitMatch.Groups[1].ToString());
                    Double yLoc = Double.Parse(splitMatch.Groups[2].ToString());
                    player.SplitLocation = new Point((int)xLoc, (int)yLoc);
                }

            }
        }

        /// <summary>
        /// This sends the entire world to the id.
        /// </summary>
        /// <param name="connId">The id to send the world to.</param>
        /// <returns>A method for sending the world.</returns>
        private Action<bool> SendWorld(int connId)
        {
            return (success) =>
            {
                if (success)
                {
                    foreach (Cube cube in World.ServerSingleton)
                    {
                        this.server.SendMessage(connId, cube.Json, (succ) =>
                        {
                            if (!succ)
                                Debug.WriteLine("Could not send cube to " + connId);
                        });

                    }

                    lock(this.subscribedClients)
                    {
                        this.subscribedClients.Add(connId);
                    }
                }
                else
                {
                    Debug.WriteLine("Could not send anything to " + connId);
                }
            };
        }

        /// <summary>
        /// Disconnects the server.
        /// </summary>
        public void Dispose()
        {
            try
            {
                this.server.Disconnect();
            }
            catch { }
            try
            {
                this.worldUpdater.Stopped = true;
            }
            catch { }
        }

        /// <summary>
        /// Updates the world.
        /// </summary>
        public void UpdateWorld()
        {
            // Move players and update world
            World.ServerSingleton.ServerHeartbeat(this.GetPlayer);

            // Split cubes
            lock(this.subscribedClients)
            {
                foreach (int clientId in this.subscribedClients)
                    this.GetPlayer(clientId).ServerTick();
            }
            // Determine if cubes eaten.
            // Add food if necessary.
            // Make players lose mass over time.
            // Let players know about the updating world.
        }

        ///// <summary>
        /// This method removes the client
        /// </summary>
        /// <param name="connId">The id of the client that disconnected.</param>
        private void ClientDisconnect(int connId)
        {
            lock(this.subscribedClients)
            {
                this.subscribedClients.Remove(connId);
            }
        }
    }
}
