// This code is licensed under the GPL v2.0 by Dyllon Gagnier and Rachel Saya.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace Model
{
    /// <summary>
    /// This represents any given player on the server.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// This is the data for PlayerCube.
        /// </summary>
        private Cube playerCube;


        /// <summary>
        /// The time that the player entered the session.
        /// </summary>
        public readonly DateTime birthTime;

        /// <summary>
        /// The amount of cubes the player has eaten.
        /// </summary>
        public int TotalCubesEaten { get; set; } = 0;

        /// <summary>
        /// The ID of the player that killed this player.
        /// </summary>
        public int KillerID { get; set; } = 0;

        /// <summary>
        /// The ID of the session the player is in.
        /// </summary>
        public readonly int SessionID;

 
        /// <summary>
        /// This is the timer for keeping track of when to rejoin.
        /// </summary>
        private Stopwatch splitTimer = new Stopwatch();

        /// <summary>
        /// The cube associated with this player. This should not be directly manipulated.
        /// </summary>
        public Cube PlayerCube
        {
            get
            {
                return this.playerCube;
            }

            private set
            {
                if (this.playerCube != null)
                    this.allCubes.Remove(this.playerCube);
                this.playerCube = value;
                this.allCubes.Add(this.playerCube);
            }
        }

        /// <summary>
        /// This set contains all cubes associated with this player.
        /// </summary>
        private HashSet<Cube> allCubes = new HashSet<Cube>();

        /// <summary>
        /// This indicates whether the name has been set or not yet.
        /// </summary>
        public bool ReceivedName { get; private set; } = false;

        /// <summary>
        /// This is the name of the player.
        /// </summary>
        public string Name
        {
            get
            {
                return this.PlayerCube.name;
            }

            set
            {
                this.PlayerCube.name = value;
                this.ReceivedName = true;
            }
        }

        /// <summary>
        /// This iterates over all cubes that are owned by this player.
        /// </summary>
        /// <returns>All cubes owned by this player.</returns>
        public IEnumerable<Cube> GetAllCubes()
        {
            foreach (Cube cube in allCubes)
                yield return cube;
        }

        /// <summary>
        /// This method splits all possible cubes under this player.
        /// </summary>
        private void Split()
        {
            List<Cube> splits = new List<Cube>();
            foreach (Cube cube in allCubes)
            {
                if (cube.Mass > GlobalConstants.MinCubeSize)
                {
                    splits.Add(cube.Split(true));
                }
            }

            splits.ForEach((cube) => this.allCubes.Add(cube));
            World.ServerSingleton.UpdateCubes(splits);
            this.splitTimer.Restart();
        }

        /// <summary>
        /// True if there is a request to split on the server.
        /// </summary>
        public bool SplitRequested { get; private set; } = false;

        /// <summary>
        /// The data for SplitLocation.
        /// </summary>
        private Point splitLocation;

        /// <summary>
        /// Gets or sets the desired location of the split.
        /// </summary>
        public Point SplitLocation
        {
            get
            {
                return this.splitLocation;
            }

            set
            {
                this.SplitRequested = true;
                this.splitLocation = value;
            }
        }

        /// <summary>
        /// This is a convenience method for PlayerCube.uid.
        /// </summary>
        public int UID
        {
            get { return PlayerCube.uid; }
        }

        /// <summary>
        /// This creates a player cube with the input uid and team id.
        /// </summary>
        /// <param name="uid">The uid of the player.</param>
        /// <param name="teamId">The team id of the player.</param>
        /// <param name="color">The argb color.</param>
        /// <param name="loc">The starting location.</param>
        public Player(int uid, int teamId, int color, Point loc)
        {
            this.PlayerCube = new Cube(uid, teamId);
            PlayerCube.argb_color = color;
            PlayerCube.Mass = GlobalConstants.StartingMass;
            PlayerCube.loc_x = loc.X;
            PlayerCube.loc_y = loc.Y;
            PlayerCube.food = false;
            PlayerCube.name = GlobalConstants.DefaultName;
            birthTime = DateTime.Now;
        }

        /// <summary>
        /// This is the lock to ensure that only one thread is creating a player at a time.
        /// </summary>
        private static readonly Object CreationLock = new Object();

        /// <summary>
        /// This generates a new player cube at a random location with a new uid with the team id
        /// equal to 0.
        /// </summary>
        /// <returns>A new player cube.</returns>
        public static Player GetNewPlayer()
        {
            lock (CreationLock)
            {
                ServerWorld server = World.ServerSingleton;
                int id = server.GetNewUID();
                return new Player(id, id, server.GeneratePlayerColor(), server.GenerateLocation());
            }
        }

        /// <summary>
        /// This method is invoked by the heartbeat and performs all necessary mutations like splits.
        /// </summary>
        public void ServerTick()
        {
            // Handles splitting
            if (this.SplitRequested)
            {
                this.Split();
                this.SplitRequested = false;
            }

            if (this.allCubes.Count > 1 && this.splitTimer.ElapsedMilliseconds / 1000.0 > GlobalConstants.SplitTime)
            {
                JoinCubes();
            }

            if (ResolveCollisions())
                World.ServerSingleton.UpdateCubes(allCubes);
        }

        /// <summary>
        /// This method resolves collisions between cubes for this player (so that cubes don't overlap).
        /// </summary>
        /// <returns>True if cubes have changed.</returns>
        private bool ResolveCollisions()
        {
            bool changed = false;
            foreach (Cube cube1 in this.allCubes)
            {
                Rectangle cube1Rect = cube1.GetRect();
                foreach (Cube cube2 in this.allCubes)
                {
                    if (cube1Rect.Contains((int)cube2.loc_x, (int)cube2.loc_y)
                        && cube1 != cube2)
                    {
                        cube2.loc_x += cube1.Width;
                        cube2.loc_y += cube1.Width;
                        changed = true;
                    }
                }
            }

            return changed;
        }

        /// <summary>
        /// Joins up all the cubes into one cube.
        /// </summary>
        private void JoinCubes()
        {
            foreach (Cube toEat in this.allCubes)
                if (toEat != this.PlayerCube)
                {
                    this.PlayerCube.EatCube(toEat);
                }
            World.ServerSingleton.UpdateCubes(this.allCubes);
            this.allCubes.Clear();
            this.allCubes.Add(this.PlayerCube);
        }

        /// <summary>
        /// This splits the cube according to virus rules (one big, two small).
        /// </summary>
        /// <param name="cubeToSplit">The cube to split.</param>
        public void VirusSplit(Cube cubeToSplit)
        {
            Cube firstSplit = cubeToSplit.Split(false);
            Cube secondSplit = cubeToSplit.Split(false);
            this.allCubes.Add(firstSplit);
            this.allCubes.Add(secondSplit);
            this.splitTimer.Restart();
            World.ServerSingleton.UpdateCubes(new Cube[] { cubeToSplit, firstSplit, secondSplit });
        }

        /// <summary>
        /// This sets all cube's desired location to the input value.
        /// </summary>
        public Point DesiredLocation
        {
            set
            {
                foreach (Cube cube in this.GetAllCubes())
                    cube.DesiredLocation = value;
            }
        }
    }
}
