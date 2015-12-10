using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;


namespace GUITestProject
{
    /// <summary>
    /// Summary description for CodedUITest1
    /// </summary>
    [CodedUITest]
    public class CodedUITest1
    {
        public CodedUITest1()
        {
        }

        /// <summary>
        /// Tests the functionality of inputing an incorrect formula, leading to a formula error, and then fixing the referenced cell which leads to the formul compiling without user input.
        /// </summary>
        [TestMethod]
        public void CodedUITestMethod1()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            this.UIMap.FormulaErrorTest1();
            this.UIMap.AssertValueMethod1();
            this.UIMap.ContentBoxUpdateTest1();
            this.UIMap.UpdateLocationRecord();
            this.UIMap.AssertMethod2();
            this.UIMap.EndSpreadsheet();

        }

        /// <summary>
        /// Tests functionality of inputing a valid formula.
        /// </summary>
        [TestMethod]
        public void CodedUITestMethod2()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            this.UIMap.RecordFormulaInput1();
            this.UIMap.AssertContents();

        }

        /// <summary>
        /// Tests the functionality of saving a file
        /// </summary>
        [TestMethod]
        public void CodedUITestMethod3()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            this.UIMap.CreateNewSpreadsheet();
            this.UIMap.NewSSContentsEmpty();
            this.UIMap.EnterValuesIntoSpreadsheetRecord();
            this.UIMap.OldSpreadsheetSame();
            this.UIMap.CloseNewSpreadsheetRecord();
            this.UIMap.CloseOldSpreadsheetRecord();
        }

        /// <summary>
        /// Tests the functionality of opening a file.
        /// </summary>
        [TestMethod]
        public void CodedUITestMethod4()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            this.UIMap.OpenNewFileAndSaveRecord();
            this.UIMap.OldSpreadsheetSame2();
            this.UIMap.OpenASavedFile();
            this.UIMap.OpenedValueAssertion();

        }

        /// <summary>
        /// Tests the functionality of the saving prompt if the user decided to close the app when there are unsaved changes. 
        /// </summary>
        [TestMethod]
        public void CodedUITestMethod5()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            this.UIMap.SaveOnClosePrompt();
            this.UIMap.SpreadsheetRemainsOpenAfterCancel();
            this.UIMap.CloseWithUnsavedChanges();
        }

        /// <summary>
        /// Tests functionality of using Go To Cell and being taken to correct cell.
        /// </summary>
        [TestMethod]
        public void CodedUITestMethod6()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            this.UIMap.OpenSpreadsheetShortcut();
            this.UIMap.PlaceValueInCell();
            this.UIMap.GoToCellRecord();
            this.UIMap.CellNameIsUpdatedAssert();
            this.UIMap.CloseSpreadsheet();
        }


        /// <summary>
        /// Tests functionality of using Arrow Keys to navigate through spreadsheet.
        /// </summary>
        [TestMethod]
        public void CodedUITestMethod07()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            this.UIMap.ArrowKeyNavigateRecord();
            this.UIMap.MoveBackToStartArrowKeys();
            this.UIMap.MoveLeftFromStartArrowKeys();
            this.UIMap.CheckCellNameFromMovingLeftWithArrowKeys();
            this.UIMap.MoveRightToStartWithArrowKeys();
            this.UIMap.ValueStayedTheSameAfterArrowKeys();
            this.UIMap.CellNameAtStartIsCorrectArrowKeys();
            this.UIMap.CellNameIsUpdatedArrowKeys();
            this.UIMap.CloseSpreadsheetArrowKeys();
        }

        /// <summary>
        /// Tests functionality of using Go To Cell and being taken to correct cell.
        /// </summary>
        [TestMethod]
        public void CodedUITestMethod8()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            this.UIMap.ResizeWindow();
            this.UIMap.ValueStayedTheSameAfterResize();
            this.UIMap.CloseSpreadsheetResize();
        }

        /// <summary>
        /// Tests functionality of using Go To Cell and being taken to correct cell.
        /// </summary>
        [TestMethod]
        public void CodedUITestMethod9()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            this.UIMap.OpenSpreadsheetHelpMenu();
            this.UIMap.HelpMenuEditCellRecord();
            this.UIMap.HelpMenuNavigateRecord();
            this.UIMap.HelpMenuFileMenuRecord();
            this.UIMap.HelpMenuShortcutsRecord();
        }

        /// <summary>
        /// Tests functionality of using Go To Cell and being taken to correct cell.
        /// </summary>
        [TestMethod]
        public void CodedUITestMethod10()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            this.UIMap.OpenSpreadsheetShortcutsTest();
            this.UIMap.CreateNewSpreadsheetShortcut();
            this.UIMap.GoToA1();
            this.UIMap.NewSpreadsheetShortcutAssert();
            this.UIMap.CloseSpreadsheetShortcut();

        }

        /// <summary>
        /// Tests functionality of using Go To Cell and being taken to correct cell.
        /// </summary>
        [TestMethod]
        public void CodedUITestMethod11()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            this.UIMap.OpenSpreadsheetShortcutsTest2();
            this.UIMap.OpenASavedFileShortcut();
            this.UIMap.OpenFileShortcutValueAssert();
            this.UIMap.MoveToA3();
            this.UIMap.CellValueAfterOpening();
            this.UIMap.CloseNewlyOpenedSpreadsheet();

        }


        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:

        ////Use TestInitialize to run code before running each test 
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        ////Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        #endregion

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        private TestContext testContextInstance;

        public UIMap UIMap
        {
            get
            {
                if ((this.map == null))
                {
                    this.map = new UIMap();
                }

                return this.map;
            }
        }

        private UIMap map;
    }
}
