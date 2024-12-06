using System.Runtime.CompilerServices;
using System.Windows.Markup;

namespace CalculatorLib
{
    public class Calculator
    {
        //Operations:∨, ∧, ¬, ⊕, →, ~, ↓, ↑
        //Operations: +, *, ¬, O, →, ~, ↓, ↑
        //Example: (x → y) ~ (y → z)
        string equation = "(x→y)~(y→z)";
        char[] operations = new char[] { '+', '*', '¬', 'O', '→', '~', '↓', '↑' };
        char[] varLetters = new char[] { 'x', 'y', 'z' };
        List<char> variables;
        string[,] truthTable;

        
        public List<char> InitVariables(string eq) 
        {
            //Method is used to find what variables are in equation
            List<char> temp = new List<char>();
            foreach (char letter in eq) 
            {
                if(varLetters.Contains(letter) && !temp.Contains(letter))
                    temp.Add(letter);
            }
            temp.Sort();
            variables = new List<char>(temp);
            return temp;
        }
        public bool GetPartialResult(string eqPart) 
        {
            //Method solves the part of equation returning it's result
            //  Example of what we sent: 0→1
            //  Here we get boolean values from string partial equation
            //      Left value
            bool left;
            if(eqPart[0] == '1') 
                left = true;
            else
                left = false;
            //      Operation
            char operation = eqPart[1];
            //      Right value
            bool right = false;
            if (eqPart.Length == 3)
                if(eqPart[2] ==  '1')
                    right = true; 
                else
                    right = false;

            bool result = false;
            switch (operation) 
            {
                case '¬': result = !left; break;
                case '+': result = left || right; break;
                case '*': result = left && right; break;
                case 'O': result = (left&&!right) || (!left&&right); break;
                case '→': result = !left || right; break;
                case '~': result = (left&&right) || (!left&&!right); break;
                case '↑': result = !left || !right; break;
                case '↓': result = !left && !right; break;
            }
            return result;
        }
        public bool Simplify(string valuesOfVariables) 
        {
            //Method gets rid of brackets, simplifying the equation and returning result of the equation
            //  Replacing vars with values
            string tempEq = equation;
            for (int i = 0; i < variables.Count; ++i)
            {
                //Clearing equation from spacebars
                tempEq = tempEq.Replace(" ", "");
                //Replace var with value (X is 0 | Y is 1 etc.)
                tempEq = tempEq.Replace(variables[i], valuesOfVariables[i]);
            }
            //  Getting rid of brackets
            while (tempEq.Contains('(')) 
            {
                //Indexes of the deepest brackets "(" and ")"
                int start = tempEq.LastIndexOf('(');
                int end = tempEq.IndexOf(')', start);
                

                //Inner Equation
                string partialEq = tempEq.Substring(start + 1, end - start - 1);

                //Getting Partial result
                bool partialRes = GetPartialResult(partialEq);

                //Replacing substring with partialResult
                tempEq = tempEq.Substring(0, start) + (partialRes ? "1":"0") + tempEq.Substring(end + 1);
            }
            //  If no brackets have left returning result of the equation 
            return GetPartialResult(tempEq);
        }
        public bool CheckBrackets(string eq)
        {
            //Method is used to check the correctness of the equation brackets
            int balance = 0;
            foreach (char letter in eq)
            {
                if (letter == '(')
                    ++balance;
                if (letter == ')')
                    --balance;
                //If the order of brackets is incorrect
                if (balance < 0)
                    return false;
            }
            if (balance == 0)
                return true;
            return false;
        }
        public string[,] InitTruthTable() 
        {
            //Method is used to create a truth table
            //  One row is for header
            int rows = 1 + (1 * (int)Math.Pow(2, variables.Count));
            //  One column is for result
            int columns = variables.Count + 1;


            string[,] temp = new string[rows, columns];
            byte bCtr = 0;
            
            //Iterating through table
            for(int row = 0; row < rows; ++row) 
            {
                for(int col = 0; col < columns; ++col) 
                {
                    //Filling the header of the table
                    //  filling variables
                    if(row == 0 && col != columns - 1) 
                    {
                        temp[row, col] = variables[col].ToString();
                    }
                    //  filling equation cell
                    if(row == 0 && col == columns - 1) 
                    {
                        temp[row, col] = equation;
                    }
                    //  filling variables variation (from 000 to 111 for example)
                    if(row != 0 && col != columns - 1) 
                    {
                        // 100 -> "100"
                        string bStr = Convert.ToString(bCtr, 2);
                        // "100" -> "00100"
                        bStr = bStr.PadLeft(3, '0');
                        temp[row, col] = bStr[col].ToString();
                    }
                    //  filling results (ERROR)
                    if (row != 0 && col == columns - 1) 
                    {
                        //  Converting to binary
                        string bStr = Convert.ToString(bCtr, 2);
                        //      Bringing to expected format
                        if (bStr.Length != variables.Count) 
                        {
                            bStr = bStr.PadLeft(3, '0');
                        }
                        temp[row, col] = Convert.ToString(Simplify(bStr));
                    }
                    Console.Write(temp[row, col] + "|");
                }
                Console.Write("\n");
                //Increment the byte value (000 -> 001 -> 010 -> 011 etc.)
                if (row >= 1)
                    bCtr += 1;
            }
            return temp;
        }
    }
}
