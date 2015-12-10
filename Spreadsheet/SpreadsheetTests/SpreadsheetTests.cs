//Created by Rachel Saya for CS 3500, September 2015

using SS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using SpreadsheetUtilities;
using System.Threading;

namespace SpreadsheetTester
{


    /// <summary>
    ///This is a test class for Spreadsheet
    ///</summary>
    [TestClass()]
    public class SpreadsheetTests {
        private TestContext testContextIns;
        public TestContext TestContext  {
            get {
                return testContextIns;
            }
            set {
                testContextIns = value;
            }
        }
 
        public IEnumerable<string> Set(AbstractSpreadsheet sheet, string name, string contents)
        {
            List<string> result = new List<string>(sheet.SetContentsOfCell(name, contents));
            return result;
        }

        //Tests isValid method
        [TestMethod()]
        public void valid()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("K2", "k");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void notValid()
        {
            AbstractSpreadsheet ss = new Spreadsheet(s => s[0] != 'B', s => s, "");
            ss.SetContentsOfCell("B1", "c");
        }


        // Tests Normalize method
        [TestMethod()]
        public void Normalize()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "ThisISAtest");
            Assert.AreEqual("", s.GetCellContents("a1"));
        }

        //Test Changed method
        [TestMethod()]
        public void Changed()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Assert.IsTrue(!ss.Changed);
            Set(ss, "A1", "6.6");
            Assert.IsTrue(ss.Changed);
        }

        //Test Formula Errors
        public void DdivideZero(AbstractSpreadsheet ss)
        {
            Set(ss, "K1", "3");
            Set(ss, "A1", "= K1 / 0.0");
            Assert.IsInstanceOfType(ss.GetCellValue("A1"), typeof(FormulaError));
        }

        // XML tests
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SaveFail()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.Save("q:\\missing\\save.txt");
        }
        [TestMethod()]
        public void Save()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            Set(s1, "A1", "hi");
            s1.Save("test1.txt");
            s1 = new Spreadsheet("test1.txt", s => true, s => s, "default");
            Assert.AreEqual("hi", s1.GetCellContents("A1"));
        }

    }
}