// This code is licensed under the GPL v2.0 by Dyllon Gagnier and Ross DiMassino.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using System.Drawing;
using System.Threading;

namespace Model
{
    /// <summary>
    /// This callback is raised whenever the world object's state changes.
    /// </summary>
    /// <param name="origin">The world from which the event was raised.</param>
    /// <param name="changed">A list of cells that have changed.</param>
    /// <param name="deleted">A list of cells that have been deleted.</param>
    public delegate void WorldChangedEventHandler(World origin, IEnumerable<Cube> changed, IEnumerable<Cube> deleted);

    /// <summary>
    /// This class represents the entire game world. Most of the time, Cubes should be delegated
    /// to here for modifications.
    /// </summary>
    public class World : IEnumerable<Cube>
    {
        /// <summary>
        /// This is the main World singleton.
        /// </summary>
        public static readonly World WorldSingleton = new World();

        /// <summary>
        /// This is the singleton for use with the server.
        /// </summary>
        public static readonly ServerWorld ServerSingleton = new ServerWorld();

        /// <summary>
        /// This is called whenver the world changes.
        /// </summary>
        public event WorldChangedEventHandler WorldChangedEvent;

        /// <summary>
        /// This is the main storage for all cube objects currently in the world.
        /// </summary>
        private readonly ConcurrentDictionary<int, Cube> allCubes = new ConcurrentDictionary<int, Cube>();

        /// <summary>
        /// This method updates all input cubes deleting them as necessary.
        /// </summary>
        /// <param name="cubes">The cubes to add.</param>
        public void UpdateCubes(IEnumerable<Cube> cubes)
        {
            List<Cube> deadCubes = new List<Cube>();
            List<Cube> changedCubes = new List<Cube>();
            foreach(Cube cube in cubes)
            {
                if (cube.IsDead)
                {
                    // The killedCube is just here so that cube.uid can be removed.
                    Cube killedCube;
                    this.allCubes.TryRemove(cube.uid, out killedCube);
                    deadCubes.Add(cube);
                }
                else
                {
                    if (this.allCubes.ContainsKey(cube.uid))
                    {
                        Cube old = this.allCubes[cube.uid];
                        old.loc_x = cube.loc_x;
                        old.loc_y = cube.loc_y;
                        old.Mass = cube.Mass;
                        old.name = cube.name;
                    }
                    else
                        this.allCubes[cube.uid] = cube;

                    this.allCubes.AddOrUpdate(cube.uid, cube, (uid, old) => old.UpdateCube(cube));

                    changedCubes.Add(cube);
                }
            }

            this.WorldChangedEvent(this, changedCubes, deadCubes);
        }


        /// <summary>
        /// This method will process the message and add all Json cubes in the message.
        /// </summary>
        /// <param name="message">A series of Json cubes separated by a \n.</param>
        public void ProcessCubeJson(string message)
        {
            this.UpdateCubes(World.GetCubesFromJson(message));
        }

        /// <summary>
        /// This method will process the message and return all Json cubes in the message.
        /// </summary>
        /// <param name="message">A series of Json cubes separated by a \n.</param>
        /// <returns>All cubes in the message.</returns>
        public static List<Cube> GetCubesFromJson(string message)
        {
            try
            {
                IEnumerable<string> jsonOfCubes = message.Split('\n').Select<string, string>((str) => str.Replace("\n", ""));
                List<Cube> toAdd = new List<Cube>();
                foreach (string jsonCube in jsonOfCubes)
                {
                    try
                    {
                        if (!jsonCube.Equals(""))
                        {
                            Cube cube = JsonConvert.DeserializeObject<Cube>(jsonCube);
                            toAdd.Add(cube);
                        }
                    }
                    catch
                    {
                    }
                }
                return toAdd;
            }
            catch { }

            return new List<Cube>();

            
        }

        /// <summary>
        /// This method iterates over all cubes in world.
        /// </summary>
        /// <returns>All cubes in world.</returns>
        IEnumerator<Cube> IEnumerable<Cube>.GetEnumerator()
        {
            return this.allCubes.Values.GetEnumerator();
        }

        /// <summary>
        /// This method iterates over all cubes in world.
        /// </summary>
        /// <returns>All cubes in world.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.allCubes.Values.GetEnumerator();
        }

        /// <summary>
        /// This initialilzes the World object.
        /// </summary>
        protected World()
        {
            // Make sure to add an event to prevent exceptions.
            this.WorldChangedEvent += (a, b, c) => { };
        }

        /// <summary>
        /// This calculates the maximum size in the x direction.
        /// </summary>
        public virtual double MaxSizeX
        {
            get
            {
                return this.allCubes.Values.Aggregate<Cube, double>(0, (current, cube) => Math.Max(current, cube.loc_x));
            }

            // Don't do anything to allow overriding by extending class.
            protected set { }
        }

        /// <summary>
        /// This calculates the maximum size in the y direction.
        /// </summary>
        public virtual double MaxSizeY
        {
            get
            {
                return this.allCubes.Values.Aggregate<Cube, double>(0, (current, cube) => Math.Max(current, cube.loc_y));
            }
            
            // Don't do anything to allow overriding by extending class.
            protected set { }
        }

        /// <summary>
        /// This method finds the closest, non-food cube to the input cube.
        /// </summary>
        /// <param name="other">The other cube.</param>
        /// <returns></returns>
        public Cube GetClosestCube(Cube other)
        {
            Cube closest = null;
            double bestDist = Double.MaxValue;
            foreach(Cube cube in this)
            {
                double dist = Math.Abs(Math.Pow(cube.loc_x - other.loc_x, 2) + Math.Pow(cube.loc_y - other.loc_y, 2));
                if (dist < bestDist && !cube.food && !cube.Equals(other))
                {
                    bestDist = dist;
                    closest = cube;
                }
            }

            return closest;
        }
        /// <summary>
        /// This method clears the current world quitely.
        /// </summary>
        public void ClearWorld()
        {
            this.allCubes.Clear();
        }
    }

    /// <summary>
    /// This represents a server's view of the world.
    /// </summary>
    public class ServerWorld : World
    {
        /// <summary>
        /// This is the random number generator used for this object.
        /// </summary>
        private readonly Random Gen = new Random();

        public override double MaxSizeX
        {
            get
            {
                return GlobalConstants.WorldSize.Width;
            }
        }

        public override double MaxSizeY
        {
            get
            {
                return GlobalConstants.WorldSize.Height;
            }
        }

        /// <summary>
        /// This initializes the world to have the starting food.
        /// </summary>
        public void InitializeWorld()
        {
            Cube[] toAdd = new Cube[GlobalConstants.MaximumFood];
            for (int i = 0; i < toAdd.Length; i++)
                toAdd[i] = this.CreateFood();
            this.UpdateCubes(toAdd);
        }

        /// <summary>
        /// This creates a new random food (but does not add it to the world yet).
        /// </summary>
        /// <returns>A new random cube.</returns>
        public Cube CreateFood()
        {
            Cube result = new Cube(this.GetNewUID(), GlobalConstants.FoodTeamID);
            Point loc = this.GenerateLocation();
            result.loc_x = loc.X;
            result.loc_y = loc.Y;
            result.food = true;
            result.Mass = GlobalConstants.FoodMass;
            result.argb_color = this.GeneratePlayerColor();
            result.name = "";
            return result;
        }

        /// <summary>
        /// Returns a new virus (not added to world yet).
        /// </summary>
        /// <returns>A new virus.</returns>
        public Cube CreateVirus()
        {
            Cube result = this.CreateFood();
            result.Mass = GlobalConstants.VirusMass;
            result.argb_color = GlobalConstants.VirusColor;
            result.IsVirus = true;
            return result;
        }

        /// <summary>
        /// This method generates a random point.
        /// </summary>
        /// <returns>A random point on the map.</returns>
        public Point GenerateLocation()
        {
            return new Point(Gen.Next((int)this.MaxSizeX), Gen.Next((int)this.MaxSizeY));
        }

        /// <summary>
        /// This generates a random color from the list of possible colors.
        /// </summary>
        /// <returns>An int of a random player color.</returns>
        public int GeneratePlayerColor()
        {
            int max = GlobalConstants.PlayerColors.Length;
            return GlobalConstants.PlayerColors[Gen.Next(max)];
        }

        /// <summary>
        /// This is the current for creating new players.
        /// </summary>
        private volatile int CurrentUID = GlobalConstants.MinUID;

        /// <summary>
        /// This returns a new UID that is globally unique..
        /// </summary>
        /// <returns>A new id.</returns>
        public int GetNewUID()
        {
            return Interlocked.Increment(ref CurrentUID);
        }

        /// <summary>
        /// This method modifies the world by moving cubes, eating cubes, populating with food,
        /// and all other functions that should be performed on the world ever server tick.
        /// </summary>
        /// <param name="playerFunc">This method accepts in a uid and returns the player with that id.</param>
        public void ServerHeartbeat(Func<int, Player> playerFunc)
        {
            HashSet<Cube> modifiedCubes = new HashSet<Cube>();
            foreach (Cube cube in this)
                if (cube.ServerTick(GetEatingAction(playerFunc, cube)))
                    modifiedCubes.Add(cube);

            // Add a constant amount of food prop to the number of players per hb if less than max food.
            int foodPerHb = GlobalConstants.FoodPerHeartbeat * this.NumerOfPlayers * 2;
            if (this.NumberOfFoods < GlobalConstants.MaximumFood)
                for (int i = 0; i < foodPerHb; i++)
                    modifiedCubes.Add(this.CreateFood());
            int virusesToAdd = GlobalConstants.VirusCount - this.Viruses;
            for (int i = 0; i < virusesToAdd; i++)
                modifiedCubes.Add(this.CreateVirus());
            if (modifiedCubes.Count > 0)
                this.UpdateCubes(modifiedCubes);
        }

        /// <summary>
        /// The function that is executed when eating cubes. 
        /// </summary>
        /// <param name="playerFunc"> Gets the player for a given teamUID.</param>
        /// <param name="predator">The cube that ate the prey cube.</param>
        /// <returns>The function</returns>
        private Action<Cube> GetEatingAction(Func<int, Player> playerFunc, Cube predator)
        {
            return (prey) =>
            {
                Player predatorPlayer = playerFunc(predator.team_id);
                if (prey.IsVirus)
                    predatorPlayer.VirusSplit(prey);

                // Keeps track of the player that killed this player
                else if (!prey.food)
                {
                    Player preyPlayer = playerFunc(prey.team_id);
                    preyPlayer.KillerID = predatorPlayer.UID;
                }

                predatorPlayer.TotalCubesEaten++;
            };
        }

        /// <summary>
        /// Returns the amount of food in the world.
        /// </summary>
        public int NumberOfFoods
        {
            get
            {
                return this.Count<Cube>((cube) => cube.food);
            }
        }

        /// <summary>
        /// This returns the number of non-food cubes in the game.
        /// </summary>
        public int NumerOfPlayers
        {
            get
            {
                return this.Count<Cube>((cube) => !cube.food);
            }
        }

        /// <summary>
        /// This returns the number of viruses.
        /// </summary>
        public int Viruses
        {
            get
            {
                return this.Count<Cube>((cube) => cube.IsVirus);
            }
        }
    }
}
