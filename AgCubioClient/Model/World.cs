// Written by Ken Bonar and Rachel Saya for CS 3500, November 2015

using System.Collections.Generic;

namespace Model
{
    //==================================================================================================================================================
    //                                                                          World Class
    //==================================================================================================================================================
    /// <summary>
    /// The world class represents the "state" of the simulation. It is responsible for tracking the 
    /// world, height, and all the cubes in the game.
    /// </summary>
    public class World
    {
        //==================================================================================================================================================
        //                                                                   Global Variables
        //==================================================================================================================================================

        /// <summary>
        /// Keeps track of all of the cubes in the world
        /// </summary>

        public Dictionary<long, Cube> dict;

        /// <summary>
        /// Provides a count of total food in game
        /// </summary>
        public int foodCount;

        /// <summary>
        /// Provides a count of food that has been eaten
        /// </summary>
        public int foodEaten;


        /// <summary>
        /// Constructor
        /// </summary>
        public World()
        {
            dict = new Dictionary<long, Cube>() ;
        }
    
        /// <summary>
        /// Adds a cube to the world dictionary.
        /// </summary>
        /// <param name="cube"></param>
        public void Add(Cube cube)
        {
            //if it is food
            if (cube.food)
                //increment food
            { ++foodCount;}
            if (dict.ContainsKey(cube.uid))
            { UpdateCube(cube.uid, cube.loc_x, cube.loc_y, cube.Mass, cube.Size);}
            
            else
            {
                dict.Add(cube.uid, cube);
            }
        }

        /// <summary>
        /// Removes a cube from the world dictionary.
        /// </summary>
        /// <param name="cube"></param>
        public void Remove(Cube cube)
        {
            if (cube.food)
            {
                --foodCount;
                ++foodEaten;
            }
            dict.Remove(cube.uid);
        }

        /// <summary>
        /// Changes the values of the cube object to reflect new values
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="mass"></param>
        /// <param name="size"></param>
        public void UpdateCube(long uid, float x, float y, double mass, double size)
        {          
                dict[uid].loc_x = x;
                dict[uid].loc_y = y;
                dict[uid].Mass = mass;
                dict[uid].Size = size;
        }

    }
}
