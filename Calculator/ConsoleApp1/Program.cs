using CalculatorLib;

//Operations: +, *, ¬, O, →, ~, ↓, ↑
Calculator calculator = new Calculator();
calculator.InitVariables("(k→r)O(x~r)");
calculator.InitTruthTable();
Console.WriteLine(calculator.GetPDNF());
Console.WriteLine(calculator.GetPCNF());
Console.WriteLine(calculator.GetMDNF());