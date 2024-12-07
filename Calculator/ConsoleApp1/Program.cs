using CalculatorLib;

//Operations: +, *, ¬, O, →, ~, ↓, ↑
Calculator calculator = new Calculator();
calculator.InitVariables("(x→y)~(y→z)");
calculator.InitTruthTable();
Console.WriteLine(calculator.GetPDNF());
Console.WriteLine(calculator.GetPCNF());