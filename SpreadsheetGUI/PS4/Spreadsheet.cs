// Written by William Nathan Merrill
// Branch PS5 Version 1.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using SpreadsheetUtilities;

namespace SS
{

    //==================================================================================================================================================
    //                                                                         Spreadsheet Class
    //==================================================================================================================================================


    /// <summary>
    /// The External Parts of a Spreadsheet
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {

        //==========================================================================================================================================
        //                                                                Global Variables
        //==========================================================================================================================================

        /// <summary>
        /// Represents the cells that are dependent on one another
        /// </summary>
        private DependencyGraph DG;

        /// <summary>
        /// Contains the set of cells in the spreadsheet
        /// </summary>
        private Dictionary<string, Cell> cells;

        /// <summary>
        /// If the spreadsheet has been changed since its' intialization or last save, then this will be set to true. Otherwise, set to false.
        /// </summary>
        public override bool Changed { get; protected set; }


        //==========================================================================================================================================
        //                                                               Constructors
        //==========================================================================================================================================

        /// <summary>
        /// Creates an empty spreadsheet object that has the validator always set to true, the normalize function to leave everything as is and a version of "default"  
        /// <para/> For elaboration see <see cref="AbstractSpreadsheet"/>
        /// </summary>
        public Spreadsheet ()
            : this(s=>true, s=>s, "default")
        {  
        }

        /// <summary>
        /// Creates an empty spreadsheet object that sets validator to isValid, normalize function to normalize, 
        /// and the version of the spreadsheet to the passed version argument. 
        /// <para/> For elaboration see <see cref="AbstractSpreadsheet"/>
        /// </summary> 
        /// <param name="isValid"> Validator </param>
        /// <param name="normalize"> Normalizer </param>
        /// <param name="version"> Version of this Spreadsheet </param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) 
            : base(isValid, normalize, version)
        {
            this.Changed = false;
            this.cells = new Dictionary<string, Cell>();
            this.DG = new DependencyGraph();
        }

        /// <summary>
        /// Loads a spreadsheet based off the file passed through the pathname argument. 
        /// Validator is set to isValid argument, normalize is set to the normalize argument and version is set to version argument.
        /// <para/> Possible Errors: version of Spreadsheet file given does not match version parameter; names in Spreadsheet file are invalid;
        /// invalid formula's or circular dependencies are encounterd; any problems reading, writing, opening or closing file.
        /// If any the these errors are encountered, a SpreadsheetReadWriteException is thrown.
        /// <para/>For elaboration, see <see cref="AbstractSpreadsheet"/> 
        /// </summary>
        /// <param name="pathname"> Path to File </param>
        /// <param name="isValid"> Validator </param>
        /// <param name="normalize"> Normalizer </param>
        /// <param name="version"> Version of this Spreadsheet </param>
        public Spreadsheet (string pathname,Func<string, bool> isValid, Func<string, string> normalize, string version)
            : this(isValid, normalize, version)
        {
            //TODO implement rest of 4 parameter Constructor
        }

        //==========================================================================================================================================
        //                                                                 Get Cell Data
        //==========================================================================================================================================

        /// <summary>
        /// This method will return the contents of the specified cell in the argument name.
        /// 
        /// <para/> See AbstractSpreadsheet.GetCellContents for further elaboration 
        /// </summary>
        /// <param name="name"> The name of the cell to get contents from </param>
        /// <returns> returns an object based off the contents of the named cell. Can either be a String, Object or Formula</returns>
        public override object GetCellContents(string name)
        {
            // if name is null or invalid, the below method will throw an exception
            CheckName(name);

            // if th cell has been populated with content, return the cell's content. Otherwise the cell is empty, so reutrn an empty string
            if (cells.ContainsKey(name))
            {
                return cells[name].cellContent;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override object GetCellValue(string name)
        {
            CheckName(name); 

            if(cells.ContainsKey(name))
            {
                return cells[name].cellValue;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Enumerates through all of the names of cells that are currently populated with data. If no cells are populated, an empty string is returned. 
        /// <para/> See AbstractSpreadsheet.GetCellContents for further elaboration 
        /// </summary>
        /// <returns> Enumerates through all names of current populated cells</returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            if(cells.Count == 0)
            {
                yield return "";
            }
            else
            {
                foreach (string name in cells.Keys)
                {
                    yield return name;
                }
            }
            
        }


        //==========================================================================================================================================
        //                                                                  Set Contents of Cells
        //==========================================================================================================================================

        /// <summary>
        /// This method will set the contents of the named cell to the contents given.
        /// <para/> For more information, please see <see cref="AbstractSpreadsheet"/>
        /// </summary>
        /// <param name="name"> The name of the Cell. </param>
        /// <param name="content"></param>
        /// <returns></returns>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            //TODO Implement SetContentsOfCell
            // Exception Checking
            CheckName(name);
            if (content == null)
            {
                throw new ArgumentNullException();
            }

            ISet<string> rSet;
            string normalizedName = Normalize(name);
            double possibleValue = 0;

            if (Double.TryParse(content, out possibleValue))
            {
                rSet = SetCellContents(normalizedName, possibleValue);
            }
            else if (content.StartsWith("="))
            {
                string formulaString = content.Substring(1);
                Formula contentForm = new Formula(formulaString, Normalize, IsValid);
                foreach (string variable in contentForm.GetVariables())
                {
                    if (!variable.CheckVarFormat())
                    {
                        throw new ArgumentException("The variable " + variable + " in your Formula is not in the correct format.");
                    }
                }
                rSet = SetCellContents(normalizedName, contentForm);


            }
            else
            {
                rSet = SetCellContents(normalizedName, content);
            }

            // TODO
            // If rSet has any values, we must recalculate all those cells.

            //foreach (string cell in rSet)
            //{
            //    SetNewValueOfCell(cell);
            //}

            Changed = true;
            return rSet;

        }

        /// <summary>
        /// This method will set the specified contents of the cell (name) to the Formula argument passed. 
        /// <para/>
        /// If name is null or is invalid, then a InvalidNameException is thrown.
        /// If formula is null a ArgumentNullException is thrown and if Formula is invalid then a FormulaFormatException is thrown. 
        /// 
        /// Otherwise, the cell's contents will equal the contents, the value will equal the evaluated formula and a set of direct as well as indirect
        /// dependent cells will be returned alongside the name of the cell. 
        /// <para/> See AbstractSpreadsheet.GetCellContents for further elaboration 
        /// </summary>
        /// <param name="name"> name of cell to be changed </param>
        /// <param name="formula"> Formula to become the new contents of the named cell </param>
        /// <returns> An ISet of cells containing name and the cells that depend on the named cell. </returns>
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            // Keep track of the old dependcies of name in case a CircularException is thrown and we need to restablish those dependencies
            List<string> oldDependencies = new List<string>();
            oldDependencies.AddRange(DG.GetDependents(name));
            try
            {
                foreach (string variable in formula.GetVariables())
                {
                    if (!variable.CheckVarFormat()) { throw new FormulaFormatException("The variable " + variable + " passed into SetContentsOfCell is not of the correct format."); }
                }

                HashSet<string> directDependents = new HashSet<string>();
                ISet<string> allDependents = new HashSet<string>();

                directDependents.UnionWith(formula.GetVariables());

                DG.ReplaceDependents(name, directDependents); 

                // If a acyclic relationship exists, this will catch that exception and throw it. Otherwise add all the direct, and indirect, cells of name.
                foreach (string variable in GetCellsToRecalculate(directDependents))
                {
                    allDependents.Add(variable);
                }

                // If the name and text are valid, add it's content to the "name" cell
                AddNewCellContents(name, formula);

                // add the current cell to the dependent set to be returned
                allDependents.Add(name);
               
                return allDependents;
            }
            catch(CircularException)
            {
                // since the formla variables would create a acyclic relationship, we now need to restablish the dependcy relations. 
                DG.ReplaceDependents(name, oldDependencies);
                throw new CircularException();
            }
        
        }

        /// <summary>
        /// This method will set the specified contents of the cell (name) to the String text argument passed. 
        /// <para/>
        /// If name is null or is invalid, then a InvalidNameException is thrown.
        /// 
        /// Otherwise, the cell's contents will equal the contents, the value will equal text and a set of direct as well as indirect
        /// dependent cells will be returned alongside the name of the cell. 
        /// <para/> See AbstractSpreadsheet.GetCellContents for further elaboration 
        /// </summary>
        /// <param name="name"> string name of cell to be changed </param>
        /// <param name="text"> string to become the new contents of the named cell </param>
        /// <returns> An ISet of cells containing name and the cells that depend on the named cell. </returns>
        protected override ISet<string> SetCellContents(string name, string text)
        { 
            CheckDependency(name);

            // If the name and text are valid, add it's content to the "name" cell
            AddNewCellContents(name, text);

            // find the dependees, directly or indirectly, then place in rDependents and return to the user
            ISet<string> rDependents = new HashSet<string>();
            rDependents.Add(name);

            foreach (string dependent in GetCellsToRecalculate(name))
            {
                rDependents.Add(dependent);
            }

            return rDependents;
        }

        /// <summary>
        /// This method will set the specified contents of the cell (name) to the Double number argument passed. 
        /// <para/>
        /// If name is null or is invalid, then a InvalidNameException is thrown.
        /// 
        /// Otherwise, the cell's contents will equal the contents, the value will equal the Double number and a set of direct as well as indirect
        /// dependent cells will be returned alongside the name of the cell. 
        /// <para/> See AbstractSpreadsheet.GetCellContents for further elaboration 
        /// </summary>
        /// <param name="name"> String of cell to be changed </param>
        /// <param name="number"> Double to become the new contents of the named cell </param>
        /// <returns> An ISet of cells containing name and the cells that depend on the named cell. </returns>
        protected override ISet<string> SetCellContents(string name, double number)
        {
            CheckDependency(name);

            // If the name and text are valid, add it's content to the "name" cell
            AddNewCellContents(name, number);

            // find the dependees, directly or indirectly, then place in rDependents and return to the user
            ISet<string> rDependents = new HashSet<string>();
            rDependents.Add(name);

            foreach(string dependent in GetCellsToRecalculate(name))
            {
                rDependents.Add(dependent);
            }

            return rDependents;
        }

        /// <summary>
        /// This method will return the direct dependents of the named cell. 
        /// 
        /// <para/> If name is null then an ArgumentNullException is thrown. If name is invalid, then an InvalidNameException is thrown
        /// 
        /// <para/> See AbstractSpreadsheet.GetCellContents for further elaboration 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            // Exception Checking
            if (name == null)
            {
                throw new ArgumentNullException();
            }
            CheckName(name);

            HashSet<string> numberOfDependents = new HashSet<string>();

            // return all the dependents of the cell
            foreach(string dependent in DG.GetDependees(name))
            {
                if(!numberOfDependents.Contains(dependent))
                {
                    numberOfDependents.Add(dependent);
                    yield return dependent;
                }
            }
        }

        //==========================================================================================================================================
        //                                                                 Save Data in XML
        //==========================================================================================================================================

        public override string GetSavedVersion(string filename)
        {
            //TODO Implement GetSavedVersion
            throw new NotImplementedException();
        }

        public override void Save(string filename)
        {
            //TODO Implement Save
            throw new NotImplementedException();
        }

        

        

        //==================================================================================================================================================
        //                                                                        Utility Methods
        //==================================================================================================================================================

        /// <summary>
        /// Checks to see if the name is valid. If name is null or invalid, a InvalidNameException is thrown.
        /// </summary>
        /// <param name="name"> name to check </param>
        private void CheckName(string name)
        {
            if(name == null || !name.CheckVarFormat())
            {
                throw new InvalidNameException();
            }
        }

        /// <summary>
        /// This method will make the contents of the specified cell (name) equal to contents and determine the value of the cell.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="contents"></param>
        private void AddNewCellContents(string name, object contents)
        {
            if (cells.ContainsKey(name))
            {
                if(cells[name].cellContent is Formula)
                {
                    DG.ReplaceDependents(name, new HashSet<string>());
                }
                
                cells[name].cellContent = contents;
                  
            }
            else
            {
                cells.Add(name, new Cell(name, contents));
            }

            SetNewValueOfCell(name);
        }

        private void SetNewValueOfCell (string cellName)
        {
            cells[cellName].SetValueOfCell(s => lookupCellValue(s));
        }

        /// <summary>
        /// If name has any dependency, then remove them from DG
        /// </summary>
        /// <param name="name"></param>
        private void CheckDependency(string name)
        {
            if (DG.HasDependents(name))
            {
                foreach (string dependent in DG.GetDependents(name))
                {
                    DG.RemoveDependency(name, dependent);
                }
            }
        }

        /// <summary>
        /// Lookups the value of the cell. If it is a double, then return that value. Otherwise throw an ArgumentException.
        /// </summary>
        /// <param name="cell"> Cell Name </param>
        /// <returns></returns>
        private double lookupCellValue(string cell)
        {
            object cellValue = GetCellValue(cell);
            if (cellValue is Double)
            {
                return (double) cellValue;
            }
            else
            {
                throw new ArgumentException();
            }
        }
    
    }


    //==================================================================================================================================================
    //                                                                          Cell Class
    //==================================================================================================================================================



    /// <summary>
    /// Represents a single cell object containing a name and contents
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// Holds the true name of this cell
        /// </summary>
        private string _CellName;

        /// <summary>
        /// Allows user to get _CellName data
        /// </summary>
        public string cellName { get { return _CellName; } }

        /// <summary>
        /// Holds the true content of this cell
        /// </summary>
        private object _CellContent;

        /// <summary>
        /// Allows user to get or set CellContent data
        /// </summary>
        public object cellContent
        {
            get
            { return _CellContent; }
                
            set
            {
                _CellContent = value;
            }
        }

        /// <summary>
        /// Holds the true value of the cell
        /// </summary>
        private object _CellValue;
        
        /// <summary>
        /// Allows user to get CellValue data
        /// </summary>
        public object cellValue
        {
            get { return _CellValue; }
        }

        /// <summary>
        /// Creates an cell with only a name and no contents
        /// </summary>
        public Cell(string name) :
            this(name, null)
        {
        }

        /// <summary>
        /// Creates a Cell object with a name and contents that will define the final value of the cell
        /// </summary>
        /// <param name="name"> The name given to the specific Cell </param>
        /// <param name="content"> The contents of the cell </param>
        public Cell(string name, object content)
        {
            this._CellName = name;
            this.cellContent = content;
        }

        /// <summary>
        /// Sets the value of the cell if changed
        /// </summary>
        /// <param name="content"></param>
        /// <param name="lookup"></param>
        /// </summary>
        public void SetValueOfCell (Func<string, double> lookup)
        {
            if (_CellContent is Formula)
            {
                Formula valueOfContent = (Formula)_CellContent;
                this._CellValue = valueOfContent.Evaluate(lookup);
            }
            else
            {
                this._CellValue = _CellContent;
            }
        }


    }



    //==================================================================================================================================================
    //                                                                         Extension Class
    //==================================================================================================================================================




    /// <summary>
    /// A set of useful extensions for strings
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Determines if the string argument has the correct formatting. (i.e. v starts with either an underscore or letter
        /// and is then followed by 0 or more letters, underscores or numeric values
        /// </summary>
        /// <param name="varToCheck"> Variable to check for format </param>
        /// <returns> true if varToCheck has correct formatting. Return false otherwise. </returns>
        public static bool CheckVarFormat(this string varToCheck)
        {
            Regex variableFormat = new Regex("(^([A-Za-z]+[A-Za-z0-9]*)$)");
            return variableFormat.IsMatch(varToCheck);
        }
    }
}
