// Written by Nate Merrill

using System;
using SS;
using SpreadsheetUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SpreadsheetTests
{
    /// <summary>
    /// Tests the Spreadsheet class
    /// </summary>
    [TestClass]
    public class SpreadSheetUnitTests
    {
        /// <summary>
        /// Hashset to contain all the valid variables for the spreadsheet object.
        /// </summary>
        private HashSet<string> validVar;

        /// <summary>
        /// This will intalize the isValid Func given to some spreadsheet objects
        /// </summary>
        [TestInitialize]
        public void IntializeTests ()
        {
            validVar = new HashSet<string>();
            fillSet();
        }

        //====================================================================================================================================================
        //                                                                  PS4 Public Tets
        //====================================================================================================================================================


        /// <summary>
        /// Determines if an empty Spreadsheet is indeed an AbstractSpreadsheet
        /// </summary>
        [TestMethod]
        public void SpreadsheetConstructorTest1()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
        }

        /// <summary>
        /// Tests whether SetContentsOfCell works for a single cell with no dependents
        /// </summary>
        [TestMethod]
        public void SpreadsheetSetContentsOfCellTest1()
        {
            AbstractSpreadsheet ss = new Spreadsheet();

            ISet<string> rSet = ss.SetContentsOfCell("A1", "1");

            Assert.AreEqual(1, rSet.Count);
            Assert.IsTrue(rSet.Contains("A1"));
        }

        /// <summary>
        /// Tests whether SetContentsOfCell works for a single cell with no dependents
        /// </summary>
        [TestMethod]
        public void SpreadsheetSetContentsOfCellTest2()
        {
            AbstractSpreadsheet ss = new Spreadsheet();

            ISet<string> rSet = ss.SetContentsOfCell("A1", "No Value");

            Assert.AreEqual(1, rSet.Count);
            Assert.IsTrue(rSet.Contains("A1"));
        }

        /// <summary>
        /// Tests whether SetContentsOfCell works for a single cell with one dependent
        /// </summary>
        [TestMethod]
        public void SpreadsheetSetContentsOfCellTest3()
        {
            AbstractSpreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("B1", "=A1*2");
            ISet<string> rSet = ss.SetContentsOfCell("A1", "1");

            Assert.AreEqual(2, rSet.Count);
            Assert.IsTrue(rSet.Contains("A1") && rSet.Contains("B1"));
        }

        /// <summary>
        /// Tests whether SetContentsOfCell works for a single cell with multiple dependent
        /// </summary>
        [TestMethod]
        public void SpreadsheetSetContentsOfCellTest4()
        {
            AbstractSpreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("B1","=A1*2");
            ss.SetContentsOfCell("C1", "=A1 + B1");// throws circular exception on B1
            ISet<string> rSet = ss.SetContentsOfCell("A1", "1");

            Assert.AreEqual(3, rSet.Count);
            Assert.IsTrue(rSet.Contains("A1") && rSet.Contains("B1") && rSet.Contains("C1"));
        }


        /// <summary>
        /// Tests whether SetContentsOfCell works for a single cell with multiple dependent
        /// </summary>
        [TestMethod]
        public void SpreadsheetSetContentsOfCellTest5()
        {
            AbstractSpreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("B1", "=A1*2");
            ss.SetContentsOfCell("C1", "=A1 + B1");
            ISet<string> rSet = ss.SetContentsOfCell("A1", "Perfect Cell");

            Assert.AreEqual(3, rSet.Count);
            Assert.IsTrue(rSet.Contains("A1") && rSet.Contains("B1") && rSet.Contains("C1"));
        }


        /// <summary>
        /// Tests whether SetContentsOfCell works for a single cell with multiple dependent
        /// </summary>
        [TestMethod]
        public void SpreadsheetSetContentsOfCellTest6()
        {
            AbstractSpreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("A1", "=B1+C1");
            ss.SetContentsOfCell("A1", "1");
            ss.SetContentsOfCell("B1", "=D1*2");

            ISet<string> rSet = ss.SetContentsOfCell("B1", "2");

            Assert.AreEqual(1, rSet.Count);
            Assert.IsTrue(rSet.Contains("B1"));
        }


        /// <summary>
        /// Tests whether SetContentsOfCell works for a single cell with multiple dependent
        /// </summary>
        [TestMethod]
        public void SpreadsheetSetContentsOfCellTest7()
        {
            AbstractSpreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("A1", "=B1+C1");
            ss.SetContentsOfCell("A1", "one");
            ss.SetContentsOfCell("B1", "=D1*2");

            ISet<string> rSet = ss.SetContentsOfCell("B1", "two");

            Assert.AreEqual(1, rSet.Count);
            Assert.IsTrue(rSet.Contains("B1"));
        }

        /// <summary>
        /// Tests whether SetContentsOfCell works for a single cell with multiple dependents
        /// </summary>
        [TestMethod]
        public void SpreadsheetSetContentsOfCellTest8()
        {
            AbstractSpreadsheet ss = new Spreadsheet();

            
            ss.SetContentsOfCell("B1", "=A1*A1");
            ss.SetContentsOfCell("C1", "=A1+B1");
            ISet<string> rSet = ss.SetContentsOfCell("A1", "2");

            Assert.AreEqual(3, rSet.Count);
            Assert.IsTrue(rSet.Contains("C1") && rSet.Contains("B1") && rSet.Contains("A1"));
        }

        /// <summary>
        /// Tests whether SetContentsOfCell works for a single cell with multiple dependents
        /// </summary>
        [TestMethod]
        public void SpreadsheetSetContentsOfCellTest9()
        {
            AbstractSpreadsheet ss = new Spreadsheet();


            ss.SetContentsOfCell("B1", "=A1*A1");
            ss.SetContentsOfCell("C1", "=B1");
            ISet<string> rSet = ss.SetContentsOfCell("A1", "2");

            Assert.AreEqual(3, rSet.Count);
            Assert.IsTrue(rSet.Contains("C1") && rSet.Contains("B1") && rSet.Contains("A1"));
        }

        /// <summary>
        /// Tests whether SetContentsOfCell works for a single cell with multiple dependents
        /// </summary>
        [TestMethod]
        public void SpreadsheetSetContentsOfCellTest10()
        {
            AbstractSpreadsheet ss = new Spreadsheet();


            ISet<string> rSet = ss.SetContentsOfCell("A1", " =A1*A1");

            Assert.AreEqual(1, rSet.Count);
            Assert.AreEqual(" =A1*A1", ss.GetCellValue("A1"));
        }


        /// <summary>
        /// Tests whether SetContentsOfCell works for a cell with a Formula and when a change is made, the value of the cell with the formula updates
        /// appropriately.
        /// </summary>
        [TestMethod]
        public void SpreadsheetSetContentsOfCellTest11()
        {
            AbstractSpreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("A1", "=B1*C1");
            ss.SetContentsOfCell("B1", "=D1+2");
            ss.SetContentsOfCell("C1", "5");
            ss.SetContentsOfCell("D1", "4");

            Assert.AreEqual(30.0, ss.GetCellValue("A1"));

            ss.SetContentsOfCell("D1", "5");

            Assert.AreEqual(35.0, ss.GetCellValue("A1"));

            ss.SetContentsOfCell("D1", "5");

            Assert.AreEqual(35.0, ss.GetCellValue("A1"));
        }

        /// <summary>
        /// Tests whether SetContentsOfCell works for a cell with a Formula and when a change is made, the value of the cell with the formula updates
        /// appropriately.
        /// </summary>
        [TestMethod]
        public void SpreadsheetSetContentsOfCellTest12()
        {
            AbstractSpreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("A1", "=B1*C1");
            ss.SetContentsOfCell("B1", "=D1+2");
            ss.SetContentsOfCell("C1", "5");
            ss.SetContentsOfCell("D1", "4");

            Assert.AreEqual(30.0, ss.GetCellValue("A1"));

            ss.SetContentsOfCell("D1", "Here comes the fail train");

            Assert.IsTrue(ss.GetCellValue("A1") is FormulaError);
        }

        /// <summary>
        /// Tests whether getCellContents returns the right object after it has been created
        /// </summary>
        [TestMethod]
        public void SpreadsheetGetCellContentsTest1()
        {
            AbstractSpreadsheet ss = new Spreadsheet();

            Assert.AreEqual("", ss.GetCellContents("A1"));
            Assert.AreEqual("", ss.GetCellContents("D1"));
        }

        /// <summary>
        /// Tests whether getCellContents returns the right object and contents even if the cell names are similar
        /// </summary>
        [TestMethod]
        public void SpreadsheetGetCellContentsTest2()
        {
            AbstractSpreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("x1", "2");
            ss.SetContentsOfCell("X1", "3");

            Assert.AreEqual(2.0, ss.GetCellContents("x1"));
            Assert.AreEqual(3.0, ss.GetCellContents("X1"));
        }

        /// <summary>
        /// Tests whether getCellContents returns the right object after it has been created
        /// </summary>
        [TestMethod]
        public void SpreadsheetGetCellContentsTest3()
        {
            AbstractSpreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("B1", "=A1*2");
            ss.SetContentsOfCell("C1", "simple");
            ISet<string> rSet = ss.SetContentsOfCell("A1", "1");
            ss.SetContentsOfCell("x1", "2");
            ss.SetContentsOfCell("X1", "3");

            Assert.AreEqual(2, rSet.Count);

            Assert.AreEqual(1.0, ss.GetCellContents("A1"));
            Assert.AreEqual(new Formula("A1*2"), ss.GetCellContents("B1"));
            Assert.AreEqual("simple", ss.GetCellContents("C1"));
            Assert.AreEqual("", ss.GetCellContents("D1"));
            Assert.AreEqual(2.0, ss.GetCellContents("x1"));
            Assert.AreEqual(3.0, ss.GetCellContents("X1"));
        }

        /// <summary>
        /// If passed an empty spreadsheet, GetNameOfNonEmptyCells shold return an empty string. 
        /// </summary>
        [TestMethod]
        public void SpreadSheetGetNamesOfNonemptyCellsTest0()
        {
            Spreadsheet ss = new Spreadsheet();
            foreach(string s in ss.GetNamesOfAllNonemptyCells())

            Assert.AreEqual("", s);
        }

        /// <summary>
        /// Tests whether getCellContents returns the right object after it has been created
        /// </summary>
        [TestMethod]
        public void SpreadsheetGetNamesOfNonemptyCellsTest1()
        {
            AbstractSpreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("B1", "=A1*2");
            ss.SetContentsOfCell("C1", "simple");
            ISet<string> rSet = ss.SetContentsOfCell("A1", "1");

            Assert.AreEqual(2, rSet.Count);
            HashSet<string> cellNames = new HashSet<string> { "A1", "B1", "C1" };

            foreach(string s in ss.GetNamesOfAllNonemptyCells())
            {
                if(cellNames.Contains(s))
                {
                    cellNames.Remove(s);
                }
                else
                {
                    Assert.IsTrue(false);
                }
            }
        }

        /// <summary>
        /// Tests whether getCellContents returns the right object after it has been created
        /// </summary>
        [TestMethod]
        public void SpreadsheetGetNamesOfNonemptyCellsTest2()
        {
            AbstractSpreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("B1", "=A1*2");
            ss.SetContentsOfCell("C1", "simple");
            ISet<string> rSet = ss.SetContentsOfCell("A1", "1");
            ss.SetContentsOfCell("x1", "2");
            ss.SetContentsOfCell("X1", "3");

            Assert.AreEqual(2, rSet.Count);
            HashSet<string> cellNames = new HashSet<string> { "A1", "B1", "C1", "x1", "X1" };

            foreach (string s in ss.GetNamesOfAllNonemptyCells())
            {
                if (cellNames.Contains(s))
                {
                    cellNames.Remove(s);
                }
                else
                {
                    Assert.IsTrue(false);
                }
            }
        }

        /// <summary>
        /// Tests to see if you are able to update the contents of a cell after it has been populated
        /// </summary>
        [TestMethod]
        public void SpreadSheetAddNewCellContentsTest1()
        {
            AbstractSpreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("A1", "1");
            ss.SetContentsOfCell("A1", "two");

            Assert.AreEqual("two", ss.GetCellContents("A1"));

        }

        /// <summary>
        /// Tests to see if you are able to update the contents of a cell after it has been populated
        /// </summary>
        [TestMethod]
        public void SpreadSheetAddNewCellContentsTest2()
        {
            AbstractSpreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("A1", "one");
            ss.SetContentsOfCell("A1", "2");

            Assert.AreEqual(2.0, ss.GetCellContents("A1"));

        }

        /// <summary>
        /// Tests to see if you are able to update the contents of a cell after it has been populated
        /// </summary>
        [TestMethod]
        public void SpreadSheetAddNewCellContentsTest3()
        {
            AbstractSpreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("A1", "one");
            ss.SetContentsOfCell("A1", "C1+B1");

            Assert.AreEqual("C1+B1", ss.GetCellContents("A1"));

        }

        /// <summary>
        /// Tests to see if you are able to update the contents of a cell after it has been populated
        /// </summary>
        [TestMethod]
        public void SpreadSheetAddNewCellContentsTest4()
        {
            AbstractSpreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("A1", "one");
            ss.SetContentsOfCell("A1", "=C1+B1");
            ISet<string> rSet = ss.SetContentsOfCell("A1", "1");

            Assert.AreEqual(1, rSet.Count);
            Assert.AreEqual(1.0, ss.GetCellContents("A1"));

        }

        /// <summary>
        /// Tests to see if you are able to update the contents of a cell after it has been populated and it will return all direct and indirect cells
        /// </summary>
        [TestMethod]
        public void SpreadSheetAddNewCellContentsTest5()
        {
            AbstractSpreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("A1", "one");
            ss.SetContentsOfCell("B1", "=A1*A1");
            ss.SetContentsOfCell("X1", "=B1+A1");
            ss.SetContentsOfCell("C1", "=A1+X1");
            ISet<string> rSet = ss.SetContentsOfCell("A1", "1");

            Assert.AreEqual(4, rSet.Count);
            Assert.AreEqual(1.0, ss.GetCellContents("A1"));

        }

        /// <summary>
        /// Tests to see if you are able to update the contents of a cell after it has been populated
        /// </summary>
        [TestMethod]
        public void SpreadSheetAddNewCellContentsTest6()
        {
            AbstractSpreadsheet ss = new Spreadsheet();

            ss.SetContentsOfCell("XY123", "1");
            ss.SetContentsOfCell("XY123", "two");

            Assert.AreEqual("two", ss.GetCellContents("XY123"));
        }


        /// <summary>
        /// Tests to make sure that the formula is not changed after a CircularException is found
        /// </summary>
        [TestMethod]
        public void SpreadsheetCircularExceptionTest1 ()
        {
            Spreadsheet circle = new Spreadsheet();
            try
            {
                circle.SetContentsOfCell("A1", "=B1+C1");
                circle.SetContentsOfCell("B1", "=D1+C1");
                circle.SetContentsOfCell("C1", "1");
                circle.SetContentsOfCell("C1", "=A1+B1");
            }
            catch (CircularException)
            {
                Assert.AreEqual(1.0, circle.GetCellContents("C1"));
                ISet<string> rSet = circle.SetContentsOfCell("C1", "2");

                Assert.AreEqual(3.0, rSet.Count);
                Assert.IsTrue(rSet.Contains("C1"));
            }
        }

        ///// <summary>
        ///// Tests to make sure Cell retains it's name, content and value if changes take place. 
        ///// </summary>
        //[TestMethod]
        //public void CellTest ()
        //{
        //    Cell imperfectCell = new Cell("A1", "I need 17 and 18 to be perfect!");
        //    imperfectCell.cellContent = 1.0;
        //    imperfectCell.SetValueOfCell(null);

        //    Assert.AreEqual(1.0, imperfectCell.cellContent);
        //    Assert.AreEqual(1.0, imperfectCell.cellValue);

        //    imperfectCell.cellContent = "I got 17 and 18!";
        //    imperfectCell.SetValueOfCell(null);
        //    Assert.AreEqual("I got 17 and 18!", imperfectCell.cellContent);
        //    Assert.AreEqual("I got 17 and 18!", imperfectCell.cellValue);

        //    imperfectCell.cellContent = new Formula("17+18");
        //    imperfectCell.SetValueOfCell(s => 0);
        //    Assert.AreEqual(new Formula("17+18"), imperfectCell.cellContent);
        //    Assert.AreEqual(35.0, imperfectCell.cellValue);

        //    Cell perfectCell = new Cell("A2", new Formula("Android17 + Android18"));
        //    perfectCell.SetValueOfCell(s => s == "Android17" ? 1 : 1);

        //    Assert.AreEqual(new Formula("Android17 + Android18"), perfectCell.cellContent);
        //    Assert.AreEqual(2.0, perfectCell.cellValue);
        //}

        //====================================================================================================================================================
        //                                                                  PS5 Public Tets
        //====================================================================================================================================================

        /// <summary>
        /// Tests to see if the spreadsheet class can create a correct Spreadsheet with three arguments
        /// </summary>
        [TestMethod]
        public void Spreadsheet_Three_Argument_Constructor1 ()
        {
            AbstractSpreadsheet tresSpreadsheet = new Spreadsheet(s => true, s => s, "default");

            Assert.AreEqual(true, tresSpreadsheet.IsValid("This is valid"));
            Assert.AreEqual("Normal", tresSpreadsheet.Normalize("Normal"));
            Assert.AreEqual("default", tresSpreadsheet.Version);
            Assert.IsFalse(tresSpreadsheet.Changed);
        }

        /// <summary>
        /// Tests to see if the spreadsheet class can create a correct Spreadsheet with three arguments
        /// </summary>
        [TestMethod]
        public void Spreadsheet_Three_Argument_Constructor2()
        {
            AbstractSpreadsheet tresSpreadsheet = new Spreadsheet(s => true, s => s, "default");

            Assert.AreEqual(true, tresSpreadsheet.IsValid("This is valid"));
            Assert.AreEqual("Normal", tresSpreadsheet.Normalize("Normal"));
            Assert.AreEqual("default", tresSpreadsheet.Version);
            Assert.IsFalse(tresSpreadsheet.Changed);

            tresSpreadsheet.SetContentsOfCell("A1", "2");
            Assert.AreEqual(2.0, tresSpreadsheet.GetCellContents("A1"));
            Assert.IsTrue(tresSpreadsheet.Changed);

            tresSpreadsheet.SetContentsOfCell("A1", "string");
            Assert.AreEqual("string", tresSpreadsheet.GetCellContents("A1"));
            Assert.IsTrue(tresSpreadsheet.Changed);

            tresSpreadsheet.SetContentsOfCell("A1", "=2+3");
            Assert.AreEqual(new Formula("2+3"), tresSpreadsheet.GetCellContents("A1"));
            Assert.IsTrue(tresSpreadsheet.Changed);
        }

        /// <summary>
        /// Tests to see if the spreadsheet class can create a correct Spreadsheet with three arguments and 
        /// </summary>
        [TestMethod]
        public void Spreadsheet_Three_Argument_Constructor_With_Normalizer()
        {
            AbstractSpreadsheet tresSpreadsheet = new Spreadsheet(s => true, s => s.ToUpper(), "default");

            Assert.AreEqual(true, tresSpreadsheet.IsValid("This is valid"));
            Assert.AreEqual("NORMAL", tresSpreadsheet.Normalize("Normal"));
            Assert.AreEqual("default", tresSpreadsheet.Version);
            Assert.IsFalse(tresSpreadsheet.Changed);

            tresSpreadsheet.SetContentsOfCell("A1", "2");
            Assert.AreEqual(2.0, tresSpreadsheet.GetCellContents("A1"));
            Assert.IsTrue(tresSpreadsheet.Changed);

            tresSpreadsheet.SetContentsOfCell("a1", "string");
            Assert.AreEqual("string", tresSpreadsheet.GetCellContents("A1"));
            Assert.IsTrue(tresSpreadsheet.Changed);

            tresSpreadsheet.SetContentsOfCell("a1", "=b1+3");
            Assert.AreEqual(new Formula("B1+3", s => s.ToUpper(), s => true), tresSpreadsheet.GetCellContents("A1"));
            Assert.IsTrue(tresSpreadsheet.Changed);
        }

        /// <summary>
        /// Tests to see if the spreadsheet class can create a correct Spreadsheet with three arguments and 
        /// </summary>
        [TestMethod]
        public void Spreadsheet_Three_Argument_Constructor_With_Validator()
        {
            AbstractSpreadsheet tresSpreadsheet = new Spreadsheet(s => isValid(s), s => s.ToUpper(), "new version");

            Assert.AreEqual(false, tresSpreadsheet.IsValid("This is valid"));
            Assert.AreEqual("NORMAL", tresSpreadsheet.Normalize("Normal"));
            Assert.AreEqual("new version", tresSpreadsheet.Version);
            Assert.IsFalse(tresSpreadsheet.Changed);

            tresSpreadsheet.SetContentsOfCell("A1", "2");
            Assert.AreEqual(2.0, tresSpreadsheet.GetCellContents("A1"));
            Assert.IsTrue(tresSpreadsheet.Changed);

            tresSpreadsheet.SetContentsOfCell("a1", "string");
            Assert.AreEqual("string", tresSpreadsheet.GetCellContents("A1"));
            Assert.IsTrue(tresSpreadsheet.Changed);

            tresSpreadsheet.SetContentsOfCell("a1", "=b1+3");
            Assert.AreEqual(new Formula("B1+3", s => s.ToUpper(), s => isValid(s)), tresSpreadsheet.GetCellContents("A1"));
            Assert.IsTrue(tresSpreadsheet.Changed);

            tresSpreadsheet.SetContentsOfCell("b1", "=c1+e1/(d1)");
            Assert.AreEqual(new Formula("C1+E1/(D1)", s => s.ToUpper(), s => isValid(s)), tresSpreadsheet.GetCellContents("B1"));
            Assert.IsTrue(tresSpreadsheet.Changed);
        }

        /// <summary>
        /// Tests to see if the spreadsheet class can create a correct Spreadsheet with three arguments
        /// </summary>
        [TestMethod]
        public void Spreadsheet_Three_Argument_Cell_Value_test()
        {
            AbstractSpreadsheet tresSpreadsheet = new Spreadsheet(s => isValid(s), s => s.ToUpper(), "default");

            tresSpreadsheet.SetContentsOfCell("B1", "1");
            tresSpreadsheet.SetContentsOfCell("C1", "2");
            tresSpreadsheet.SetContentsOfCell("D1", "3");
            tresSpreadsheet.SetContentsOfCell("E1", "4");

            tresSpreadsheet.SetContentsOfCell("a1", "=b1+3");
            Assert.AreEqual(4.0, tresSpreadsheet.GetCellValue("A1"));
            Assert.IsTrue(tresSpreadsheet.Changed);

            tresSpreadsheet.SetContentsOfCell("b1", "=(c1+e1)/(d1)");
            Assert.AreEqual(2.0, tresSpreadsheet.GetCellValue("B1"));
            Assert.AreEqual(5.0, tresSpreadsheet.GetCellValue("A1"));
            Assert.IsTrue(tresSpreadsheet.Changed);
        }

        /// <summary>
        /// Tests to see if the spreadsheet class can create a correct Spreadsheet with three arguments
        /// </summary>
        [TestMethod]
        public void SpreadsheetFourArgumentConstructor1()
        {
            AbstractSpreadsheet quadSpreadsheet = new Spreadsheet(@"...\...\...\Spreadsheet_Good.xml",
                s => true, s => s, "1.0");

            Assert.AreEqual(true, quadSpreadsheet.IsValid("This is valid"));
            Assert.AreEqual("Normal", quadSpreadsheet.Normalize("Normal"));
            Assert.AreEqual("1.0", quadSpreadsheet.Version);
            Assert.IsFalse(quadSpreadsheet.Changed);
        }

        /// <summary>
        /// Tests to see if the spreadsheet class can create a correct Spreadsheet with three arguments
        /// </summary>
        [TestMethod]
        public void SpreadsheetFourArgumentConstructor2()
        {
            AbstractSpreadsheet quadSpreadsheet = new Spreadsheet(@"...\...\...\Spreadsheet_Good.xml",
                s => true, s => s, "1.0");

            Assert.AreEqual(true, quadSpreadsheet.IsValid("This is valid"));
            Assert.AreEqual("Normal", quadSpreadsheet.Normalize("Normal"));
            Assert.AreEqual("1.0", quadSpreadsheet.Version);
            Assert.IsFalse(quadSpreadsheet.Changed);

            Assert.AreEqual(2.5, quadSpreadsheet.GetCellContents("A1"));
            Assert.AreEqual("cell contents", quadSpreadsheet.GetCellContents("B1"));
            Assert.AreEqual(new Formula("2+A1*2"), quadSpreadsheet.GetCellContents("C1"));

            Assert.AreEqual(2.5, quadSpreadsheet.GetCellValue("A1"));
            Assert.AreEqual("cell contents", quadSpreadsheet.GetCellValue("B1"));
            Assert.AreEqual(7.0, quadSpreadsheet.GetCellValue("C1"));

            quadSpreadsheet.SetContentsOfCell("A1", "5");

            Assert.AreEqual(5.0, quadSpreadsheet.GetCellContents("A1"));
            Assert.AreEqual("cell contents", quadSpreadsheet.GetCellContents("B1"));
            Assert.AreEqual(new Formula("2+A1*2"), quadSpreadsheet.GetCellContents("C1"));

            Assert.AreEqual(5.0, quadSpreadsheet.GetCellValue("A1"));
            Assert.AreEqual("cell contents", quadSpreadsheet.GetCellValue("B1"));
            Assert.AreEqual(12.0, quadSpreadsheet.GetCellValue("C1"));

            Assert.IsTrue(quadSpreadsheet.Changed);

            quadSpreadsheet.Save(@"...\...\...\XMLFile1.xml");

            Assert.IsFalse(quadSpreadsheet.Changed);

            quadSpreadsheet.SetContentsOfCell("A1", "2.5");
            
            Assert.IsTrue(quadSpreadsheet.Changed);

            quadSpreadsheet.Save(@"...\...\...\XMLFile2.xml");

            Assert.IsFalse(quadSpreadsheet.Changed);
        }


        /// <summary>
        /// Tests to see if the spreadsheet class can create a correct Spreadsheet with four arguments
        /// </summary>
        [TestMethod]
        public void SpreadsheetFourArgumentConstructor3()
        {
            AbstractSpreadsheet quadSpreadsheet = new Spreadsheet(@"...\...\...\Spreadsheet_Good.xml",
                s => true, s => s.ToLower(), "1.0");

            Assert.AreEqual(true, quadSpreadsheet.IsValid("This is valid"));
            Assert.AreEqual("normal", quadSpreadsheet.Normalize("Normal"));
            Assert.AreEqual("1.0", quadSpreadsheet.Version);
            Assert.IsFalse(quadSpreadsheet.Changed);

            Assert.AreEqual(2.5, quadSpreadsheet.GetCellContents("a1"));
            Assert.AreEqual("cell contents", quadSpreadsheet.GetCellContents("b1"));
            Assert.AreEqual(new Formula("2+a1*2"), quadSpreadsheet.GetCellContents("c1"));

            Assert.AreEqual(2.5, quadSpreadsheet.GetCellValue("a1"));
            Assert.AreEqual("cell contents", quadSpreadsheet.GetCellValue("b1"));
            Assert.AreEqual(7.0, quadSpreadsheet.GetCellValue("c1"));
        }

        /// <summary>
        /// Tests to see if GetVersion returns the correct output if given a good file
        /// </summary>
        [TestMethod]
        public void SpreadsheetGetVersionTest()
        {
            Assert.AreEqual("1.0", new Spreadsheet().GetSavedVersion(@"...\...\...\Spreadsheet_Good.xml"));
        }

        //===============================================================================================================================================
        //                                                                 PS4 Exception Testing
        //===============================================================================================================================================



        /// <summary>
        /// Tests whether an InvalidNameException is thrown when an invalid cell name is passed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SpreadsheetGetContentsOfCellExceptionTest1()
        {
            Spreadsheet exceptSS = new Spreadsheet();

            exceptSS.GetCellContents(null);
        }

        /// <summary>
        /// Tests whether an InvalidNameException is thrown when an invalid cell name is passed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SpreadsheetGetContentsOfCellExceptionTest2 ()
        {
            Spreadsheet exceptSS = new Spreadsheet();

            exceptSS.GetCellContents("25");
        }

        /// <summary>
        /// Tests whether an InvalidNameException is thrown when an invalid cell name is passed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SpreadsheetGetContentsOfCellExceptionTest3 ()
        {
            Spreadsheet exceptSS = new Spreadsheet();

            exceptSS.GetCellContents("25x");
        }

        /// <summary>
        /// Tests whether an InvalidNameException is thrown when an invalid cell name is passed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SpreadsheetGetContentsOfCellExceptionTest4()
        {
            Spreadsheet exceptSS = new Spreadsheet();

            exceptSS.GetCellContents("?");
        }


        /// <summary>
        /// Tests whether an InvalidNameException is thrown when an invalid cell name is passed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SpreadsheetSetContentsOfCellExceptionTest1()
        {
            Spreadsheet exceptSS = new Spreadsheet();

            exceptSS.SetContentsOfCell(null, "1");
        }

        /// <summary>
        /// Tests whether an InvalidNameException is thrown when an invalid cell name is passed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SpreadsheetSetContentsOfCellExceptionTest2()
        {
            Spreadsheet exceptSS = new Spreadsheet();

            exceptSS.SetContentsOfCell("2x", "1");
        }

        /// <summary>
        /// Tests whether an InvalidNameException is thrown when an invalid cell name is passed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SpreadsheetSetContentsOfCellExceptionTest3()
        {
            Spreadsheet exceptSS = new Spreadsheet();

            exceptSS.SetContentsOfCell("25", "1");
        }

        /// <summary>
        /// Tests whether an InvalidNameException is thrown when an invalid cell name is passed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SpreadsheetSetContentsOfCellExceptionTest4()
        {
            Spreadsheet exceptSS = new Spreadsheet();

            exceptSS.SetContentsOfCell("?", "1");
        }


        /// <summary>
        /// Tests whether an InvalidNameException is thrown when an invalid cell name is passed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SpreadsheetSetContentsOfCellExceptionTest5()
        {
            Spreadsheet exceptSS = new Spreadsheet();

            exceptSS.SetContentsOfCell(null, "should not work");
        }

        /// <summary>
        /// Tests whether an InvalidNameException is thrown when an invalid cell name is passed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SpreadsheetSetContentsOfCellExceptionTest6()
        {
            Spreadsheet exceptSS = new Spreadsheet();

            exceptSS.SetContentsOfCell("2x", "invalid");
        }

        /// <summary>
        /// Tests whether an InvalidNameException is thrown when an invalid cell name is passed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SpreadsheetSetContentsOfCellExceptionTest7()
        {
            Spreadsheet exceptSS = new Spreadsheet();

            exceptSS.SetContentsOfCell("25", "I'm okay");
        }

        /// <summary>
        /// Tests whether an InvalidNameException is thrown when an invalid cell name is passed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SpreadsheetSetContentsOfCellExceptionTest8()
        {
            Spreadsheet exceptSS = new Spreadsheet();

            exceptSS.SetContentsOfCell("?", "Nightwing");
        }

        /// <summary>
        /// Tests whether an InvalidNameException is thrown when an invalid cell name is passed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SpreadsheetSetContentsOfCellExceptionTest9()
        {
            Spreadsheet exceptSS = new Spreadsheet();

            exceptSS.SetContentsOfCell(null, "A1+B1");
        }

        /// <summary>
        /// Tests whether an InvalidNameException is thrown when an invalid cell name is passed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SpreadsheetSetContentsOfCellExceptionTest10()
        {
            Spreadsheet exceptSS = new Spreadsheet();

            exceptSS.SetContentsOfCell("2x", "A1+B1");
        }

        /// <summary>
        /// Tests whether an InvalidNameException is thrown when an invalid cell name is passed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SpreadsheetSetContentsOfCellExceptionTest11()
        {
            Spreadsheet exceptSS = new Spreadsheet();

            exceptSS.SetContentsOfCell("25", "A1+B1");
        }

        /// <summary>
        /// Tests whether an InvalidNameException is thrown when an invalid cell name is passed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SpreadsheetSetContentsOfCellExceptionTest12()
        {
            Spreadsheet exceptSS = new Spreadsheet();

            exceptSS.SetContentsOfCell("?", "A1+B1");
        }

        

        /// <summary>
        /// Tests for ArgumentNullException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SpreadsheetNullTest()
        {
            Spreadsheet nullSheet = new Spreadsheet();
            string nullString = null;

            nullSheet.SetContentsOfCell("A1", nullString);
        }

        /// <summary>
        /// Tests to see if a CircularException is thrown because after adding C1, it becomes acyclic.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SpreadsheetCircularException1()
        {
            Spreadsheet circle = new Spreadsheet();

            circle.SetContentsOfCell("A1", "=A1");
        }

        /// <summary>
        /// Tests to see if a CircularException is thrown because after adding C1, it becomes acyclic.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SpreadsheetCircularException2 ()
        {
            Spreadsheet circle = new Spreadsheet();

            circle.SetContentsOfCell("A1", "=B1+C1");
            circle.SetContentsOfCell("B1", "=D1+C1");
            circle.SetContentsOfCell("C1", "=A1+B1");
        }


        //===============================================================================================================================================
        //                                                                 PS5 Exception Testing
        //===============================================================================================================================================

        /// <summary>
        /// Tests whether an InvalidNameException is thrown when an invalid cell name is passed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SpreadsheetSetContentsOfCellExceptionTest13()
        {
            Spreadsheet exceptSS = new Spreadsheet();

            exceptSS.SetContentsOfCell("A1", "=_1+B1");
        }

        /// <summary>
        /// Tests whether an InvalidNameException is thrown when an invalid cell name is passed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SpreadsheetSetContentsOfCellExceptionTest14()
        {
            Spreadsheet exceptSS = new Spreadsheet();

            exceptSS.SetContentsOfCell("A", "1");
        }

        /// <summary>
        /// Tests whether an InvalidNameException is thrown when an invalid cell name is passed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SpreadsheetSetContentsOfCellExceptionTest15()
        {
            Spreadsheet exceptSS = new Spreadsheet();

            exceptSS.SetContentsOfCell("hello", "1");
        }


        /// <summary>
        /// Tests whether an InvalidNameException is thrown when an invalid cell name is passed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SpreadsheetGetCellValueExceptionTest1()
        {
            Spreadsheet exceptSS = new Spreadsheet();

            exceptSS.GetCellValue("!1");
        }

        /// <summary>
        /// Tests whether an InvalidNameException is thrown when an invalid cell name is passed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SpreadsheetGetCellValueExceptionTest2()
        {
            Spreadsheet exceptSS = new Spreadsheet();

            exceptSS.GetCellValue("A_1");
        }

        /// <summary>
        /// Tests whether an InvalidNameException is thrown when an invalid cell name is passed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SpreadsheetGetCellValueExceptionTest3()
        {
            Spreadsheet exceptSS = new Spreadsheet();

            exceptSS.GetCellValue("");
        }

        /// <summary>
        /// Tests whether an InvalidNameException is thrown when an invalid cell name is passed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SpreadsheetGetCellValueExceptionTest4()
        {
            Spreadsheet exceptSS = new Spreadsheet();

            exceptSS.GetCellValue(null);
        }

        /// <summary>
        /// Tests whether an InvalidNameException is thrown when an invalid cell name is passed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SpreadsheetGetCellValueExceptionTest5()
        {
            Spreadsheet exceptSS = new Spreadsheet();

            exceptSS.GetCellValue("A");
        }

        /// <summary>
        /// Tests to see if SpreadsheetReadWriteException will be thrown if the versions don't match
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SpreadsheetReadWriteException_Test1()
        {
            AbstractSpreadsheet except = new Spreadsheet(@"...\...\...\Spreadsheet_Good.xml", s => true, s => s, "default");
        }

        /// <summary>
        /// Tests to see if SpreadsheetReadWriteException will be thrown if there is a bad variables in the file
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SpreadsheetReadWriteException_Test2()
        {
            AbstractSpreadsheet except = new Spreadsheet(@"...\...\...\Bad_Variables.xml", s => true, s => s.ToLower(), "1.0");
        }

        /// <summary>
        /// Tests to see if SpreadsheetReadWriteException will be thrown if there is a bad formula in the file
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SpreadsheetReadWriteException_Test3()
        {
            AbstractSpreadsheet except = new Spreadsheet(@"...\...\...\Bad_Formula.xml", s => true, s => s.ToLower(), "1.0");
        }

        /// <summary>
        /// Tests to see if SpreadsheetReadWriteException will be thrown if there is a circular dependency in the file given
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SpreadsheetReadWriteException_Test4()
        {
            AbstractSpreadsheet except = new Spreadsheet(@"...\...\...\Circular_Dependency.xml", s => true, s => s.ToLower(), "1.0");

        }

        /// <summary>
        /// Tests to see if SpreadsheetReadWriteException will be thrown if there is a problem reading the file
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SpreadsheetReadWriteException_Test5()
        {
            AbstractSpreadsheet except = new Spreadsheet(@"...\...\...\XML_Fail.xml", s => true, s => s.ToLower(), "1.0");

        }

        /// <summary>
        /// Tests to see if SpreadsheetReadWriteException will be thrown if the wrong parameters are given
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SpreadsheetReadWriteException_Test6()
        {
            AbstractSpreadsheet except = new Spreadsheet(@"...\...\...\Invalid_Variable.xml", s => isValid(s), s => s.ToLower(), "1.0");
        }

        /// <summary>
        /// Tests to see if SpreadsheetReadWriteException will be thrown if the file given has no version
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SpreadsheetReadWriteException_Test()
        {
            AbstractSpreadsheet except = new Spreadsheet(@"...\...\...\No_Version.xml", s => isValid(s), s => s.ToLower(), "1.0");
        }

        /// <summary>
        /// Tests to see if GetVersion returns the correct output if given a good file
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SpreadsheetGetVersionExceptionTest1()
        {
            new Spreadsheet().GetSavedVersion(@"...\...\...\Spreadsheet_Bad.xml");
        }

        /// <summary>
        /// Tests to see if GetVersion returns the correct output if given a good file
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SpreadsheetGetVersionExceptionTest2()
        {
             new Spreadsheet().GetSavedVersion(@"...\...\...\No_Version.xml");

        }

        //===============================================================================================================================================
        //                                                                 Utility Methods
        //===============================================================================================================================================

        /// <summary>
        /// Returns either true or false if the string argument given is valid
        /// </summary>
        /// <param name="var"></param>
        /// <returns></returns>
        protected bool isValid (string var)
        {
            return validVar.Contains(var);
        }

        /// <summary>
        /// Fills the local HashSet
        /// </summary>
        protected void fillSet ()
        {
            validVar.Add("A1");
            validVar.Add("B1");
            validVar.Add("C1");
            validVar.Add("D1");
            validVar.Add("E1");
        }
    }
}
