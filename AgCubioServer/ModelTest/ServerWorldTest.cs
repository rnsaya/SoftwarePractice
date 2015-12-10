using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace ModelTest
{
    [TestClass]
    public class ServerWorldTest
    {
        /// <summary>
        /// This tests that viruses have the right properties.
        /// </summary>
        [TestMethod]
        public void TestCreateVirus()
        {
            Cube virus = World.ServerSingleton.CreateVirus();
            Assert.AreEqual(true, virus.IsVirus);
            Assert.AreEqual(GlobalConstants.VirusMass, virus.Mass);
            Assert.AreEqual(GlobalConstants.VirusColor, virus.argb_color);
            Assert.AreEqual(true, virus.food);
        }

        /// <summary>
        /// This tests that a cube can eat food at a server heartbeat.
        /// </summary>
        [TestMethod]
        public void EatFoodTest()
        {
            ServerWorld world = new ServerWorld();
            world.InitializeWorld();
            Assert.AreEqual(GlobalConstants.MaximumFood, world.NumberOfFoods);

            int playerID = world.GetNewUID();
            Cube bigOldCube = world.CreateVirus();
            bigOldCube.food = false;
            bigOldCube.Mass = 10000;

            world.UpdateCubes(new Cube[] { bigOldCube });
            world.ServerHeartbeat((num) => null);

            Assert.AreNotEqual(GlobalConstants.MaximumFood, world.NumberOfFoods);

        }
    }
}
