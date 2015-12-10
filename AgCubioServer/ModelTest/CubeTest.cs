// This code is licensed under the GPL v2.0 by Dyllon Gagnier and Ross DiMassino.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Drawing.Design;

namespace Model
{
    /// <summary>
    /// This class is the main test class for Cube.
    /// </summary>
    [TestClass]
    public class CubeTest
    {
        /// <summary>
        /// This method tests the creation of a cube and its default values.
        /// </summary>
        [TestMethod]
        public void DefaultConstructorTest()
        {
            Cube test = new Cube(1234);
            Assert.AreEqual(0, test.argb_color);
            Assert.AreEqual(false, test.food);
            Assert.AreEqual(0, test.loc_x);
            Assert.AreEqual(0, test.loc_y);
            Assert.AreEqual(0, test.Mass);
            Assert.IsNull(test.name);
            Assert.AreEqual(1234, test.uid);
            Assert.AreEqual(0, test.Width);
        }

        /// <summary>
        /// This method tests that the width is the sqrt of the Mass.
        /// </summary>
        [TestMethod]
        public void TestWidth()
        {
            Cube test = new Cube(0);
            test.Mass = 2;
            Assert.AreEqual(Math.Sqrt(2), test.Width);
        }

        /// <summary>
        /// This method tests that two cubes which differ in everything except their uid
        /// are still equal.
        /// </summary>
        [TestMethod]
        public void TestDisparateEquals()
        {
            Cube test1 = new Cube(1234);
            Cube test2 = new Cube(1234);
            test2.argb_color = 32;
            test2.food = true;
            test2.loc_x = 14;
            test2.loc_y = 15;
            test2.Mass = 400;
            test2.name = "Sally";
            Assert.AreEqual(test1, test2);
        }

        /// <summary>
        /// This method tests two cubes identical except for uid are not equal.
        /// </summary>
        [TestMethod]
        public void TestAlmostEquals()
        {
            Cube test1 = new Cube(1234);
            Cube test2 = new Cube(1);
            Assert.AreNotEqual(test1, test2);
        }

        /// <summary>
        /// This method verfies that the hash code is the UID.
        /// </summary>
        [TestMethod]
        public void HashCodeIsUID()
        {
            int id = 1234;
            Cube test1 = new Cube(id);
            Assert.AreEqual(id.GetHashCode(), test1.GetHashCode());
        }

        /// <summary>
        /// This method tests that a Cube is not equal to something else.
        /// </summary>
        [TestMethod]
        public void DiffTypeNotEquals()
        {
            Cube test = new Cube(1234);
            Assert.IsFalse(test.Equals(42));
        }
        
        /// <summary>
        /// This method tests that cube is not equal to null.
        /// </summary>
        [TestMethod]
        public void TestNotEqualsNull()
        {
            Cube test = new Cube(1234);
            Assert.IsFalse(test.Equals(null));
        }

        /// <summary>
        /// This method tests that a cell with no mass is dead.
        /// </summary>
        [TestMethod]
        public void TestIsDead()
        {
            Cube test = new Cube(1234);
            test.Mass = 0.0;
            Assert.IsTrue(test.IsDead);
        }

        /// <summary>
        /// This method checks that a cell with mass is not dead.
        /// </summary>
        [TestMethod]
        public void TestNotDead()
        {
            Cube test = new Cube(1234);
            test.Mass = 1.0;
            Assert.IsFalse(test.IsDead);
        }
        
        /// <summary>
        /// This method test that the uid and the team id can be set separately.
        /// </summary>
        [TestMethod]
        public void TestJsonConstructor()
        {
            Cube test = new Cube(1, 2);
            Assert.AreEqual(1, test.uid);
            Assert.AreEqual(2, test.team_id);
        }
    }
}
