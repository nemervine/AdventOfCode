using System.Text.RegularExpressions;

var start = DateTime.Now;
var input = File.ReadAllLines("input.txt");
var monkeyList = new List<Monkey>();

foreach (var line in input)
{
    var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    if (parts.Length == 2)
        monkeyList.Add(new(parts[0].Replace(":", ""), double.Parse(parts[1])));
    else
        monkeyList.Add(new(parts[0].Replace(":", ""), parts[1], parts[3], parts[2][0]));
}

Console.WriteLine(Yell((monkeyList.Find(m => m.Name == "root"))));
var part1 = DateTime.Now;
Console.WriteLine((part1 - start).TotalMilliseconds);

var left = GetEquation(monkeyList.Find(m => m.Name == monkeyList.Find(m => m.Name == "root").Monkey1));
var right = Eval(GetEquation(monkeyList.Find(m => m.Name == monkeyList.Find(m => m.Name == "root").Monkey2)));
Regex eqStart = new Regex(@"^[(]\d+[.]0(?:[+]|[-]|[*]|[\/])");
Regex eqEnd = new Regex(@"(?:[+]|[-]|[*]|[\/])\d+[.]0[)]$");

while (!left.Equals("humn"))
{
    var match = eqStart.Match(left);
    if (match.Success)
    {
        var factor = long.Parse(match.Value.Substring(1, match.Length - 4));
        left = left.Remove(match.Index, match.Length);
        left = left.Remove(left.Length - 1, 1);
        switch (match.Value.ToCharArray()[match.Value.Length - 1])
        {
            case '+':
                right = (right - factor);
                break;
            case '-':
                right = (factor - right);
                break;
            case '*':
                right = (right / factor);
                break;
            case '/':
                right = (right * factor);
                break;
        }
    }
    else
    {
        match = eqEnd.Match(left);
        var factor = long.Parse(match.Value.Substring(1, match.Length - 4));
        left = left.Remove(match.Index, match.Length);
        left = left.Remove(0, 1);
        switch (match.Value.ToCharArray()[0])
        {
            case '+':
                right = (right - factor);
                break;
            case '-':
                right = (right + factor);
                break;
            case '*':
                right = (right / factor);
                break;
            case '/':
                right = (right * factor);
                break;
        }
    }

}

Console.WriteLine(right);
var part2 = DateTime.Now;
Console.WriteLine((part2 - start).TotalMilliseconds);

double Yell(Monkey monkey)
{
    if (monkey.Type == "num")
    {
        return (double)monkey.Num;
    }
    else if (monkey.Type == "job")
        switch (monkey.Job)
        {
            case '+':
                return Yell(monkeyList.Find(m => m.Name == monkey.Monkey1)) + Yell(monkeyList.Find(m => m.Name == monkey.Monkey2));
            case '-':
                return Yell(monkeyList.Find(m => m.Name == monkey.Monkey1)) - Yell(monkeyList.Find(m => m.Name == monkey.Monkey2));
            case '*':
                return Yell(monkeyList.Find(m => m.Name == monkey.Monkey1)) * Yell(monkeyList.Find(m => m.Name == monkey.Monkey2));
            case '/':
                return Yell(monkeyList.Find(m => m.Name == monkey.Monkey1)) / Yell(monkeyList.Find(m => m.Name == monkey.Monkey2));
        }
    return 0;
}
string GetEquation(Monkey monkey)
{
    if (monkey.Name == "humn")
        return "humn";
    if (monkey.Type == "num")
        return monkey.Num.ToString() + ".0";
    else if (monkey.Type == "job")
    {
        var monkey1 = GetEquation(monkeyList.Find(m => m.Name == monkey.Monkey1));
        if (!monkey1.Contains("humn"))
            monkey1 = Eval(monkey1).ToString() + ".0";
        var monkey2 = GetEquation(monkeyList.Find(m => m.Name == monkey.Monkey2));
        if (!monkey2.Contains("humn"))
            monkey2 = Eval(monkey2).ToString() + ".0";
        return String.Format("({0}{2}{1})", monkey1, monkey2, monkey.Job);
    }
    return "";
}
static double Eval(String expression)
{
    System.Data.DataTable table = new System.Data.DataTable();
    table.Columns.Add("expression", ((double)0).GetType(), expression);
    System.Data.DataRow row = table.NewRow();
    table.Rows.Add(row);
    return ((double)row["expression"]);
}
record struct Monkey
{
    public string Name { get; set; }
    public string Type { get; set; }
    public double Num { get; set; }
    public string? Monkey1 { get; set; }
    public string? Monkey2 { get; set; }
    public char? Job { get; set; }
    public Monkey(string name, double Number)
    {
        Name = name;
        Type = "num";
        Num = Number;
    }
    public Monkey(string name, string monkey1, string monkey2, char job)
    {
        Name = name;
        Type = "job";
        Num = 0;
        Monkey1 = monkey1;
        Monkey2 = monkey2;
        Job = job;
    }
}