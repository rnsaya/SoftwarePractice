// Created by Rachel Saya

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace FormulaEvaluator
{
    /// <summary>
    /// Evaluator contains a method that evaluates arithmetic expressions
    ///  written using standard infix notation. 
    /// </summary>
    public static class Evaluator
    {
        /// <summary>
        /// Lookup variables will contain methods that have String input and int output
        /// </summary>
        /// <param name="v">Lookup function</param>
        /// <returns></returns>
        public delegate int Lookup(String v);


        /// <summary>
        /// Evaluates arithmetic expressions written using standard infix notation. 
        /// </summary>
        /// <param name="exp">The expression to be evaluated.</param>
        /// <param name="variableEvaluator">A function used to lookup variable values.</param>
        /// <returns></returns>
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            Stack<String> operators = new Stack<String>();
            Stack<Double> values = new Stack<Double>();
            double value = -1;
            String current;
            Regex reg = new Regex("^[a-zA-Z][0-9]*$");
            double vTop;
            String oTop;
            for (int i = 0; i < substrings.Length; i++)                         //Processes tokens
            {
                current = substrings[i].Trim();
                if (double.TryParse(current, out value))                        //Checks if current as a literal value
                {
                    if(operators.hasMDOnTop())
                    {
                        if (values.Count == 0)
                            throw new ArgumentException("There are no values available!");
                        values.Push(multDiv(operators.Pop(), values.Pop(), value));
                    }
                    else
                        values.Push(value);
                }
                else if (reg.IsMatch(current))                                  //Checks if valid variable
                {
                    value = variableEvaluator(current);
                    if (operators.hasMDOnTop())
                    {
                        if (values.Count == 0)
                            throw new ArgumentException("There are no values available!");
                        values.Push(multDiv(operators.Pop(), values.Pop(), value));
                    }
                    else
                        values.Push(value);
                }
                else if (current.Equals("+") || current.Equals("-"))            // Checks if current is addition or subtraction
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
                else if (current.Equals(")"))                                                        // Checks for left bracket
                {
                    if (values.Count < 2)
                        throw new ArgumentException("There are not enough values available!");
                    oTop = operators.Pop();
                    vTop = values.Pop();

                    if (oTop.Equals("+") || oTop.Equals("-"))
                        values.Push(addSub(oTop, values.Pop(), vTop));

                    if (operators.Count == 0)
                        throw new ArgumentException("A '(' was expected but a '" + current + "' was found!");

                    oTop = operators.Pop();
                    if (!oTop.Equals("("))
                        throw new ArgumentException("A '(' was expected but a '" + current + "' was found!"); 
                    else if (operators.hasMDOnTop())                                                // Division and multiplication
                    {
                        oTop = operators.Pop();
                        if (values.Count < 2)
                            throw new ArgumentException("There are not enough values available!");
                        vTop = values.Pop();
                        if (oTop.Equals("*") || oTop.Equals("/"))
                            values.Push(multDiv(oTop, values.Pop(), vTop));
                        else
                            values.Push(vTop);

                    }
                }
                else if (String.IsNullOrWhiteSpace(current) || current.Equals(", "))                  //Ignores whitespace and ", "
                    continue;
                else
                    throw new System.ArgumentException("Invalid entry!");

            }
            if (operators.Count == 0 && values.Count == 1)                                            // Makes sure all conditions have been met for correct output
                return (int)Math.Truncate(values.Pop());
            else if (operators.Count == 1 & values.Count == 2 &&
               (operators.Peek().Equals("+") || operators.Peek().Equals("-")))
            {
                vTop = values.Pop();
                return (int)Math.Truncate(addSub(operators.Peek(), values.Pop(), vTop));
            }
            else
                throw new ArgumentException("Not enough or too many tokens!");
        }


        /// <summary>
        /// Adds or subtracts num 1 from num 2 based on whether addSub is "+" or "-"
        /// </summary>
        /// <param name="addSub"></param>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns>The product</returns>
        private static double addSub(String addSub, double num1, double num2)
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
        private static double multDiv(String multDiv, double num1, double num2)
        {
            if (multDiv.Equals("*"))
                return num1 * num2;
            else {
                if(num2 != 0)
                   return num1 / num2;
            }
            throw new ArgumentException("Cannot divide by zero!");
        }

        /// <summary>
        /// Extension method that determines whether or not the top of the stack is a "*" or "/"
        /// </summary>
        /// <param name="stack"></param>
        /// <returns></returns>
        public static Boolean hasMDOnTop(this Stack<String> stack)
        {
            if (stack.Count > 0 && (stack.Peek().Equals("*") || stack.Peek().Equals("/")))
                return true;
            return false;
        }


    }
}
