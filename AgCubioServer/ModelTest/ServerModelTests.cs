// This code is licensed under the GPL v2.0 by Dyllon Gagnier and Rachel Saya.

using System;
using System.Drawing;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using System.Threading;
using System.Linq;

namespace ModelTest
{
    [TestClass]
    public class ServerModelTests
    {
        /// <summary>
        /// Tests isDead Method
        /// </summary>
        [TestMethod]
        public void TestIsDeadSet()
        {
            Cube player = new Cube(1, 1);
            player.IsDead = true;
            Assert.AreEqual(0, player.Mass);
        }

        /// <summary>
        /// Tests attrition method
        /// </summary>
        [TestMethod]
        public void TestAttrition()
        {
            Cube player = new Cube(1, 1);
            player.Mass = 10;
            player.Attrition();
            Assert.AreEqual(10, player.Mass);

            player.Mass = 1000;
            player.Attrition();
            Assert.AreNotEqual(1000, player.Mass);
        }

        /// <summary>
        /// Tests desired location
        /// </summary>
        [TestMethod]
        public void TestDesiredLocation()
        {
            Cube player = new Cube(1, 1);
            Point point = new Point(2, 3);
            player.DesiredLocation = point;
            Assert.AreEqual(point, player.DesiredLocation);
        }

        /// <summary>
        /// Tests speed, momentum, and split.
        /// </summary>
        [TestMethod]
        public void TestSpeedSplit()
        {
            Cube player = new Cube(1, 1);
            player.Mass = 1000;
            double speed = player.Speed;
            player.Split(true);
            Assert.AreNotEqual(speed, player.Speed);
        }

        /// <summary>
        /// Tests ServerTick.
        /// </summary>
        [TestMethod]
        public void TestServerTick()
        {
            Cube player = new Cube(1, 1);
            bool toTrue = false;
            bool test = player.ServerTick(() => { toTrue = true; });

            Assert.AreEqual(true, test);
        }

        /// <summary>
        /// Tests move.
        /// </summary>
        [TestMethod]
        public void TestMove()
        {
            Cube player = new Cube(1, 1);
            Point point = new Point(2, 3);
            player.DesiredLocation = point;
            player.ServerTick(() => { });

            Assert.AreEqual(false, player.MoveRequested);
        }

        /// <summary>
        /// Tests eat cube.
        /// </summary>
        [TestMethod]
        public void TestEatCube()
        {
            Cube player = new Cube(1, 1);
            player.Mass = 1000;

            Cube secondPlayer = new Cube(2, 2);
            secondPlayer.Mass = 500;

            player.EatCube(secondPlayer, () => { });
            Assert.AreEqual(1500, player.Mass);
        }

        /// <summary>
        /// Tests initialize world.
        /// </summary>
        [TestMethod]
        public void TestInitializeWorld()
        {
            ServerWorld world = new ServerWorld();
            world.InitializeWorld();         

            Assert.AreEqual(world.NumberOfFoods, 5000);
        }

        /// <summary>
        /// Tests player server tick.
        /// </summary>
        [TestMethod]
        public void TestPlayerServerTick()
        {
            Player test = Player.GetNewPlayer();

            test.SplitLocation = new Point(0, 0);
            Assert.AreEqual(new Point(0, 0), test.SplitLocation);
            test.ServerTick();
            Assert.AreEqual(test.GetAllCubes().Count<Cube>(), 2);
            Thread.Sleep((int)(1000 * GlobalConstants.SplitTime * 1.1));
            test.ServerTick();
            Assert.AreEqual(test.GetAllCubes().Count<Cube>(), 1);


        }






    }
}
