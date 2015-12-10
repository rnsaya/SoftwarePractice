// Form.cs written by Nate Merrill and Rachel Saya for CS 3500, November 2015

// Revision history:  
//   Version 1.1 10/26/15 9:51 a.m.   Adjusted size property to allow growing and shrinking of GUI
//   Version 1.2 10/28/15 4:15 p.m.   Added skeleton for multithreading
//   Version 1.3 10/29/15 2:34 p.m.   Enter key implemented to update contents
//   Version 1.4 11/01/15 1:47 p.m.   New/Open/Save capabilities added.
//   Version 1.5 11/02/15 11:12 a.m.  Added SpreadsheetPanel to PS6
//   Version 1.6 11/04/15 12:05 p.m.  Cells are highlighted when they are updated 
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using SS;
using SpreadsheetUtilities;

namespace SpreadsheetGUI
{
    /// <summary>
    /// The GUI object representation for a spreadsheet. 
    /// </summary>
    public partial class SpreadsheetForm : Form
    {
        //==============================================================================================================================================================================
        //                                                                             Global Variables
        //==============================================================================================================================================================================

        /// <summary>
        /// Represents the spreadsheet in the GUI
        /// </summary>
        private Spreadsheet SS;

        private string recentSavePathname;

        // ==============================================================================================================================================================================
        //                                                                               Display Code
        // ==============================================================================================================================================================================

        /// <summary>
        /// Initializes the GUI
        /// </summary>
        public SpreadsheetForm()
        {
            // Initalize a new spreadsheet with a Regex Validator and s.ToUpper() normalizer. 
            SS = new Spreadsheet(s => s.CheckVarFormat(), s => s.ToUpper(), "default");

            InitializeComponent();

            spreadsheetPanel.SelectionChanged += displaySelection;
            spreadsheetPanel.SetSelection(0, 0);
            spreadsheetPanel.Focus();

            recentSavePathname = "";
        }

        /// <summary>
        /// Will allow user to select a cell and replace it's contents which in turn will replace the cell's value.
        /// The location of the selected will then be displayed.
        /// </summary>
        /// <param name="sp"> SpreadsheetPanel to be changed by user. </param>
        private void displaySelection(SS.SpreadsheetPanel sp)
        {

            String value;
            String locationOfCell;

            // Finds the location of the cell and converts it to a string.
            // For example, if the columm and row added together is A1, 
            // this will create a string reprsentation of that column and row. 
            locationOfCell = getLocationOfSelectedCell(sp);

            // get the selected cell's value
            value = SS.GetCellValue(locationOfCell).ToString();

            // get the selected cell's contents
            String content = getContentsOfCell(locationOfCell);
            cellValueLabel.Text = value;

            // set the text in the UI information bar to correct values and contents
            contentTextBox.Text = content;
            contentTextBox.Modified = false;
            cellValueLabel.Text = value;
            currentCellLabel.Text = locationOfCell;
            spreadsheetPanel.Focus();
        }

        // ==============================================================================================================================================================================
        //                                                                              Event Code
        // ==============================================================================================================================================================================

        /// <summary>
        /// If the user changes the contents in the text box, 
        /// this method broadcasts that the text box has been modified.
        /// </summary>
        /// <param name="sender">  </param>
        /// <param name="e">  </param>
        private void contentTextBox_TextChanged(object sender, EventArgs e)
        {
            contentTextBox.Modified = true;

        }

        /// <summary>
        /// When the enter key is pressed the GUI selects the cell that is in the textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goToCellTextBox_EnterPressed(object sender, KeyEventArgs e)
        {
            //Make sure the key that was pressed was "Enter".
            if (e.KeyCode == Keys.Return)
            {
                int col, row;

                
                String location = goToCellTextBox.Text.ToUpper();
                if(location.CheckVarFormat())
                {
                    col = location[0] - 65;

                    // This if statement will check to see if the location given falls within the cell range.
                    // If true, go to that location on the spreadsheetPanel. Else, return the message and do nothing. 
                    if (Int32.TryParse(location.Substring(1), out row) && row <= 99)
                    {
                        row--;

                        spreadsheetPanel.SetSelection(col, row);
                        currentCellLabel.Text = location;
                    }
                    else
                    {
                        MessageBox.Show("That cell is out of the scope of the spreadsheet");
                    }
                    
                }
                else
                {
                    MessageBox.Show("That is not a valid variable cell name.");
                }
                //row = Int32.Parse(location.Substring(1)) - 1;

                goToCellTextBox.Text = "";

                // Stops the bing that Windows makes when you press a key   
                e.SuppressKeyPress = true;

                spreadsheetPanel.Focus();
            }

        }

        /// <summary>
        /// When the enter key is pressed the GUI content is updated to what is in the contentBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="KeyEventArgs"></param>
        /// <param name=""></param>
        private void contentTextBox_EnterPressed(object sender, KeyEventArgs e)
        {
            //Make sure the key that was pressed was "Enter".
            if (e.KeyCode == Keys.Return)
            {
                // Uses a background worker to update the GUI
                ssInnards.RunWorkerAsync(getLocationOfSelectedCell(spreadsheetPanel));

                // Stops the bing that Windows makes when you press a key   
                e.SuppressKeyPress = true;

                spreadsheetPanel.Focus();
            }
        }

        /// <summary>
        /// If the user presses the arrow keys, the position of the cell will change accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpreadsheetPanel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            int col, row;
            spreadsheetPanel.GetSelection(out col,out row);
            switch (e.KeyCode)
            {
                case Keys.Up:
                    spreadsheetPanel.SetSelection(col, row - 1);
                    break;
                case Keys.Down: 
                    spreadsheetPanel.SetSelection(col, row + 1);
                    break;
                case Keys.Right:
                    spreadsheetPanel.SetSelection(col + 1, row);
                    break;
                case Keys.Left:
                    spreadsheetPanel.SetSelection(col - 1, row);
                    break;
            }

            if (e.Control)
            {
                switch(e.KeyCode)
                {
                    case Keys.S:
                        checkToSave();
                        break;
                    case Keys.N:
                        new SpreadsheetForm().Visible = true;
                        break;
                    case Keys.O:
                        openAFile();
                        break;
                }
                
            }
    
            displaySelection(spreadsheetPanel);
        }

        /// <summary>
        /// If the user presses ctrl and s at the same time, it will prompt the user to save if there have been recent changes. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contentTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.S:
                        checkToSave();
                        break;
                    case Keys.N:
                        new SpreadsheetForm().Visible = true;
                        break;
                    case Keys.O:
                        openAFile();
                        break;
                }
            }
        }

        /// <summary>
        /// If the user clicks the exit button on Form and there are changes that haven't been saved, it will prompt the user to save the file before closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpreadsheetForm_FormClosing(object sender, FormClosingEventArgs e)
        {
           if(checkToSave() == false)
            {
                e.Cancel = true;
            }
            
        }      

        // ==============================================================================================================================================================================
        //                                                                          Background Threading Code
        // ==============================================================================================================================================================================

        /// <summary>
        /// When this thread is called, then it will determine if it needs to get the contents of a cell or change the spreadsheet.
        /// Once the event determines what to do, it will return from the thread and update the GUI.
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void ssInnards_DoWork(object sender, DoWorkEventArgs e)
        {
            //MessageBox.Show("entered thread");
            String cellLocation = (String)e.Argument;
            Dictionary<string, string> cellsToChange;

            // locks the SS object to one thread at a time.
            lock (SS)
            {
                ISet<string> changedCells;
                try
                {
                    changedCells = SS.SetContentsOfCell(cellLocation, contentTextBox.Text);
                }
                catch (CircularException)
                {
                    MessageBox.Show("The Formula given contains an acyclic relationship, thus being invalid.");
                    changedCells = new HashSet<string>();
                }

                // Creates a dictionary of all cells that need to be updated
                cellsToChange = new Dictionary<string, string>();

                if (SS.GetCellValue(cellLocation) is FormulaError)
                {
                    MessageBox.Show("You gave an Invalid Formula");
                }

                foreach (string cell in changedCells)
                {
                    cellsToChange.Add(cell, SS.GetCellValue(cell).ToString());
                }

            }

            e.Result = cellsToChange;
        }

        /// <summary>
        /// This method will get the contents for cell argument and then return it as a string.
        /// </summary>
        /// <param name="cell"> cell to look get contents </param>
        /// <returns> The contents of the cell as a string </returns>
        private string getContentsOfCell(String cell)
        {
            if (SS.GetCellContents(cell) is Formula)
            {
                return "=" + SS.GetCellContents(cell).ToString();
            }
            else
            {
                return SS.GetCellContents(cell).ToString();
            }
        }

        /// <summary>
        /// When DoWork thread is done, this method will redraw the GUI with the updated cell values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ssInnards_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int col, row;
            string value;
            if (e.Error != null)
                MessageBox.Show("Error!");
            else
            {
                Dictionary<string, string> cellsToUpdate = (Dictionary<string, string>)e.Result;
                spreadsheetPanel.SetUpdatedCells(cellsToUpdate);
                
                foreach (string cell in cellsToUpdate.Keys)
                {
                    // Gets the cell location and updates it to the new value
                    col = cell[0] - 65;
                    row = Int32.Parse(cell.Substring(1)) - 1;

                    spreadsheetPanel.SetValue(col, row, cellsToUpdate[cell]);
                }
            }

            spreadsheetPanel.GetSelection(out col, out row);
            spreadsheetPanel.GetValue(col, row, out value);
            MethodInvoker set_label = new MethodInvoker(() => cellValueLabel.Text = value);
            this.Invoke(set_label);

        }

        // ==============================================================================================================================================================================
        //                                                                             File Menu Methods
        // ==============================================================================================================================================================================

        /// <summary>
        /// When the newFileButton is clicked, this event will fire and create a new SpreadsheetForm and then make it visible to the user.
        /// </summary>
        private void newFileButton_Click(object sender, EventArgs e)
        {
            DemoApplicationContext.getAppContext().RunForm(new SpreadsheetForm());
        }

        /// <summary>
        /// When the closeFleButton is clicked, this event will fire and close the current SpreadsheetForm window.
        /// </summary>
        private void closeFileButton_Click(object sender, EventArgs e)
        {
            if(checkToSave())
            {
                this.Close();
            }
        }

        /// <summary>
        /// When this button is clicked, the user will be prompted to look for a file, and then open it through our spreadsheet app.
        /// Invariant: the file MUST be have the .sprd extension
        /// </summary>
        private void openFileButton_Click(object sender, EventArgs e)
        {
            openAFile();
        }

        /// <summary>
        ///  The user will be prompted to look for a file, and then open it through our spreadsheet app.
        /// </summary>
        private void openAFile()
        {
            if (checkToSave())
            {
                try
                {
                    // opens a dialog for the user to select a file
                    OpenFileDialog openFile = new OpenFileDialog();

                    // Set filter options, filter index, title, and starting directory.               
                    openFile.Filter = "sprd Files (.sprd) |*.sprd| xml Files (.xml)|*.xml| xml & sprd files (.xml), (.sprd)|*.xml;*.sprd| All Files (*.*)|*.*";
                    openFile.FilterIndex = 1;

                    openFile.InitialDirectory = @"C:\";
                    openFile.Title = "Please select a file to open as a spreadsheet";

                    // when user selects a file, go into if statement.
                    if (openFile.ShowDialog() == DialogResult.OK)
                    {
                        // determines the pathname for the file
                        string path = openFile.FileName;
                        string version = "";

                        // if the file is represented in XML style, this method will pass it into this reader and retrieve the version of the file. 
                        // if the file is not of XML file, it is not able to be read and should throw an exception. 
                        using (XmlReader reader = XmlReader.Create(path))
                        {
                            reader.ReadToFollowing("spreadsheet");
                            version = reader.GetAttribute(0);
                        }

                        // set Spreadsheet global (SS) to the .sprd file.
                        SS = new Spreadsheet(path, s => s.CheckVarFormat(), s => s.ToUpper(), version);

                        int col, row;
                        foreach (string cell in SS.GetNamesOfAllNonemptyCells())
                        {
                            col = cell[0] - 65;
                            row = Int32.Parse(cell.Substring(1)) - 1;
                            spreadsheetPanel.SetValue(col, row, SS.GetCellValue(cell).ToString());
                        }

                        this.Text = openFile.SafeFileName.Remove(openFile.SafeFileName.Length - 5);

                        this.spreadsheetPanel.SetSelection(0, 0);
                        displaySelection(this.spreadsheetPanel);

                        recentSavePathname = path;

                    }
                }
                // TODO what exception catches should be put in place?
                catch (Exception)
                {
                    MessageBox.Show("there was an exception thrown while trying to open the file");
                }

            }
        }

        /// <summary>
        /// When this button is clicked, the user will be prompted to save the current window as a .sprd file. 
        /// </summary>
        private void saveFileButton_Click(object sender, EventArgs e)
        {
            if (recentSavePathname == "")
            {
                saveFile();
            }
            else
            {
                string path = recentSavePathname;
                SS.Save(path);
                MessageBox.Show("File Successfully Saved");
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        /// <summary>
        /// This method will save the current spreadsheet to a file chosen by the user.
        /// </summary>
        private void saveFile()
        {

            try
            {
                SaveFileDialog saveFile = new SaveFileDialog();

                // Set filter options, filter index, title, default extension and starting directory.               
                saveFile.Filter = "sprd Files (.sprd) |*.sprd| xml Files (.xml)|*.xml| xml & sprd files (.xml), (.sprd)|*.xml;*.sprd| All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;

                saveFile.InitialDirectory = @"C:\";
                saveFile.Title = "Please select a file to open as a spreadsheet";

                saveFile.DefaultExt = ".sprd";
                saveFile.AddExtension = true;
                saveFile.FileName = "Spreadsheet";

                saveFile.OverwritePrompt = true;

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    string path = saveFile.FileName;

                    SS.Save(path);

                    recentSavePathname = path;

                    MessageBox.Show("File Successfully Saved");
                }

                

            }
            catch (Exception)
            {
                MessageBox.Show("there was an exception thrown while trying to save the file.");
            }
        }

        // ==============================================================================================================================================================================
        //                                                                             Help Menu Methods
        // ==============================================================================================================================================================================


        /// <summary>
        /// Displays a Message Box to User to help them through setting contents. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("\t\t\t Change Contents\n\n1. To change the contents, first select a cell to edit, then click on the text box next to 'contents'." 
                                + "\n\n2. Now enter either a string, double or formula and press enter. "
                                + "\n\n3. If you give a valid input to the contents box (string, double or valid formula), the contents will change."
                                + "\n\n4. After the contents are accepted by the spreadsheet, the cell value will be updated and placed in the selected cell."
                                + "\n\n5. If a change in a cell would effect a pre-existing formula, all the cells affected by that change will be highlighted for the user.", "Help: Contents");
        }

        /// <summary>
        /// Displays a Message Box to User to help them through setting value. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void valueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("\t\t\t Change Value\n\n1. User's can not directly change the value. To update value, please refer to contents."
                               + "\n\n2. If you do give a valid input to the contents box (string, double or valid formula), the value will also change to reflect the contents."
                               + "If a formula is entered, and has invalid variables, a formula error will be placed in value. "
                               + "However, if an invalid input is given an error message will show and nothing will change."
                               + "\n\n4. After the contents are accepted by the spreadsheet, the cell value will be updated and placed in the selected cell.", "Help: Value");
        }

        /// <summary>
        /// Displays a Message Box to User to help them through setting Cell Name. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cellNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("\t\t\t Change Cell Name\n\n1. User's can not directly change the cell name. To update cell name you must navigate to a cell, please refer to contents."
                              + "\n\n2. Once a cell is selected, the location will be updated in the Cell Name label", "Help: Cell Name");
        }

        /// <summary>
        /// Displays a Message Box to User to help them navigate through spreadhsheet using Mouse Click. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mouseClickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("\t         Navigate using Mouse Click \n\n1. Users can select cells by clicking on directly on the cell", "Help: Mouse Click");
        }

        /// <summary>
        /// Displays a Message Box to User to help them navigate through spreadhsheet using Scroll Bar. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("\t           Navigate using 'Go To Cell' \n\n1. Users can navigate directly to a specifed cell using the 'Go To Cell' text box.", "Help: Go To Cell");
        }

        /// <summary>
        /// Displays a Message Box to User to help them navigate through spreadhsheet using Arrow Keys. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void arrowKeysToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("\t\t      Navigate using Arrow Keys \n\n1. Users can select cells by pressing the arrow keys to navigate between cells."
                            + "\nAs the user navigates, the window will follow the position of the selected cell; however, the scroll bar will remain stationary."
                                + "\n\n 2. The user must select a cell before the arrow keys will allow navigation.", "Help: Arrow Keys");
        }

        /// <summary>
        /// Displays a Message Box to User to help them navigate through creating a new Spreadsheet window. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newSpreadsheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("\t\t\t New Spreadsheet: \n\n1. To open a new spreadsheet window, first go to File menu bar."
                               + "\n\n2. Next, click New. "
                               + "\n\n3. A new spreadsheet window will open that is seperate from the current spreadsheet window."
                               + "\n\nNOTE: Changes in one spreadsheet will not affect another spreadsheet that is open.", "Help: New Spreadsheet");
        }

        /// <summary>
        /// Displays a Message Box to User to help them navigate through opening a spreadsheet file. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("\t\t\t Open File: "
                               + "\n\n1. To open a pre-existing file, first go to File menu bar."
                               + "\n\n2. Next click Open. "
                               + "\n\n3. If there are changes that would be overwritten as a result of opening a file, then the user will be prompted to either save your changes, discard them or cancel the open file operation."
                               + "\n\n4. Once the File Dialog opens, search for the file to open, select it and then click Open."
                               + "\n\nNOTE: If you open a pre-existing file, the current spreadsheet will be replaced with the contents of the file chosen", "Help: Open File");

        }

        /// <summary>
        /// Displays a Message Box to User to help them navigate through saving a spreadsheet window. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("\t\t\t Save File: "
                              + "\n\n1. To save a file that has been recently changed, first go to File menu bar."
                              + "\n\n2. Next click Save to save automatically if using a previously saved file. Click Save As to directly save to computer."
                              + "\n\n3. If the file does not already have a location on the computer, then a file dialog will appear allowing users to locate a" 
                              + "sufficent file location where a user would wish to save the file, select it and then click Save. Otherwise the file will be placed in the pre-existing file pathname."
                              , "Help: Save File");
        }

        /// <summary>
        /// Displays a Message Box to User to help them navigate through closing a Spreadsheet window. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeSpreadsheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("\t\t\t Close Spreadsheet: "
                              + "\n\n1. To close the current spreadsheet the user may do one of two things: "
                              + "\n\ta. First go to File menu bar. Next, click Close "
                              + "\n\n\t\t\tOR"
                              + "\n\n\tb. Click on the big red X in the upper right-hand corner. "
                              + "\n\n2. In either case, if there are changes that would be overwritten by closing the spreadsheet, then the user will be prompted to either save changes, discard them or cancel the closing of the spreadsheet."
                              , "Help: Close Spreadsheet");
        }


        // ==============================================================================================================================================================================
        //                                                                              Utility Methods
        // ==============================================================================================================================================================================

        /// <summary>
        /// Finds the location of the cell and then returns the string representation of that cell's location on the spreadsheetPanel argument passed. 
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        private string getLocationOfSelectedCell(SS.SpreadsheetPanel sp)
        {
            int col, row;
            sp.GetSelection(out col, out row);

            return (((char)('A' + col)).ToString()) + (row + 1);
        }

       

        /// <summary>
        /// This method will check to see if the spreadsheet has changed. 
        /// If it has been changed, the user will be prompted to save the current spreadsheet by a Message Box that has a Yes, No or Cancel button set.
        /// If yes is choosen, the user will be prompted to save through the saveFile method.
        /// If Cancel is choosen, the closing procedure will be cancelled and the current spreadsheet will remain open.
        /// If No is choosen, the closing procedure will proceed and the current spreadsheet file will be unsaved (lost).
        /// </summary>
        private bool checkToSave()
        {
            if (SS.Changed == true)
            {
                DialogResult dialog = MessageBox.Show("Would you like to save any new changes?", "Spreadsheet", MessageBoxButtons.YesNoCancel);

                if (dialog == DialogResult.Yes)
                {
                    saveFile();
                }
                else if (dialog == DialogResult.Cancel)
                {
                    return false;
                }
            }
            return true;

        }

        
    }// end of the SpreadsheetForm Class

}// end of the SpreadshetGUI Namespace
