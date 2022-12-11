// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using static AoC_Day_9.Coords;
using static AoC_Day_9.CoordsComparer;
using static AoC_Day_9.RopeKnot;
using static AoC_Day_9.Rope;

string[] inputText = System.IO.File.ReadAllLines(Environment.CurrentDirectory + "\\input.txt");
AoC_Day_9.Rope rope = new AoC_Day_9.Rope(10);
foreach(string line in inputText)
{
    char dir = char.Parse(line.Split(" ", StringSplitOptions.RemoveEmptyEntries)[0]);
    int spaces = Int32.Parse(line.Split(" ", StringSplitOptions.RemoveEmptyEntries)[1]);
    //Debug.WriteLine(line);
    rope.Move(dir,spaces);
}
for (int i = 0; i < rope.knots.Count(); i++)
{
    /*Debug.WriteLine("Knot "+i+" Positions visited: " + rope.knots[i].history.Count());
    Debug.WriteLine("Knot " + i + " Unique Positions visited: " + rope.knots[i].history.Distinct(new AoC_Day_9.CoordsComparer()).Count());*/
    rope.Visualize(0);
}

