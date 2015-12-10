//Written by Rachel Saya for CS 3500

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SpreadsheetUtilities;

namespace FormulaTester
{
    [TestClass]
    public class FormulaTestere
    {
        [TestMethod()]
        public void Test1ToString()                                     // Test ToString
        {
            Formula f = new Formula("5*5");
            Assert.IsTrue(f.Equals(new Formula(f.ToString())));
        }

        [TestMethod()]
        public void Test2DivideByZero()                                 // Test for FormulaError
        {
            Formula f = new Formula("2/0");
            Assert.IsInstanceOfType(f.Evaluate(s => 0), typeof(FormulaError));
        }    

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test3ExtraParanthesis()                             // Tests Syntax Errors
        {
            Formula f = new Formula("1+2*3*3)1");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test4NoNum()
        {
            Formula f = new Formula("/");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test5Null()
        {
            Formula f = new Formula(null);
        }

        [TestMethod()]
        public void Test6EqualTrue()                                    // Tests ==
        {
            Formula f1 = new Formula("1 + 2");
            Formula f2 = new Formula("1 + 2");
            Assert.IsTrue(f1 == f2);
        }

        [TestMethod()]
        public void Test7EqualFalse()
        {
            Formula f1 = new Formula(" ");
            Formula f2 = new Formula("5");
            Assert.IsFalse(f1 == f2);
        }

        [TestMethod()]
        public void Test8NotEqualFalse()                                 // Tests of != 
        {
            Formula f1 = new Formula("1 + 2");
            Formula f2 = new Formula("1 + 2");
            Assert.IsFalse(f1 != f2);
        }

        [TestMethod()]
        public void Test9NotEqualTrue()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula(" ");
            Assert.IsTrue(f1 != f2);
        }
        [TestMethod()]
        public void Test10Spaces()                                         // Test Equals
        {
            Formula f1 = new Formula("X1+      Y2");
            Formula f2 = new Formula("X1+Y2");
            Assert.IsTrue(f1.Equals(f2));
        }

        [TestMethod()]
        public void Test11SameHash()                                       // Tests of GetHashCode method
        {
            Formula f1 = new Formula("2/1");
            Formula f2 = new Formula("2/1");
            Assert.IsTrue(f1.GetHashCode() == f2.GetHashCode());
        }

        [TestMethod()]
        public void Test12DifferentHash()
        {
            Formula f1 = new Formula("2*1");
            Formula f2 = new Formula("2*2");
            Assert.IsTrue(f1.GetHashCode() != f2.GetHashCode());
        }







    }
}

