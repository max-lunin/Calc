namespace Calc
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Input a mathematical expression to be calculated\r\n");
                var consoleInput = Console.ReadLine();
                if (consoleInput == String.Empty)
                {
                    Console.WriteLine("Empty input! Try again!");
                    return; 
                }
                var validatedInput = InputValidate(consoleInput);
                Console.WriteLine($"\r\nResult is {BracketAnalyzer(validatedInput)}");
                Environment.Exit(0);
        }
        static void IncorrectInput()
        {
            Console.WriteLine("\r\nIncorrect input!");
            Environment.Exit(0);
        }
        private static string InputValidate(string input)
        {
            char[] actions = {'+','-','*','/'};
            input = input.Replace(" ", "");
            if (!(input[0] == '-' && Char.IsDigit(input[1])) && !Char.IsDigit(input[0]) && input[0] != '(')
            {
                IncorrectInput();
            }
            for (int i = 0; i < input.Length; i++)
            {
                if (!Char.IsDigit(input[i]) && input[i] != '(' && input[i] != ')' && !(actions.Contains(input[i])) && input[i] != '.')
                    IncorrectInput();
                else if (i == input.Length - 1 && (input[i] == '.' || input[i] == '(' || actions.Contains(input[i])))
                    IncorrectInput();
                else if (actions.Contains(input[i]) && actions.Contains(input[i + 1]) && actions.Contains(input[i + 2]))
                    IncorrectInput();
                else if (input[i] == '(' && !Char.IsDigit(input[i + 1]) && !Char.IsDigit(input[i + 2]))
                    IncorrectInput();
                else if (input[i] == '(' && input[i + 1] == '-' && !Char.IsDigit(input[i + 2]))
                    IncorrectInput();
                else if (actions.Contains(input[i]) && input[i + 1] == ')')
                    IncorrectInput();
                else if ((input[i] == '.' && !Char.IsDigit(input[i - 1])) || (input[i] == '.' && !Char.IsDigit(input[i + 1])))
                    IncorrectInput();
                else if (i < input.Length - 1 && input[i] == ')' && Char.IsDigit(input[i + 1]))
                    IncorrectInput();
            }
                return input;
        }

        private static string BracketAnalyzer(string validatedInput)
        {
            bool bracketIsOpen = false;
            bool doubleBrackets = false;
            int openingBracketIndex = -1;
            int closingBracketIndex = -1;

            for (int i = 0; i < validatedInput.Length; i++)
            {
                if (validatedInput[i] == '(')
                {
                    bracketIsOpen = true;
                    openingBracketIndex = i;
                }
                if (validatedInput[i] == '(' && bracketIsOpen) 
                    doubleBrackets = true; 
                if (validatedInput[i] == ')')
                {
                    if (bracketIsOpen)
                    {
                        closingBracketIndex = i;
                        string tempResult = ExpressionAnalyzer(validatedInput.Substring(openingBracketIndex + 1, closingBracketIndex - openingBracketIndex - 1));
                        validatedInput = validatedInput.Remove(openingBracketIndex, closingBracketIndex - openingBracketIndex + 1);
                        validatedInput = validatedInput.Insert(openingBracketIndex, tempResult);
                        bracketIsOpen = doubleBrackets;
                        openingBracketIndex = -1;
                        closingBracketIndex = -1;
                        doubleBrackets = false;
                        i = -1;
                    }
                    else
                    {
                        Console.WriteLine("\r\nThere is no opening bracket");
                        Environment.Exit(0);
                    }
                }
            }
            return ExpressionAnalyzer(validatedInput);
        }

        static string ExpressionAnalyzer(string expression)
        {
            char[] actions = { '+', '-', '*', '/' };
            List<string> list = new List<string>();
            string temp = "";
            for (int i = 0; i < expression.Length; i++)
            {
                if (i == 0 && expression[i] == '-' && Char.IsDigit(expression[i + 1]))
                    temp += expression[i];
                else if (!actions.Contains(expression[i]))
                {
                    temp += expression[i];
                }
                else if (actions.Contains(expression[i]) && expression[i] != '-')
                {
                    list.Add(temp);
                    list.Add((expression[i]).ToString());
                    temp = "";
                }
                else if (actions.Contains(expression[i - 1]) && expression[i] == '-' && Char.IsDigit(expression[i + 1]))
                    temp += expression[i];
                else
                {
                    list.Add(temp);
                    list.Add((expression[i]).ToString());
                    temp = "";
                }
            }
                list.Add(temp);
            for (int j = 0; j < list.Count; j++)
            {
                if (list[j] == "*" || list[j] == "/" )
                {
                    if (list[j] == "/" && list[j+1] == "0")
                    {
                        Console.WriteLine("Division by zero is forbidden!");
                        Environment.Exit(0);
                    }
                    string tempResult = Calc(list[j - 1], list[j + 1], list[j]); 
                    list.RemoveRange(j-1,3);
                    list.Insert(j-1, tempResult);
                    j = 0;
                }
            }
            for (int j = 0; j < list.Count; j++)
            {
                    if (list[j] == "+" || list[j] == "-")
                    {
                        string tempResult = Calc(list[j - 1], list[j + 1], list[j]);
                        list.RemoveRange(j - 1, 3);
                        list.Insert(j - 1, tempResult);
                        j = 0;
                    }
                }
            
            return list[0];
        }

        static string Calc(string operand1, string operand2, string action)
        {
            var oper1 = Convert.ToDouble(operand1);
            var oper2 = Convert.ToDouble(operand2);

            switch(action)
            {
                case "+":
                    return (oper1 + oper2).ToString();
                case "-":
                    return (oper1 - oper2).ToString();
                case "*":
                    return (oper1 * oper2).ToString();
                case "/":
                    return (oper1 / oper2).ToString();
                default:
                    return "0";
            }
        }
    }
}