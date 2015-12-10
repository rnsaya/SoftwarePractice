// Skeleton written by Joe Zachary for CS 3500, September 2013
// Completed by Rachel Saya for CS 3500, September 2015


using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax; variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {

        private IEnumerable<string> formula;
        Func<string, bool> validator;
        Func<string, string> normalizer;


        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            if (formula == null)
                throw new FormulaFormatException("Exception: no formula passed in");
            try
            {
                this.normalizer = normalize;
            }
            catch
            {
                throw new FormulaFormatException("Not a legal variable");
            }
            try
            {
                this.validator = isValid;
            }
            catch
            {
                throw new FormulaFormatException("Not valid");
            }

            this.formula = ValidFormula(formula);

        }

        /// <summary>
        /// Makes sure the string formula is valid. If it is not then it throws a FormulaFormatException
        /// </summary>
        /// <param name="formula">String to be converted to a valid formula</param>
        /// <returns>IEnumberable<string></returns>
        private IEnumerable<string> ValidFormula(string formula)
        {
            if (formula == null || formula == "")                                                           //Checks for nulls and empty strings
                throw new FormulaFormatException("Nor formula found");

            List<string> formulas = new List<string>();                                                     //Formula to be returned

            IEnumerable<string> tokens = GetTokens(formula);

            List<string> operators = new List<string> { "+", "-", "*", "/" };                               //Potential Operators

            // Stack to check for matching parenthesis
            Stack<string> parenthesis = new Stack<string>();
            bool leftOperator = false;
            bool rightNumVar = false;

            int count = 0;

            double token;

            foreach (string str in tokens)
            {
                if (str == "")                                                                              //Ignore empty strings                                                                             
                    continue;

                if (count == 0 || count == tokens.Count() - 1)                                              // Check to make sure first or last token is a number, 
                {                                                                                           // variable, or left/right parenthisis
                    if (!double.TryParse(str, out token) && !ValidVariable(str))
                    {
                        if ((count == 0 && str != "(") || (count == tokens.Count() - 1 && str != ")"))
                            throw new FormulaFormatException("Formula did not start and end with a number, variable, or left paren");
                    }
                }

                if (leftOperator)                                                                           // Check parenthesis following rule
                    if (!double.TryParse(str, out token) && !ValidVariable(str) && str != "(")
                        throw new FormulaFormatException("Not followed by a number, variable, or opening parenthisis");


                if (rightNumVar)                                                                            // Check extra following rule
                    if (str != ")" && !operators.Contains(str))
                        throw new FormulaFormatException("Not followed by a closing parenthesis or operator");

                leftOperator = false;
                rightNumVar = false;

                if (str == "(")
                {
                    parenthesis.Push(str);
                    leftOperator = true;
                }

                else if (str == ")")
                {
                    rightNumVar = true;
                    if (parenthesis.Count > 0)
                        parenthesis.Pop();
                    else
                        throw new FormulaFormatException("Missing parenthesis");
                }

                else if (operators.Contains(str))                                                         //Check for operators
                {
                    leftOperator = true;
                }
                // Check if str is a valid variable or number
                else if ((ValidVariable(str) && ValidVariable(normalizer(str))) || double.TryParse(str, out token))
                {
                    rightNumVar = true;
                }

                else
                {
                    throw new FormulaFormatException("Token found that is not valid");
                }
                if (double.TryParse(str, out token))
                    formulas.Add(token.ToString());
                else
                    formulas.Add(str);
                count++;
            }

            if (parenthesis.Count != 0)                                                                   //Check Balanced Parentheses Rule
                throw new FormulaFormatException("Unbalanced parenthesis");

            return formulas;
        }


        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        ///
        /// </summary>

        public object Evaluate(Func<string, double> lookup)
        {
            string[] substrings = Regex.Split(this.ToString(), "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            Stack<String> operators = new Stack<String>();
            Stack<Double> values = new Stack<Double>();
            double value = -1;
            String current;
            double vTop;
            String oTop;
            for (int i = 0; i < substrings.Length; i++)                                                 //Processes tokens
            {
                current = substrings[i].Trim();
                if (double.TryParse(current, out value))                                                //Checks if current as a literal value
                {
                    if (operators.Count > 0 && (operators.Peek().Equals("*") || operators.Peek().Equals("/")))
                    {
                        if (values.Count == 0)
                            return new FormulaError("There are no values available!");
                        if (value == 0.0)
                            return new FormulaError("Cannot divide by zero!");
                        values.Push(multDiv(operators.Pop(), values.Pop(), value));
                    }
                    else
                        values.Push(value);
                }
                else if (ValidVariable(current))                                                        //Checks if valid variable
                {
                    value = lookup(normalizer(current));
                    if (operators.Count > 0 && (operators.Peek().Equals("*") || operators.Peek().Equals("/")))
                    {
                        if (values.Count == 0)
                            return new FormulaError("There are no values available!");
                        if (value == 0.0)
                            return new FormulaError("Cannot divide by zero!");
                        values.Push(multDiv(operators.Pop(), values.Pop(), value));
                    }
                    else
                        values.Push(value);
                }
                else if (current.Equals("+") || current.Equals("-"))                                    // Checks if current is addition or subtraction
                {
                    String temp;
                    if (operators.Count > 0 && (operators.Peek().Equals("+") || operators.Peek().Equals("-")))
                    {
                        temp = operators.Pop();
                        operators.Push(current);
                        current = temp;
                    }
                    if (values.Count < 2 || operators.Contains("("))
                        operators.Push(current);
                    else if (current.Equals("+") || current.Equals("-"))
                    {
                        vTop = values.Pop();
                        values.Push(addSub(current, values.Pop(), vTop));
                    }

                }
                else if (current.Equals("*") || current.Equals("/") || current.Equals("("))
                    operators.Push(current);
                else if (current.Equals(")"))                                                           // Checks for left bracket
                {
                    if (values.Count < 2)
                        return new FormulaError("There are not enough values available!");
                    oTop = operators.Pop();
                    vTop = values.Pop();

                    if (oTop.Equals("+") || oTop.Equals("-"))
                        values.Push(addSub(oTop, values.Pop(), vTop));

                    if (operators.Count == 0)
                        return new FormulaError("A '(' was expected");

                    oTop = operators.Pop();
                    if (!oTop.Equals("("))
                        return new FormulaError("A '(' was expected but a '" + current + "' was found!");
                    else if (operators.Count > 0 && (operators.Peek().Equals("*") || operators.Peek().Equals("/")))                                                // Division and multiplication
                    {
                        oTop = operators.Pop();
                        if (values.Count < 2)
                            return new FormulaError("There are not enough values available!");
                        vTop = values.Pop();
                        if (oTop.Equals("*") || oTop.Equals("/")) {
                            if (vTop == 0)
                                return new FormulaError("Cannot divide by zero!");
                            values.Push(multDiv(oTop, values.Pop(), vTop));
                        }
                        else
                            values.Push(vTop);

                    }
                }
                else if (String.IsNullOrWhiteSpace(current) || current.Equals(", "))                  //Ignores whitespace and ", "
                    continue;
                else
                    return new FormulaError("Invalid entry!");

            }
            double answer;
            if (operators.Count == 0 && values.Count == 1)    {                                          // Makes sure all conditions have been met for correct output
                answer = (values.Pop());
                return answer;
            }
            else if (operators.Count == 1 & values.Count == 2 &&
               (operators.Peek().Equals("+") || operators.Peek().Equals("-")))
            {
                vTop = values.Pop();
                answer = (double)addSub(operators.Peek(), values.Pop(), vTop);
                return answer;
            }
            else
                return new FormulaError("Not enough or too many tokens!");
        }


        /// <summary>
        /// Adds or subtracts num 1 from num 2 based on whether addSub is "+" or "-"
        /// </summary>
        /// <param name="addSub"></param>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns>The product</returns>
        private double addSub(String addSub, double num1, double num2)
        {
            if (addSub.Equals("+"))
                return num1 + num2;
            else
                return num1 - num2;
        }

        /// <summary>
        /// Divides or multiplies num 1 from num 2 based on whether addSub is "*" or "/"
        /// </summary>
        /// <param name="multDiv"></param>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns>Result of multiplication and division</returns>
        private double multDiv(String multDiv, double num1, double num2)
        {
            if (multDiv.Equals("*"))
                return num1 * num2;
            else   
                return num1 / num2;
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            {
                HashSet<string> variables = new HashSet<string>();
                foreach (string str in formula)
                {
                    if (ValidVariable(normalizer(str)))                                                 //If the normalizer of each string in formula is valid
                        variables.Add(normalizer(str));                                                 //Add it to the string HashSet variables
                }
                return variables;
            }
        }

        /// <summary>
        /// Decides whether or not the string is a valid variable. Valid variables
        /// consists of a letter or underscore followed by zero or more letters, underscores,
        /// or digits.
        /// </summary>
        /// <param name="str">The variable</param>
        /// <returns>True if str is valid</returns>
        private bool ValidVariable(String str)
        {
            if (Regex.IsMatch(str, "^[_a-zA-Z][a-zA-Z0-9_]*$"))                                         //Regex checks for a letter or underscore followed by zero 
                return validator(normalizer(str));                                                      //or more letters, underscores, or digits.
            return false;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// </summary>
        public override string ToString()
        {
            string noSpace = "";
            foreach (string str in formula)
                noSpace = noSpace + str;                                                                //Adds each str in formula to a new string that has no spaces
            return noSpace;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens, which are compared as doubles, and variable tokens,
        /// whose normalized forms are compared as strings.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.Equals(""))                                                          //Checks for nulls
                return false;
            return (this.ToString() == obj.ToString()) && (this.GetHashCode() == obj.GetHashCode());    //Returns true if equal and false if not based off HashCode and string
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            if (ReferenceEquals(f1, null) || ReferenceEquals(f2, null))                                  //Check to see if there is a null
            {
                if (ReferenceEquals(f1, null) && ReferenceEquals(f2, null))                              //Checks to see if both are null
                    return true;
                else
                    return false;
            }
            return f1.Equals(f2);                                                                        //Otherwise use equals function
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            if (ReferenceEquals(f1, null) || ReferenceEquals(f2, null))                                   //Check to see if there is a null
            {
                if (ReferenceEquals(f1, null) && ReferenceEquals(f2, null))                               //Checks to see if both are null
                    return false;
                else
                    return true;
            }
            return !f1.Equals(f2);                                                                        //Otherwise return not of equals function
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();                                                          //Use GetHashCode() on string version of formula
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}