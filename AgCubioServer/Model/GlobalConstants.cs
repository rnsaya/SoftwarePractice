// This code is licensed under the GPL v2.0 by Dyllon Gagnier and Ross DiMassino.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Model
{
    /// <summary>
    /// This represents all the constants for the server.
    /// </summary>
    public static class GlobalConstants
    {
        /// <summary>
        /// This is the initial mass for a player cube.
        /// </summary>
        public const int StartingMass = 100;

        /// <summary>
        /// This is the default name for a cube.
        /// </summary>
        public const string DefaultName = "";

        /// <summary>
        /// This is the smallest value that a uid can take.
        /// </summary>
        public const int MinUID = 1;

        /// <summary>
        /// This is the size of the world rectangle.
        /// </summary>
        public static readonly Size WorldSize = new Size(1000, 1000);

        /// <summary>
        /// This is the list of all possible player colors.
        /// </summary>
        public static readonly int[] PlayerColors = new int[]
        {
            Color.Red.ToArgb(), Color.Aqua.ToArgb(), Color.Blue.ToArgb(),
            Color.DeepPink.ToArgb(), Color.DarkOrange.ToArgb(), Color.BlueViolet.ToArgb() 
        };

        /// <summary>
        /// This is the rate at which cubes loose mass.
        /// </summary>
        public static readonly double AttritionRate = 200.0;

        /// <summary>
        /// This is the minimum cube size after which no further splits are allowed.
        /// </summary>
        public const double MinCubeSize = 10;

        /// <summary>
        /// The default port to start AgCubio on.
        /// </summary>
        public const int DefaultPort = 11000;

        /// <summary>
        /// This is the period of when the world updates (i.e. wait between updates).
        /// </summary>
        public const double WorldUpdatePeriod = 1 / 60.0;

        /// <summary>
        /// This is the team id of all food (and viruses).
        /// </summary>
        public const int FoodTeamID = 0;

        /// <summary>
        /// This is the default food mass.
        /// </summary>
        public const double FoodMass = 1;

        /// <summary>
        /// The maximum food allowed in the world.
        /// </summary>
        public const int MaximumFood = 5000;

        /// <summary>
        /// This is the maximum speed a cube can go.
        /// </summary>
        public const double MaxSpeed = 30.0;

        /// <summary>
        /// This is the minimum speed a cube can go.
        /// </summary>
        public const double MinSpeed = 3.0;

        /// <summary>
        /// The amount of time to wait before rejoining split cubes.
        /// </summary>
        public const double SplitTime = 10.0;

        /// <summary>
        /// This is the speed that the Cube shoots off at.
        /// </summary>
        public const double SplitSpeed = 1.2;

        /// <summary>
        /// This is the minimum split speed at which the momentum is considered to be 0.
        /// </summary>
        public const double MinSplitSpeed = 1.0;

        /// <summary>
        /// The rate at which momentum slows down from splits.
        /// </summary>
        public const double Friction = 1.1;

        /// <summary>
        /// This is the amount of food to add per heartbeat.
        /// </summary>
        public const int FoodPerHeartbeat = 10;

        /// <summary>
        /// This is the mass of viruses.
        /// </summary>
        public const double VirusMass = 100;

        /// <summary>
        /// This is the number of viruses in the world. As soon as one is eaten, one pops up somewhere else.
        /// </summary>
        public const int VirusCount = 30;

        /// <summary>
        /// This is the color of a virus.
        /// </summary>
        public static readonly int VirusColor = Color.Green.ToArgb();

        /// <summary>
        /// The database connection string for statistics.
        /// </summary>
        public const string DatabaseConnection = "server=atr.eng.utah.edu;database=cs3500_gagnier;uid=cs3500_gagnier;password=PSWRD";
    }
}
