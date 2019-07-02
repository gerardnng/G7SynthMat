using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrafcetConvertor;
using System.Windows.Forms;

namespace GrafcetConvertor
{

    /// <summary>
    /// Expression evaluator class
    /// </summary>
    public static class Eval
    {
        // Token state enums
        private enum State
        {
            None = 0,
            Operand = 1,
            Operator = 2,
            UnaryOperator = 3
        }

        /// <summary>
        /// Converts a standard infix expression to list of tokens in
        /// postfix order.
        /// </summary>
       public static List<string> analyze(string expression)
        {
            List<string> tokens = new List<string>();
            Stack<string> stack = new Stack<string>(); //tokens analyse a siack is need
            State state = State.None;
            int parenCount = 0;
            string temp;

            TextParser parser = new TextParser(expression);

            while (!parser.EndOfText)
            {
                if (Char.IsWhiteSpace(parser.Peek()))
                {
                    // Ignore spaces, tabs, etc.
                }
                else if (parser.Peek() == '(')
                {
                    // Allow additional unary operators after "("
                    if (state == State.UnaryOperator)
                        state = State.Operator;
                    // Push opening parenthesis onto stack
                    stack.Push(parser.Peek().ToString());
                    // Track number of parentheses
                    parenCount++;
                }
                else if (parser.Peek() == ')')
                {
                    // Pop all operators until matching "(" found
                    temp = stack.Pop();
                    while (temp != "(")
                    {
                        tokens.Add(temp);
                        temp = stack.Pop();
                    }
                    // Track number of parentheses
                    parenCount--;
                }
                
                else if ("+*!^".Contains(parser.Peek()))
                {
                    // Need a bit of extra code to support unary operators
                    if (state == State.Operand)
                    {
                        // Pop operators with precedence >= current operator
                        int currPrecedence = GetPrecedenceIndex(parser.Peek().ToString());
                        while (stack.Count > 0 && GetPrecedenceIndex(stack.Peek()) >= currPrecedence)
                            tokens.Add(stack.Pop());
                        stack.Push(parser.Peek().ToString());
                        state = State.Operator;
                    }
                    else
                    {
                        // Test for unary operator
                        if (parser.Peek() == '!')
                        {
                            stack.Push("!");
                            state = State.UnaryOperator;
                        }
                        else
                            if (parser.Peek() == '^')
                            {
                                stack.Push("^");
                                state = State.UnaryOperator;
                            }
                    }
                }
                else //if (Char.IsDigit(parser.Peek()))
                {
                    // Parse a variable that represents a number
                    temp = ParseNumberToken(parser);
                    tokens.Add(temp);
                    state = State.Operand;
                    continue;
                }
                
                parser.MoveAhead();
            }
            // Retrieve remaining operators from stack
            while (stack.Count > 0)
                tokens.Add(stack.Pop());
            return tokens;
        }

        /// <summary>
        /// Parses and extracts a numeric value at the current position
        /// </summary>
        /// <param name="parser">TextParser object</param>
        /// <returns></returns>
       public static string ParseNumberToken(TextParser parser)
        {
            //bool hasDecimal = false;
            int start = parser.Position;
            while (!"+*!()^".Contains(parser.Peek()) && !parser.EndOfText)
            {
                parser.MoveAhead();
                //MessageBox.Show("In ParseNumberToken on "+parser.Peek());
            }
            // Extract token
            string token = parser.Extract(start, parser.Position);
            return token;
        }

        /// <summary>
        /// Evaluates the given list of tokens and returns the result.
        /// Tokens must appear in postfix order.
        /// </summary>
        /// <param name="tokens">List of tokens to evaluate.</param>
        /// <returns></returns>
       public static Boolean Execute(List<string> tokens)
        {
           //NNg
           string toAff="";
           foreach(string toc in tokens)
               toAff+=toc;
           //MessageBox.Show(toAff);

            Stack<bool> stack = new Stack<bool>();

            foreach (string token in tokens)
            {
                // Is this a value token?
                int count = token.Count(c => Char.IsDigit(c));
                if (count == token.Length)
                {
                    if (token == "1")
                    {
                        stack.Push(true);
                    }
                    else
                    {
                        stack.Push(false);
                    }
                }
                else if (token == "+")
                {
                    stack.Push(stack.Pop() | stack.Pop());
                }

                else if (token == "*")
                {
                    stack.Push(stack.Pop() & stack.Pop());
                }

                else if (token == "!")
                {
                    stack.Push(!stack.Pop());
                }
                else if (token == "^")
                {
                    //Ti correct HERE
                    bool old_state = false;
                    stack.Push(old_state && stack.Pop());
                }
            }
            // Remaining item on stack contains result
            return (stack.Count > 0) ? stack.Pop() : false;
        }

        /// <summary>
        /// Returns a value that indicates the relative precedence of
        /// the specified operator
        /// </summary>
        /// <param name="s">Operator to be tested</param>
        /// <returns></returns>
       public static int GetPrecedenceIndex(string s)
        {
            switch (s)
            {
                case "+":

                    return 1;

                case "*":

                    return 2;

                case "!":

                    return 10;
                
                case "^":

                    return 11;
            }
            return 0;
        }
    }
}
