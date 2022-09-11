using RiskCalculator;

// See https://aka.ms/new-console-template for more information

Calculator calc = new Calculator();

string accountCurrency = args[0].ToUpper();
string pair = args[1].ToUpper();
double pips = double.Parse(args[2]);
double risk = calc.CalcRisk(accountCurrency, pair, pips, double.Parse(args[3]));
double accountSize = double.Parse(args[4]);
Console.WriteLine($"Risk for {pair} {pips} pips is: {risk.ToString("F2")}{accountCurrency} A/C risk: {((risk/accountSize)*100).ToString("F2")}%");

// Console.ReadLine();