using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using System.Linq;
using System.Drawing;

namespace ModelTest
{
    [TestClass]
    public class PlayerTest
    {

        /// <summary>
        /// This tests Virus splitting splits to 3 cubes.
        /// </summary>
        [TestMethod]
        public void VirusSplitTest()
        {
            Player player = Player.GetNewPlayer();
            player.VirusSplit(player.PlayerCube);
            Assert.AreEqual(3, player.GetAllCubes().Count<Cube>());
        }

        /// <summary>
        /// This method tests setting location sets location for all cubes.
        /// </summary>
        [TestMethod]
        public void SetDesiredLocationTest()
        {
            Player player = Player.GetNewPlayer();

            // Easy way to get three cubes.
            player.VirusSplit(player.PlayerCube);

            Point expected = new Point(30, 42);
            player.DesiredLocation = expected;
            foreach (Cube cube in player.GetAllCubes())
            {
                Assert.AreEqual(0, cube.Momentum);
                Assert.AreEqual(expected, cube.DesiredLocation);
                Assert.IsTrue(cube.MoveRequested);
            }
        }
        
        /// <summary>
        /// This tests that received name is true iff player.Name = ... has been called.
        /// </summary>
        [TestMethod]
        public void TestReceivedName()
        {
            Player player = Player.GetNewPlayer();
            Assert.IsFalse(player.ReceivedName);
            string name = "Harry Potter";
            player.Name = name;
            Assert.IsTrue(player.ReceivedName);
            Assert.AreEqual(name, player.Name);
        }

        /// <summary>
        /// This tests that player.UID == player.PlayerCube.uid
        /// </summary>
        [TestMethod]
        public void TestGetUID()
        {
            Player player = Player.GetNewPlayer();
            Assert.AreEqual(player.PlayerCube.uid, player.UID);
        }
    }
}
