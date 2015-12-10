// Written by Rachel Saya for CS 3500, September 2015
//
// Revision history:  
//   Version 1.1 10/15/15 10:08 a.m.  Added setContentsOfCell method and updated contentsOfCell methods
//   Version 1.2 10/15/15 10:42 a.m.  Added and updated all three constructors and added XMLread helper file.
//   Version 1.3 10/15/15 11:18 a.m.  Completed GetCellValue and GetSavedVersion 
//   Version 1.4 10/15/15 2:08  p.m.  Implemented Save method
//   Version 1.5 10/16/15 7:36  a.m.  Updated formula and spreadsheet utilities references
//   Version 1.6 10/16/15 9:49  a.m.  Added to private cell class


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpreadsheetUtilities;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;

namespace SS
{
    ///<summary>
    ///A spreadsheet consists of an infinite number of named cells.
    /// 
    /// A string is a cell name if and only if it consists of one or more letters,
    /// followed by one or more digits AND it satisfies the predicate IsValid.
    /// For example, "A15", "a15", "XY032", and "BC7" are cell names so long as they
    /// satisfy IsValid.  On the other hand, "Z", "X_", and "hello" are not cell names,
    /// regardless of IsValid.
    /// 
    /// Any valid incoming cell name, whether passed as a parameter or embedded in a formula,
    /// must be normalized with the Normalize method before it is used by or saved in 
    /// this spreadsheet.  For example, if Normalize is s => s.ToUpper(), then
    /// the Formula "x3+a5" should be converted to "X3+A5" before use.
    /// 
    /// A spreadsheet contains a cell corresponding to every possible cell name.  (This
    /// means that a spreadsheet contains an infinite number of cells.)  In addition to 
    /// a name, each cell has a contents and a value.  The distinction is important.
    /// 
    /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
    /// contents is an empty string, we say that the cell is empty.  (By analogy, the contents
    /// of a cell in Excel is what is displayed on the editing line when the cell is selected.)
    /// 
    /// In a new spreadsheet, the contents of every cell is the empty string.
    ///  
    /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
    /// (By analogy, the value of an Excel cell is what is displayed in that cell's position
    /// in the grid.)
    /// If a cell's contents is a string, its value is that string.
    /// 
    /// If a cell's contents is a double, its value is that double.
    /// 
    /// If a cell's contents is a Formula, its value is either a double or a FormulaError,
    /// as reported by the Evaluate method of the Formula class.  The value of a Formula,
    /// of course, can depend on the values of variables.  The value of a variable is the 
    /// value of the spreadsheet cell it names (if that cell's value is a double) or 
    /// is undefined (otherwise).
    /// 
    /// Spreadsheets are never allowed to contain a combination of Formulas that establish
    /// a circular dependency.  A circular dependency exists when a cell depends on itself.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        /// <summary>
        /// Cells contains cell names and contents
        /// </summary>
        private Dictionary<string, Cell> spreadsheet;

        /// <summary>
        /// DependencyGraph contains all data for the spreadsheet.
        /// </summary>
        private DependencyGraph dependencyGraph;

        /// <summary>
        /// Boolean that keeps track to see if a change has occured
        /// </summary>
        private bool changed;

        /// <summary>
        /// Zero-argument constructor creates a new empty spreadsheet
        /// Imposes no extra validity conditions, normalizes every cell 
        /// name to itself, and has version "default".
        /// </summary>        
        public Spreadsheet() : base(s => true, s => s, "default") {
            spreadsheet = new Dictionary<string, Cell>();
            dependencyGraph = new DependencyGraph();

            Changed = false;
        }

        /// <summary>
        /// Constructs an abstract spreadsheet by recording its variable validity test,
        /// its normalization method, and its version information.  The variable validity
        /// test is used throughout to determine whether a string that consists of one or
        /// more letters followed by one or more digits is a valid cell name.  The variable
        /// equality test should be used thoughout to determine whether two variables are
        /// equal.
        /// </summary>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version)
            : base(isValid, normalize, version) {
            spreadsheet = new Dictionary<string, Cell>();
            dependencyGraph = new DependencyGraph();

            Changed = false;
        }

        /// <summary>
        /// allow the user to provide a string representing a path to
        /// a file, a validity delegate, a normalization delegate,
        /// and a version. It reads a saved spreadsheet from a file
        /// and uses it to construct a new spreadsheet. The new spreadsheet
        /// uses the provided validity delegate, normalization delegate, and version.
        /// </summary>
        public Spreadsheet(string filepath, Func<string, bool> isValid, Func<string, string> normalize, string version)
            : base(isValid, normalize, version) {
            spreadsheet = new Dictionary<string, Cell>();
            dependencyGraph = new DependencyGraph();

            if (!(GetSavedVersion(filepath).Equals(version)))
                throw new SpreadsheetReadWriteException("The version of the file cannot be found");

            XMLFile(filepath, false);                                                        //Uses helper method to read in XML file

            Changed = false;
        }

        /// <summary>
        /// True if it has been modified or changed, false otherwise.
        /// </summary>
        public override bool Changed {
            get           {
                return changed;
            }
            protected set {
                changed = value;
            }
        }

        /// <summary>
        /// Helper method to read in a spreadsheet from an xml file
        /// </summary>
        private string XMLFile(string filename, bool version) {
            if (ReferenceEquals(filename, null))                                            //Checks for null filename                                    
                throw new SpreadsheetReadWriteException("Filename is null");
            if (filename.Equals(""))                                                        //Checks for empty filename
                throw new SpreadsheetReadWriteException("Filename is Empty");
            try            {
                using (XmlReader reader = XmlReader.Create(filename)) {
                    string contents = "";
                    string cell = "";

                    while (reader.Read()) {                                                 //While reader has more elements
                        if (reader.IsStartElement())  {
                            bool set = false;

                            if (reader.Name.Equals("spreadsheet")) {                        //If it is a spreadsheet
                                if (version)
                                    return reader["version"];
                                else
                                    Version = reader["version"];
                            }
                            if (version)                                                    //Version should not have already been returned
                                throw new SpreadsheetReadWriteException("Error: has already been returned");

                            else if (reader.Name.Equals("contents")) {                      //If it is contents
                                reader.Read();
                                contents = reader.Value;
                                set = true;
                            }
                            else if (reader.Name.Equals("name")) {                          //If it is name                       
                                reader.Read();
                                cell = reader.Value;
                            }
                            if (set)
                                SetContentsOfCell(cell, contents);
                        }
                    }
                }
            }
            catch (XmlException e)  {
                throw new SpreadsheetReadWriteException(e.ToString());
            }
            catch (IOException e)  {
                throw new SpreadsheetReadWriteException(e.ToString());
            }
            return Version;
        }

        /// <summary>
        /// Returns all nonempty cells aka the keys
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return spreadsheet.Keys;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// A string is a valid cell name if and only if:
        ///   (1) its first character is an underscore or a letter
        ///   (2) its remaining characters (if any) are underscores and/or letters and/or digits
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            if (String.Compare(name, null) == 0 || !Regex.IsMatch(name, "^[_a-zA-Z][a-zA-Z0-9_]*$")) {
                throw new InvalidNameException();                                                    //If invalid or if name is null throw exception
            }
            else if (!spreadsheet.ContainsKey(Normalize(name)))                                      //If name is not in spreadsheet return empty
                return string.Empty;
            else
                return spreadsheet[Normalize(name)].getContents();                                   //Return contents of name

        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.  (No change is made to the spreadsheet.)
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, double number) {
            if (spreadsheet.ContainsKey(name))
                updateDependency(name);
            try {
                spreadsheet.Add(name, new Cell(number));                                            //Try to add name and cell with number
            }
            catch (ArgumentException) {
                spreadsheet.Remove(name);
                spreadsheet.Add(name, new Cell(number));
            }
            if (dependencyGraph.HasDependees(name)) {                                               //If it has dependents update and recalculate them
                HashSet<String> ans = new HashSet<string>();
                foreach (String str in GetCellsToRecalculate(name))
                    ans.Add(str);
                return ans;
            }
            return new HashSet<String> { name };
        }

        /// <summary>
        /// Helper method that updates dependencys for formulas
        /// </summary>
        /// <param name="name">Name of the cell to update</param>
        private void updateDependency(String name)  {
            try   {                                                                                 //Updates dependencies
                Cell cell;
                spreadsheet.TryGetValue(name, out cell);
                Formula formula = (Formula)cell.getContents();
                foreach (String str in formula.GetVariables())
                    dependencyGraph.RemoveDependency(name, str);
            }
            catch (InvalidCastException) { }                                                        //Does nothing if it is not a valid formula
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.  (No change is made to the spreadsheet.)
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, string text) {

            if (spreadsheet.ContainsKey(name))
                updateDependency(name);
            if (!spreadsheet.ContainsKey(name)) {
                if (text != "")                                                                  //If spreadsheet does not contain cell and text is not empty, add to spreadsheet
                    spreadsheet.Add(name, new Cell(text));
            }
            else  {
                spreadsheet.Remove(name);                                                       //Updates contents of cell
                if (text != "")
                    spreadsheet.Add(name, new Cell(text));
            }
            if (dependencyGraph.HasDependees(name)) {                                           //If it has dependees than recalculate
                HashSet<String> ans = new HashSet<string>();
                foreach (String str in GetCellsToRecalculate(name))
                    ans.Add(str);
                return ans;
            }
            return new HashSet<String> { name };
        }

        /// <summary>
        /// If changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.  (No change is made to the spreadsheet.)
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, Formula formula) {
            if (spreadsheet.ContainsKey(name)) {                                                //If it is contained
                Cell cell;
                IEnumerable<String> old;
                spreadsheet.TryGetValue(name, out cell);
                try {
                    Formula value = (Formula)cell.getContents();                               //Assume contents is a formula
                    old = value.GetVariables();
                }
                catch (InvalidCastException) {
                    old = new LinkedList<String>();                                            //If it is not catch exception
                }
                try {
                    ISet<String> ans = generateISet(name, formula, old);                       //Remove and add unless it is a circular exception
                    spreadsheet.Remove(name);
                    spreadsheet.Add(name, new Cell(formula, CellValueDouble));
                    return ans;
                }
                catch (CircularException)  {
                    throw new CircularException();
                }
            }
            else {
                try  {
                    ISet<String> ans = generateISet(name, formula, new HashSet<String> { });
                    spreadsheet.Add(name, new Cell(formula, CellValueDouble));           //If it is not contained, add it unless there is a circular exception
                    return ans;
                }
                catch (CircularException)
                {
                    throw new CircularException();
                }
            }
        }

        /// <summary>
        /// Helper method that returns an ISet and throws circular exception
        /// </summary>

        private ISet<String> generateISet(String name, Formula formula, IEnumerable<String> old)
        {
            dependencyGraph.ReplaceDependents(name, formula.GetVariables());
            try  {
                IEnumerable<String> use = GetCellsToRecalculate(name);                         //Try to process and return an ISet
                HashSet<String> ans = new HashSet<string>();
                foreach (String s in use)
                    ans.Add(s);
                return ans;
            }
            catch (CircularException)  {                                                        //Catch Circular Exception
                dependencyGraph.ReplaceDependents(name, old);
                throw new CircularException();
            }
        }

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if (name == null)
                throw new ArgumentNullException();  

            if (!Regex.IsMatch(name, "^[a-zA-Z]+[1-9][0-9]*$"))
                throw new InvalidNameException();                                               //If invalid or null throw exceptions

            if (dependencyGraph.HasDependees(name))
                return dependencyGraph.GetDependees(name);                                      //Return direct dependents

            return new HashSet<string>();
        }


        /// <summary>
        /// Looks at a previously saved file and return the version information.
        /// </summary>
        public override string GetSavedVersion(string filename)
        {
            return XMLFile(filename, true);                                                 // Uses XMLFile helper function      
        }

        /// <summary>
        /// Saves the spreadsheet as an XML file on disk.
        /// </summary>
        public override void Save(string filename)
        {

            if (ReferenceEquals(filename, null))                                            //If filename is null throw exception
                throw new SpreadsheetReadWriteException("The filename cannot be null");

            if (filename.Equals(""))                                                        //If filename is empty throw exception
                throw new SpreadsheetReadWriteException("The filename cannot be empty");

            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();

                using (XmlWriter writer = XmlWriter.Create(filename, settings))
                {
                    writer.WriteStartDocument();                                                   // Start the document
                    writer.WriteStartElement("spreadsheet");

                    writer.WriteAttributeString("version", null, Version);
                    foreach (string str in spreadsheet.Keys)
                    {
                        writer.WriteStartElement("cell");
                        writer.WriteElementString("name", str);

                        string content;

                        if (spreadsheet[str].getContents() is double) {                           //If contents is double save toString of double                   
                            content = spreadsheet[str].getContents().ToString();
                        }
                        else if (spreadsheet[str].getContents() is Formula)  {                    //If the contents is a Formula save toString of Formula
                            content = "=" + spreadsheet[str].getContents().ToString();
                        }
                        else {
                            content = (string)spreadsheet[str].getContents();                    //Otherwise save string
                        }

                        writer.WriteElementString("contents", content);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            catch (XmlException e)
            {
                throw new SpreadsheetReadWriteException(e.ToString());
            }
            catch (IOException e)
            {
                throw new SpreadsheetReadWriteException(e.ToString());
            }
            Changed = false;
        }


        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Gets the value as opposed to he content of the cell.
        /// </summary>
        public override object GetCellValue(string _name)
        {
            if (_name == null)
                throw new InvalidNameException();                                               //If name is null throw exception

            String name = Normalize(_name);                                                     //Normalize the name

            if (!(Regex.IsMatch(name, "^[a-zA-Z]+[1-9][0-9]*$") && IsValid(name)))              //Make sure it is valid
                throw new InvalidNameException();
            Cell cell;
            if (spreadsheet.TryGetValue(name, out cell))                                        
                return cell.getValue();
            else                                   
                return "";
        }

        /// <summary>
        /// If name is null or invalid, throws an ArgumentException.
        /// Method that return the value of a double
        /// </summary>
        private double CellValueDouble(string _name)
        {
            double value;

            if (_name == null)                                                                   //If name is null, throw exception
                throw new ArgumentException("Name given was set to null.");
            String name = Normalize(_name);                                                      //Normalize name
            if (!(Regex.IsMatch(name, "^[a-zA-Z]+[1-9][0-9]*$") && IsValid(name)))
                throw new ArgumentException("Invalid variable symbol.");                         //Make sure it is valid

            try {
                Cell cell;
                if (spreadsheet.TryGetValue(name, out cell))
                    value = (double)cell.getValue();
                else
                    throw new ArgumentException();
            }
            catch (InvalidCastException)  {
                throw new ArgumentException();                                                   //Catch exceptions
            }
            return value;
        }
        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        /// 
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor.  There are then three possibilities:
        /// 
        ///   (1) If the remainder of content cannot be parsed into a Formula, a 
        ///       SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///       
        ///   (3) Otherwise, the contents of the named cell becomes f.
        /// 
        /// Otherwise, the contents of the named cell becomes content.
        /// 
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetContentsOfCell(string _name, string content)
        {
            if (content == null)
                throw new ArgumentNullException("Content was null.");

            if (_name == null)
                throw new InvalidNameException();                                                //Check for nulls and throw exceptions

            string name = Normalize(_name);                                                      //Normalize name

            if (!(Regex.IsMatch(name, "^[a-zA-Z]+[1-9][0-9]*$") && IsValid(name)))
                throw new InvalidNameException();                                                //Make sure valid
            ISet<String> ans;
            double valueDouble;
            if (Double.TryParse(content, out valueDouble))                                       //If it is a double set to double
            {
                ans = SetCellContents(name, valueDouble);
                Cell temp;
                foreach (String str in ans) {
                    spreadsheet.TryGetValue(str, out temp);
                    temp.recalculateValue(CellValueDouble);                                     //Recalculate
                }
                Changed = true;

                return ans;
            }

            if (content.StartsWith("=")) {                                                        //If it is a formula   
                String contentTemp = content.Substring(1);
                Formula testFormula = new Formula(contentTemp, Normalize, IsValid);
                try   {
                    Changed = true;
                    ans = SetCellContents(name, testFormula);                                   //Recalculate and update formula
                    foreach (String s in ans)  {
                        Cell cell;
                        spreadsheet.TryGetValue(s, out cell);
                        cell.recalculateValue(CellValueDouble);
                    }
                    return ans;
                }
                catch (CircularException)  {                                                       //Catch circular exception
                    Changed = false;
                    throw new CircularException();
                }
            }
            Changed = true;
            ans = SetCellContents(name, content);

            foreach (String s in ans)   {
                Cell cell;
                spreadsheet.TryGetValue(s, out cell);

                if (cell != null)
                    cell.recalculateValue(CellValueDouble);                                         //Recalculate
            }

            return ans;
        }

        /// <summary>
        /// Cell class to help simplify code
        /// </summary>
        private class Cell {

            double conDouble = Double.PositiveInfinity;
            double valDouble = Double.PositiveInfinity;
            String content;
            String value;
            Formula conFormula;
            FormulaError formulaError;

            /// <summary>
            /// Constructor for cell with string
            /// </summary>
            public Cell(String contents) {
                content = contents;
                value = contents;
            }

            /// <summary>
            /// Constructor for cell with double
            /// </summary>
            public Cell(double contents) {
                conDouble = contents;
                valDouble = contents;
            }

            /// <summary>
            /// Constructor for cell with formula
            /// </summary>
            public Cell(Formula contents, Func<string, double> lookup)
            {
                conFormula = contents;
                try {
                    valDouble = (double)contents.Evaluate(lookup);
                }
                catch (InvalidCastException) {
                    formulaError = (FormulaError)contents.Evaluate(lookup);
                }
            }

            /// <summary>
            /// Returns the content
            /// </summary>
            public object getContents() {
                if (content != null)                                                                        //If content is not null or empty return content, otherwise return error
                    return content;
                else if (conDouble != Double.PositiveInfinity)                                              
                    return conDouble;
                else
                    return conFormula;
            }

            /// <summary>
            /// Returns the Value of a cell
            /// </summary>
            /// <returns>String, Double, or FormulaError</returns>
            public object getValue()    {
                if (value != null)                                                                          //If content is not null or empty return content, otherwise return error
                    return value;
                else if (valDouble != Double.PositiveInfinity)
                    return valDouble;
                else
                    return formulaError;
            }

            /// <summary>
            /// Recalculates the value of the cell
            /// </summary>
            public void recalculateValue(Func<string, double> lookup)
            {
                if (content != null)
                    value = content;
                else if (conDouble != Double.PositiveInfinity)
                    valDouble = conDouble;
                else  {
                    try {
                        valDouble = (double)conFormula.Evaluate(lookup);                                    //try to reevaluate
                    }
                    catch (InvalidCastException)  {                                                         //Return formula error
                        formulaError = (FormulaError)conFormula.Evaluate(lookup);
                    }
                }
            }
        }
    }
}