using System.Windows.Markup;

namespace CalculatorLib
{
    public class Calculator
    {
        //Operations:∨, ∧, ¬, ⊕, →, ~, ↓, ↑
        //Operations: +, *, ¬, O, →, ~, ↓, ↑
        //Example: (x → y) ~ (y → z)
        string equation = "(x→y)~(y→z)";
        char[] varLetters = new char[] {'x', 'y', 'z' };
        List<char> variables;
        string[,] truthTable;

        public bool CheckBrackets(string eq) 
        {
            //Method is used to check the correctness of the equation brackets
            int balance = 0;
            foreach(char letter in eq) 
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
        public List<char> InitVariables(string eq) 
        {
            //Method is used to find what variables are in equation
            List<char> temp = new List<char>();
            foreach (char letter in eq) 
            {
                if(varLetters.Contains(letter))
                    temp.Add(letter);
            }
            temp.Sort();
            variables = new List<char>(temp);
            return temp;
        }
        public string GetPartialResult(string simpleEq) 
        {
            //Example of what we sent
            //  0→1
        }
        public string GetRidOfBrackets(string eq) 
        {
            string eqTemp = eq;
            while (eqTemp.Contains('(')) 
            {
                //Indexes of the deepest brackets "(" and ")"
                int start = eq.LastIndexOf(')');
                int end = eq.IndexOf('(', start);
            }

 
        }
        public int GetTotalResult(string valuesOfVariables) 
        {
            string equationReplaced = equation;
            //Replacing variables with their values
            for(int i = 0; i < variables.Count; ++i) 
            {
                //Clearing equation from spacebars
                equationReplaced.Replace(" ", "");
                //Replace var with value (X is 0 | Y is 1 etc.)
                equationReplaced.Replace(variables[i], valuesOfVariables[i]);
            }
            //Example of what we got
            //  (1→0)~(0→0)
            //Now we must get rid of brackets

        }
        public string[,] InitTruthTable() 
        {
            //Method is used to create a truth table
            //One row is for header
            int rows = 1 + (1 * (int)Math.Pow(2, variables.Count));
            //One column is for result
            int columns = variables.Count + 1;


            string[,] temp = new string[rows, columns];
            byte bCtr = 000;

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
                        string bStr = Convert.ToString(bCtr, 2);
                        temp[row, col] = bStr[col].ToString();
                    }
                    //  filling results
                    if (row != 0 && col == columns - 1) 
                    {
                        string bStr = Convert.ToString(bCtr, 2);
                        temp[row, col] = Convert.ToString(GetTotalResult(bStr));
                    }
                    //Increment the byte value (000 -> 001 -> 010 -> 011 etc.)
                    ++bCtr;
                }
            }
            return temp;
        }
        //*
        public string Conjunction() {}
        //¬
        public string Negation() { }
        //O
        public string XOR() { }
        //→
        public string Implication() { }
        //~
        public string Equivalence() { }
        //↓
        public string NOR() { }
        //↑
        public string NAND() { }
    }
}
