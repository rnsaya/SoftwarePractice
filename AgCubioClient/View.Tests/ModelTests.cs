
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelTests
{
    [TestClass]
    public class WorldTests
    {
        /// <summary>
        ///     Tests for the world and cube adding
        /// </summary>
        [TestMethod]
        public void WorldTest()
        {
            World world = new World();
            Cube cube = new Cube(1, 1, 34324, 2342, true, "test", 20, 23342);
        }

        /// <summary>
        ///     Tests for the adding
        /// </summary>
        [TestMethod]
        public void WorldTestAdd()
        {
            World world = new World();
            Cube cube = new Cube(1, 1, 12312, 123123, true, "", 23, 234);
            world.Add(cube);
            Assert.IsTrue(world.dict.ContainsValue(cube));
        }

        /// <summary>
        ///     Tests for the value being removed
        /// </summary>
        [TestMethod]
        public void WorldTestRemove()
        {
            World world = new World();
            Cube cube = new Cube(1, 1, 12312, 123123, true, "", 23, 234);
            world.Add(cube);
            world.Remove(cube);
            Assert.IsFalse(world.dict.Any());
        }

        /// <summary>
        ///     Tests addomg a duplicate.
        /// </summary>
        [TestMethod]
        public void WorldTestDuplicate()
        {
            World world = new World();
            Cube cube = new Cube(1, 1, 12312, 123123, true, "", 23, 234);
            world.Add(cube);
            world.Add(cube);
        }

    } // End of Tests Class

} // End of Namespace