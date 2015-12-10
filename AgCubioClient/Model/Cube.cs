#region

using System;
using Newtonsoft.Json;

#endregion

namespace Model
{
    //==================================================================================================================================================
    //                                                                          Cube Class
    //==================================================================================================================================================


    /// <summary>
    ///     The Cube class represents a cube in the game logic. A cube as a unique ID, a position in space (x,y), a color,
    ///     a name if it is a player cube, a mass, a status as food or player.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Cube
    {
        //==================================================================================================================================================
        //                                                                   Global Variables
        //==================================================================================================================================================

        /// <summary>
        ///     The cube's unique identification number.
        /// </summary>
        [JsonProperty] public long uid;

        /// <summary>
        ///     True when the cube is food and false if it is a player.
        /// </summary>
        [JsonProperty] public bool food;

        /// <summary>
        ///     The cube's color.
        /// </summary>
        [JsonProperty] public int argb_color;

        /// <summary>
        ///     The cube's name if it is a player. Will be null if cube is food.
        /// </summary>
        [JsonProperty] public string Name;

        /// <summary>
        ///     The cube's mass.
        /// </summary>
        [JsonProperty] public double Mass;

        /// <summary>
        ///     The x coordinate of the cube.
        /// </summary>
        [JsonProperty] public float loc_x;

        /// <summary>
        ///     The y coordinate of the cube.
        /// </summary>
        [JsonProperty] public float loc_y;

        /// <summary>
        ///     The square root of the cube's mass.
        /// </summary>
        public double Size;

        /// <summary>
        ///     This ID is used to identify multiple blocks from the same user
        /// </summary>
        public int _teamid;

        //==================================================================================================================================================
        //                                                                          Constructor
        //==================================================================================================================================================

        /// <summary>
        ///     Creates a cube object.
        /// </summary>
        public Cube(float xVar, float yVar, int color, int uid, bool isFood, string name, double mass, int teamid)
        {
            _teamid = teamid;
            Mass = mass;
            loc_x = xVar;
            loc_y = yVar;
            argb_color = color;
            Name = name;
            Size = Math.Pow(mass, .65);
            this.uid = uid;
            food = isFood;
        }

    } // End of Cube class

} // End of Model namespace
