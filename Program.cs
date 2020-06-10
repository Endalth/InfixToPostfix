using System;
using System.Collections.Generic;

namespace InfixToPostfix
{
    class Program
    {
        static Dictionary<char, int> precedence = new Dictionary<char, int>
        {
            {'(', 0 }, //we don't want anything to pop '(' that is why it is 0
            {'^', 3 },
            {'*', 2 },
            {'/', 2 },
            {'+', 1 },
            {'-', 1 }
        };

        static void Main(string[] args)
        {
            Stack<char> myStack = new Stack<char>();

            string operators = "-+*/^()";
            string[] expressions = new string[]
            {
                "A^B^C",
                "A*(B+C)",
                "A-B*C^D",
                "A+B*C-D*E",
                "((A+B)*C-D)*E"
            };

            foreach (string expression in expressions)
            {
                string postfixString = "";
                Console.Write(expression + " -> ");
                for (int i = 0; i < expression.Length; i++)
                {
                    char character = expression[i];
                    if (!operators.Contains(expression[i]))
                    {
                        postfixString += character;
                    }
                    else
                    {
                        if (character == '(')
                            myStack.Push(character);
                        else if (character == ')')
                        {
                            while (myStack.Peek() != '(')
                            {
                                postfixString += myStack.Pop();
                            }
                            myStack.Pop(); //To remove '('
                        }
                        else
                        {
                            while (myStack.Count > 0 && HasHigherOrEqualPrec(myStack.Peek(), character))
                            {
                                postfixString += myStack.Pop();
                            }
                            myStack.Push(character);
                        }
                    }
                }

                //Append what is left inside the stack to postfixString
                while (myStack.Count > 0)
                {
                    postfixString += myStack.Pop();
                }

                Console.WriteLine(postfixString);
            }


            Console.ReadKey();
        }

        static bool HasHigherOrEqualPrec(char first, char second)
        {
            //Exponent is right to left that is why we don't want it to pop itself
            //without this if statement 2^3^4 would be 23^4^ and this is wrong
            if (first == '^' && second == '^')
                return false;

            return precedence[first] >= precedence[second];
        }
    }
}
