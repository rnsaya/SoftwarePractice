// This code is licensed under the GPL v2.0 by Dyllon Gagnier and Ross DiMassino.
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Linq;

namespace Model
{
    /// <summary>
    /// This tests the world class. ALL TESTS AFFECTING THE WORLD MUST BE IN HERE.
    /// </summary>
    [TestClass]
    public class WorldTest
    {
        /// <summary>
        /// These are for the first cube. CubeOneString should be equal to CubeOne once parsed.
        /// </summary>
        public const string CubeOneString = "{ \"loc_x\":926.0, \"loc_y\":682.0, \"argb_color\":-65536, \"uid\":5571, \"food\":false, \"Name\":\"3500 is love\", \"Mass\":1000.0, \"team_id\":1 }\n";
        private readonly Cube CubeOne = new Cube(5571, 1);

        public const string CubeTwoString = "{ \"loc_x\":500.0, \"loc_y\":600.0, \"argb_color\":-42, \"uid\":222, \"food\":true, \"Name\":\"Dyllon\", \"Mass\":50.0, \"team_id\":2 }\n";
        public readonly Cube CubeTwo = new Cube(222, 2);

        /// <summary>
        /// This method checks very precisely for value equality between cubes by looking at
        /// all public properties.
        /// </summary>
        /// <param name="expected">The expected cube.</param>
        /// <param name="actual">The actual cube.</param>
        public static void AssertCubeEquals(Cube expected, Cube actual)
        {
            Assert.AreEqual(expected.argb_color, actual.argb_color);
            Assert.AreEqual(expected.food, actual.food);
            Assert.AreEqual(expected.IsDead, actual.IsDead);
            Assert.AreEqual(expected.loc_x, actual.loc_x);
            Assert.AreEqual(expected.loc_y, actual.loc_y);
            Assert.AreEqual(expected.Mass, actual.Mass);
            Assert.AreEqual(expected.name, actual.name);
            Assert.AreEqual(expected.team_id, actual.team_id);
            Assert.AreEqual(expected.uid, actual.uid);
            Assert.AreEqual(expected.Width, actual.Width);
        }

        /// <summary>
        /// This keeps track of the event handler so that it can be cleaned up from the World.
        /// </summary>
        private WorldChangedEventHandler handler = null;

        /// <summary>
        /// This method returns an event handler that verifies the expected and deleted cells.
        /// </summary>
        /// <param name="expected">The expected changed cells.</param>
        /// <param name="deleted">The expected deleted cells.</param>
        public void GetHandlerChecker(HashSet<Cube> expected, HashSet<Cube> deleted)
        {
            lock(this)
            {
                this.ClearHandler();
                this.handler = (origin, changed, actDeleted) =>
                   {
                       Assert.IsTrue(expected.SetEquals(changed));
                       Assert.IsTrue(deleted.SetEquals(actDeleted));
                   };

                World.WorldSingleton.WorldChangedEvent += this.handler;
            }
        }

        /// <summary>
        /// This method clears out the current event handler from the world singleton.
        /// </summary>
        private void ClearHandler()
        {
            lock(this)
            {
                try
                {
                    World.WorldSingleton.WorldChangedEvent -= this.handler;
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// This method clears out the world for reuse.
        /// </summary>
        [TestInitialize, TestCleanup]
        public void ClearWorldOut()
        {
            lock(this)
            {
                this.ClearHandler();
                Cube[] cubes = World.WorldSingleton.ToArray<Cube>();
                foreach(Cube cube in cubes)
                {
                    cube.Mass = 0;
                    World.WorldSingleton.UpdateCubes(new Cube[] { cube });
                }
            }

            this.CubeOne.argb_color = -65536;
            this.CubeOne.loc_x = 926.0;
            this.CubeOne.loc_y = 682.0;
            this.CubeOne.food = false;
            this.CubeOne.name = "3500 is love";
            this.CubeOne.Mass = 1000.0;

            this.CubeTwo.argb_color = -42;
            this.CubeTwo.loc_x = 500;
            this.CubeTwo.loc_y = 600;
            this.CubeTwo.food = true;
            this.CubeTwo.name = "Dyllon";
            this.CubeTwo.Mass = 50;
        }

        /// <summary>
        /// This method tests that all things in the world can be enumerated over.
        /// </summary>
        [TestMethod]
        public void TestEnumerateWorld()
        {
            lock(this)
            {
                Cube cube1 = new Cube(1);
                Cube cube2 = new Cube(2);
                cube1.Mass = cube2.Mass = 1.0;
                HashSet<Cube> cubes = new HashSet<Cube>() { cube1, cube2 };
                HashSet<Cube> empty = new HashSet<Cube>();
                foreach (Cube cube in cubes)
                {
                    this.GetHandlerChecker(new HashSet<Cube>() { cube }, empty);
                    World.WorldSingleton.UpdateCubes(new Cube[] { cube });
                }

                Assert.IsTrue(cubes.SetEquals(World.WorldSingleton));
            }
        }

        /// <summary>
        /// This method checks that the maximum x value of the world is calculated correctly.
        /// </summary>
        [TestMethod]
        public void TestMaximumSizeX()
        {
            Cube cube = new Cube(1);
            cube.loc_x = 42;
            Cube cube2 = new Cube(2);
            cube.Mass = cube2.Mass = 1;
            World.WorldSingleton.UpdateCubes(new Cube[] { cube, cube2 });
            Assert.AreEqual(42, World.WorldSingleton.MaxSizeX);
        }

        /// <summary>
        /// This method checks that the maximum Y value of the world is correctly computed.
        /// </summary>
        [TestMethod]
        public void TestMaximumSizeY()
        {
            Cube cube = new Cube(1);
            cube.loc_y = 42;
            Cube cube2 = new Cube(2);
            cube.Mass = cube2.Mass = 1;
            World.WorldSingleton.UpdateCubes(new Cube[] { cube, cube2 });
            Assert.AreEqual(42, World.WorldSingleton.MaxSizeY);
        }

        [TestMethod]
        public void TestReferenceEqualityPresevered()
        {
            Cube old = new Cube(1);
            old.Mass = 1;
            Cube newCube = new Cube(1);
            newCube.Mass = 13;
            World.WorldSingleton.UpdateCubes(new Cube[] { old });
            World.WorldSingleton.UpdateCubes(new Cube[] { newCube });

            bool pass = false;
            foreach(Cube cube in World.WorldSingleton)
            {
                if (ReferenceEquals(cube, old))
                {
                    Assert.AreEqual(13, old.Mass);
                    Assert.AreEqual(13, cube.Mass);
                    pass = true;
                }
            }

            Assert.IsTrue(pass);
        }

        /// <summary>
        /// This method tests that one Json cube can be added to the world.
        /// </summary>
        [TestMethod]
        public void TestAddJsonCubes()
        {
            World.WorldSingleton.ProcessCubeJson(WorldTest.CubeOneString);
            foreach(Cube cube in World.WorldSingleton)
            {
                if (cube.uid == this.CubeOne.uid)
                {
                    AssertCubeEquals(CubeOne, cube);
                    return;
                }
            }

            Assert.Fail("Cube with uid:" + CubeOne.uid + " was not added.");
        }

        /// <summary>
        /// Tests that mass is accumlated.
        /// </summary>
        [TestMethod]
        public void TestGetTrueMass()
        {
            Cube cube1 = new Cube(1, 0);
            cube1.Mass = 1000;
            Cube cube2 = new Cube(2, 1);
            cube2.Mass = 1000;
            Cube other = new Cube(3);
            other.Mass = 1000;
            World.WorldSingleton.UpdateCubes(new Cube[] { cube1, cube2, other });
            Assert.AreEqual(2000, cube1.TrueMass, 0.0001);
        }

        /// <summary>
        /// This method tests getting the closest cube.
        /// </summary>
        [TestMethod]
        public void TestGetClosestCube()
        {
            Cube food = new Cube(0);
            food.food = true;
            food.Mass = 100;
            food.loc_y = food.loc_x = 0;

            Cube main = new Cube(1);
            main.food = false;
            main.Mass = 100;
            main.loc_x = main.loc_y = 0;

            Cube close = new Cube(2);
            close.food = false;
            close.Mass = 100;
            close.loc_x = close.loc_y = 100;

            Cube far = new Cube(3);
            far.food = false;
            far.Mass = 100;
            far.loc_y = Double.MaxValue;
            far.loc_x = 0;

            Cube far2 = new Cube(4);
            far2.food = false;
            far2.Mass = 100;
            far2.loc_x = Double.MaxValue;
            far2.loc_y = 0;

            World.WorldSingleton.UpdateCubes(new Cube[] { main, food, close, far, far2 });
            Assert.AreEqual(close, World.WorldSingleton.GetClosestCube(main));
        }

        /// <summary>
        /// This method tests that the world can be cleared quitely.
        /// </summary>
        [TestMethod]
        public void TestClearWorld()
        {
            Cube cube1 = new Cube(1);
            Cube cube2 = new Cube(2);
            cube1.Mass = cube2.Mass = 2000;
            World.WorldSingleton.UpdateCubes(new Cube[] { cube1, cube2 });
            WorldChangedEventHandler hand = (var1, var2, var3) => Assert.Fail("Trigered an event.");
            World.WorldSingleton.WorldChangedEvent += hand;
            World.WorldSingleton.ClearWorld();
            bool failure = false;
            foreach(Cube cube in World.WorldSingleton)
            {
                failure = true;
            }
            Assert.IsFalse(failure);
            World.WorldSingleton.WorldChangedEvent -= hand;
        }

        /// <summary>
        /// This method tests that multiple json cubes can be added in one call.
        /// </summary>
        [TestMethod]
        public void TestAddMultipleJsonCubes()
        {
            string message = "\n\n" + CubeOneString + "\n\n\n\n" + CubeTwoString + "\n\n\n";
            World.WorldSingleton.ProcessCubeJson(message);
            int found = 0;
            foreach (Cube cube in World.WorldSingleton)
            {
                if (cube.uid == this.CubeOne.uid)
                {
                    AssertCubeEquals(CubeOne, cube);
                    found++;
                }
                else if (cube.uid == this.CubeTwo.uid)
                {
                    AssertCubeEquals(CubeTwo, cube);
                    found++;
                }
            }

            Assert.AreEqual(2, found);
        }
    }
}
