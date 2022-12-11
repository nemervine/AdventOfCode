// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

string inputText = File.ReadAllText("input.txt");
Monkey[] monkeys = new Monkey[inputText.Split("Monkey").Length-1];

for (int i = 0; i < monkeys.Length; i++)
{
    string[] monkeyText = inputText.Split(Environment.NewLine.ToArray(),StringSplitOptions.RemoveEmptyEntries);
    string[] monkeyRows = new string[] { };
    for (int j=0; j<5; j++)
    {
        monkeyRows = monkeyRows.Append(monkeyText[i*6 + j+1]).ToArray();
    }
    monkeys[i] = new Monkey(monkeyRows);
}

int moduloTrick = 1;
for (int i = 0; i < monkeys.Length; i++)
{
    moduloTrick *= monkeys[i].test;
}

for (int r = 0; r < 10000; r++)
{
    //Debug.WriteLine("Round: "+r);
    for (int m = 0; m < monkeys.Length; m++)
    {
        //Debug.WriteLine("Monkey " + m + " starting items: " + string.Join(',', monkeys[m].items));
        for (int i = 0; i < monkeys[m].items.Length; i++)
        {
            monkeys[m].inspections=monkeys[m].inspections + 1;
            double tempNum=0;
            if (monkeys[m].operationNum == -1)
                tempNum = monkeys[m].items[i];
            else
                tempNum = monkeys[m].operationNum;
            switch (monkeys[m].operation)
            {
                case '+':
                    monkeys[m].items[i] = monkeys[m].items[i] + tempNum;
                    break;
                case '*':
                    monkeys[m].items[i] = monkeys[m].items[i] * tempNum;
                    break;
            }
            monkeys[m].items[i] = monkeys[m].items[i] % moduloTrick;
            double result = (monkeys[m].items[i] % monkeys[m].test);
            if ( result == 0.0)
                monkeys[monkeys[m].testTrue].items = monkeys[monkeys[m].testTrue].items.Append(monkeys[m].items[i]).ToArray();
            else
                monkeys[monkeys[m].testFalse].items = monkeys[monkeys[m].testFalse].items.Append(monkeys[m].items[i]).ToArray();
        }
        monkeys[m].items = new double[] { };
        //Debug.WriteLine("Monkey "+m+" inspections: " + monkeys[m].inspections);
    }
}
Debug.WriteLine("Monkey 0: " + (monkeys[0].inspections));
Debug.WriteLine("Monkey 1: " + (monkeys[1].inspections));
Debug.WriteLine("Monkey 2: " + (monkeys[2].inspections));
Debug.WriteLine("Monkey 3: " + (monkeys[3].inspections));
monkeys = (from monkey in monkeys orderby monkey.inspections descending select monkey).ToArray();
Debug.WriteLine("");
Debug.WriteLine("Monkey business: " + (monkeys[0].inspections * monkeys[1].inspections));
public struct Monkey
{
    public double[] items;
    public char operation;
    public int operationNum;
    public int test;
    public int testTrue;
    public int testFalse;
    public long inspections=0;

    public Monkey(string[] monkeyRows)
    {
        items = Array.ConvertAll(monkeyRows[0].Replace("Starting items: ", "").Split(",",StringSplitOptions.TrimEntries),s=>(double)double.Parse(s));
        operation = monkeyRows[1].Replace("  Operation: new = old ", "").ToCharArray()[0];
        if (monkeyRows[1].EndsWith("old"))
            operationNum = -1;
        else
            operationNum = Int32.Parse(monkeyRows[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Last());
        test = Int32.Parse(monkeyRows[2].Split(" ").Last());
        testTrue = Int32.Parse(monkeyRows[3].Split(" ").Last());
        testFalse = Int32.Parse(monkeyRows[4].Split(" ").Last());
    }
}