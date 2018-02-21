using System;
using System.Globalization;

namespace Calculator
{
    class Calculator
    {
        static void Main(string[] args)
        {
            //Change to US for "." as the decimal symbol
            CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

            Calculator calclator = new Calculator();
            calclator.Start();
        }

        //Setting up some enums to their corresponding character codes
        enum Operator {
            None,
            Addition = 43,
            Subtration = 45,
            Multiplication = 42,
            Division = 47
        };
        enum Characters {
            Plus = 43,
            Minus = 45,
            Asterisk = 42,
            ForwardSlash = 47,
            Enter = 13,
            Equals = 61,
            Dot = 46,
            Escape = 27
        };

        //The input gets added to inputLine if its valid, also is checked for being ESC
        char inputChar;

        //Gets turned into a number for calculations when user finishes typing
        string inputLine;

        //Stores the result for display & further calculations
        float result;

        //If user already has a decimal sign in the input
        bool hasDecimal;

        //Decides what to do with the result and input when its time to calculate
        Operator currentOperator;

        //The menu that is displayed at all times
        string menu;

        public void Start() {
            //Initialize variables
            inputLine = "";
            result = 0;
            hasDecimal = false;
            currentOperator = Operator.None;

            //Make a nice menu
            menu = "Calculator";
            menu += "\nAvailable functions:";
            //Tabs for indentation
            menu += "\n\t+, Addition";
            menu += "\n\t-, Subtractoin";
            menu += "\n\t*, Multiplication";
            menu += "\n\t/, Division";
            menu += "\nPress = to view result, ESC to exit.";
            UpdateView();

            while(true)
            {
                inputChar = Console.ReadKey(true).KeyChar;
                switch ((Characters)inputChar)
                {
                    case Characters.Plus: // +: addition
                        //Calculate, set new operator, display
                        result = Calculate();
                        currentOperator = Operator.Addition;
                        UpdateView(result + "+");

                        //Reset variables
                        ResetVars();
                        break;
                    case Characters.Minus: // -: subtraction
                        //Calculate, set new operator, display
                        result = Calculate();
                        currentOperator = Operator.Subtration;
                        UpdateView(result + "-");

                        //Reset variables
                        ResetVars();
                        break;
                    case Characters.Asterisk: // *: multiplicaton
                        //Calculate, set new operator, display
                        result = Calculate();
                        currentOperator = Operator.Multiplication;
                        UpdateView(result + "*");

                        //Reset variables
                        ResetVars();
                        break;
                    case Characters.ForwardSlash: // /: division
                        //Calculate, set new operator, display
                        result = Calculate();
                        currentOperator = Operator.Division;
                        UpdateView(result + "/");

                        //Reset variables
                        ResetVars();
                        break;
                    case Characters.Enter:  // Enter, \r:
                    case Characters.Equals: // =: calculate and display
                        //Build line for result, unlike the others this will show the previous result
                        string toWrite = "";
                        if (currentOperator != Operator.None)
                        {
                            toWrite = result + ((char)currentOperator).ToString() + inputLine + " = ";
                        } else
                        {
                            //Weird case for when the user doesn't actually want to do math
                            //If there has been input, thats the new result,
                            //otherwise it's the previous result again
                            if(inputLine.Length > 0)
                                toWrite = inputLine + " = ";
                            else
                                toWrite = result + " = ";
                        }

                        //Calculate, reset operator, display
                        result = Calculate();
                        currentOperator = Operator.None;
                        UpdateView(toWrite + result);

                        //Reset variables
                        ResetVars();
                        break;
                    case Characters.Escape: //ESC: exit program
                        Environment.Exit(0);
                        break;
                    case Characters.Dot: // .: act as a digit if its the first instance

                        //Input already has a dot, abort
                        if (hasDecimal) break;

                        //Clear result from last run if its still there, and replace with new input
                        if (inputLine.Length == 0 && currentOperator == Operator.None)
                        {
                            UpdateView("");
                        }

                        //Input now has a dot
                        hasDecimal = true;

                        //Display & add to input
                        Console.Write(inputChar);
                        inputLine += inputChar;
                        break;
                    default: //Other, check if digit
                        if (Char.IsDigit(inputChar))
                        {
                            //Clear result from last run if its still there, and replace with new input
                            if (inputLine.Length == 0 && currentOperator == Operator.None)
                            {
                                UpdateView("");
                            }

                            //Display & add to input
                            Console.Write(inputChar);
                            inputLine += inputChar;
                        }
                        break;
                }
            }
        }

        private void ResetVars()
        {
            //Reset to initial values
            inputLine = "";
            hasDecimal = false;
        }

        private void UpdateView(string toWrite = "")
        {
            //Clear presvious text
            Console.Clear();
            //Show the menu
            Console.WriteLine(menu);
            //Additional text
            Console.Write(toWrite);
        }

        private float Calculate()
        {
            //Convert input into a number
            float input;
            if(!float.TryParse(inputLine, out input))
            {
                //Probably entered an unreasonably large number
                //Could also be an empty input
                //Either way just return at what we're on, this calculator is for proper math only
                return result;
            }

            //Redirect to the correct function
            switch (currentOperator)
            {
                case Operator.Addition:
                    return Add(result, input);
                case Operator.Subtration:
                    return Subtract(result, input);
                case Operator.Multiplication:
                    return Multiply(result, input);
                case Operator.Division:
                    return Divide(result, input);
                case Operator.None: //First time or first input after "=", so we set it as the result
                    return input;
            }
            //Won't reach this unless someone changed code
            //It's just to make VS stop complaining
            return result;
        }
        
        //Divides the first number by the second
        private float Divide(float first, float second)
        {
            return first / second;
        }

        //Multiplies two numbers together
        private float Multiply(float first, float second)
        {
            return first * second;
        }

        //Subtracts the second number from the first
        private float Subtract(float first, float second)
        {
            return first - second;
        }

        //Adds two numbers together
        private float Add(float first, float second)
        {
            return first + second;
        }
    }
}
