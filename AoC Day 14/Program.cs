// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Drawing;

var input = File.ReadAllLines("input.txt");


foreach (var line in input)
{
    var points = new List<Point>();
    foreach (var point in line.Replace("->", "").Split(' ', StringSplitOptions.RemoveEmptyEntries))
    {
        points.Add(new Point(int.Parse(point.Split(',')[0]), int.Parse(point.Split(',')[1])));
    }
    for (int i = 0; i < line.Split("->").Length - 1; i++)
    {
        Globals.lines.Add(new Line(points[i], points[i + 1]));
    }
}
Globals.lines.Add(new Line(new Point(Globals.lines.Select(l => l.MinX()).Min(), Globals.lines.Select(l => l.MaxY()).Max() + 2), new Point(Globals.lines.Select(l => l.MaxX()).Max(), Globals.lines.Select(l => l.MaxY()).Max() + 2)));
Globals.slice = Extension.MakeSlice(Globals.lines);

Globals.sandList.Add(new Sand(new Point(500 - Globals.lines.Select(l => l.MinX()).Min(), 0)));
do
{
    var currentSand = Globals.sandList.Last();
    if (currentSand.falling)
    {
        try
        {
            if (currentSand.FallDown(ref Globals.slice))
            {
                Globals.sandList.RemoveAt(Globals.sandList.Count - 1);
                Globals.sandList.Add(currentSand);
            }
            else if (currentSand.FallLeft(Globals.slice))
            {
                Globals.sandList.RemoveAt(Globals.sandList.Count - 1);
                Globals.sandList.Add(currentSand);
            }
            else if (currentSand.FallRight(Globals.slice))
            {
                Globals.sandList.RemoveAt(Globals.sandList.Count - 1);
                Globals.sandList.Add(currentSand);
            }
            /*else
            {
                Debug.WriteLine("");
                Debug.WriteLine("You fucked up");
                Debug.WriteLine("");
                Extension.DrawSlice(Globals.slice);
                break;
            }*/
        }
        catch
        {
            if (currentSand.pos.x == 0)
            {
                var bottomLine = Globals.lines.Last();
                Globals.lines.RemoveAt(Globals.lines.Count - 1);
                Globals.sandList.RemoveAt(Globals.sandList.Count - 1);
                Globals.lines.Add(new Line(new Point(bottomLine.start.x - 1, bottomLine.start.y), new Point(bottomLine.end.x, bottomLine.end.y)));
                Globals.sandList.Add(new Sand(new Point(currentSand.pos.x + 1, currentSand.pos.y)));
                Globals.slice = Extension.WidenSlice(Globals.slice,true);
            }
            else
            {
                var bottomLine = Globals.lines.Last();
                Globals.lines.RemoveAt(Globals.lines.Count - 1);
                Globals.sandList.RemoveAt(Globals.sandList.Count - 1);
                Globals.lines.Add(new Line(new Point(bottomLine.start.x, bottomLine.start.y), new Point(bottomLine.end.x + 1, bottomLine.end.y)));
                Globals.sandList.Add(new Sand(new Point(currentSand.pos.x, currentSand.pos.y)));
                Globals.slice = Extension.WidenSlice(Globals.slice,false);
            }
        }
    }
    else
    {
        if ((currentSand.pos.x == 500 - Globals.lines.Select(l => l.MinX()).Min()) && (currentSand.pos.y == 0))
            break;
        else
        {
            //Debug.WriteLine("");
            //Extension.DrawSlice(Globals.slice);
            Globals.sandList.Add(new Sand(new Point(500 - Globals.lines.Select(l => l.MinX()).Min(), 0)));
        }
    }
} while (true);

Debug.WriteLine("");
Debug.WriteLine(String.Format("Dropped {0}", Globals.sandList.Count));
//Extension.DrawSlice(Globals.slice);
struct Point
{
    public int x { get; set; }
    public int y { get; set; }
    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
struct Line
{
    public Point start { get; set; }
    public Point end { get; set; }
    public bool horizontal;
    public Line(Point start, Point end)
    {
        this.start = start;
        this.end = end;
        if (start.y == end.y)
            horizontal = true;
        else
            horizontal = false;
    }
    public int MinX()
    {
        return (start.x < end.x ? start.x : end.x);
    }
    public int MaxX()
    {
        return (start.x > end.x ? start.x : end.x);
    }
    public int MinY()
    {
        return (start.y < end.y ? start.y : end.y);
    }
    public int MaxY()
    {
        return (start.y > end.y ? start.y : end.y);
    }
}
struct Sand
{
    public Point pos { get; set; }
    public bool falling { get; set; }
    public Sand(Point current)
    {
        this.pos = new Point(current.x, current.y);
        this.falling = true;
    }
    public bool FallDown(ref char[,] slice)
    {
        //Debug.WriteLine("Checking down");
        char[] under = new char[3] { slice[this.pos.x - 1, this.pos.y + 1], slice[this.pos.x, this.pos.y + 1], slice[this.pos.x + 1, this.pos.y + 1] };
        if (slice[this.pos.x, this.pos.y + 1] == '.')
        {
            this.pos = new Point(this.pos.x, this.pos.y + 1);
            return true;
        }
        else if (under.Where(c => c == 'O' || c == '#').Count() == 3)
        {
            this.falling = false;
            slice[this.pos.x, this.pos.y] = 'O';
            return true;
        }
        else
            return false;
    }
    public bool FallLeft(char[,] slice)
    {
        //Debug.WriteLine("Checking left");
        if ((slice[this.pos.x, this.pos.y + 1] == 'O' || slice[this.pos.x, this.pos.y + 1] == '#') && slice[this.pos.x - 1, this.pos.y + 1] == '.')
        {
            this.pos = new Point(this.pos.x - 1, this.pos.y + 1);
            return true;
        }
        else
            return false;
    }
    public bool FallRight(char[,] slice)
    {
        //Debug.WriteLine("Checking right");
        if (slice[this.pos.x + 1, this.pos.y + 1] == '.')
        {
            this.pos = new Point(this.pos.x + 1, this.pos.y + 1);
            return true;
        }
        else
            return false;
    }
}
class Extension
{
    public static char[,] MakeSlice(List<Line> lines)
    {
        int minX = lines.Select(l => l.MinX()).Min();
        int maxX = lines.Select(l => l.MaxX()).Max();
        int minY = lines.Select(l => l.MinY()).Min();
        int maxY = lines.Select(l => l.MaxY()).Max();
        int xoffset = maxX - minX + 1;
        char[,] slice = new char[xoffset, maxY + 1];
        for (int i = 0; i < slice.GetLength(0); i++)
        {
            for (int j = 0; j < slice.GetLength(1); j++)
            {
                slice[i, j] = '.';
            }
        }
        foreach (Line line in lines)
        {
            if (line.horizontal)
                if (line.start.x < line.end.x)
                    for (int i = (line.start.x - minX); i <= (line.end.x - minX); i++)
                        slice[i, line.start.y] = '#';
                else
                    for (int i = (line.end.x - minX); i <= (line.start.x - minX); i++)
                        slice[i, line.start.y] = '#';
            else
                if (line.start.y < line.end.y)
                for (int i = line.start.y; i <= line.end.y; i++)
                    slice[line.start.x - minX, i] = '#';
            else
                for (int i = line.end.y; i <= line.start.y; i++)
                    slice[line.start.x - minX, i] = '#';
        }
        slice[500 - minX, 0] = 'X';
        return slice;
    }
    public static void DrawSlice(char[,] slice)
    {
        string[] display = new string[] { };
        for (int i = 0; i < slice.GetLength(1); i++)
        {
            display = display.Append(String.Join("", Enumerable.Range(0, slice.GetLength(0)).Select(x => slice[x, i]).ToArray())).ToArray();
        }
        Debug.WriteLine(String.Join('\n', display));
    }
    public static char[,] WidenSlice(char[,] slice, bool side)
    {
        char[,] newSlice = new char[slice.GetLength(0) + 1, slice.GetLength(1)];
        for (int i = 0; i < newSlice.GetLength(0); i++)
            for (int j = 0; j < newSlice.GetLength(1); j++)
            {
                if (side)
                {
                    if (i == 0)
                    {
                        if (j == newSlice.GetLength(1) - 1)
                            newSlice[i, j] = '#';
                        else
                            newSlice[i, j] = '.';
                    }
                    else
                        newSlice[i, j] = slice[i - 1, j];
                }
                else
                {
                    if (i == newSlice.GetLength(0) - 1)
                    {
                        if (j == newSlice.GetLength(1) - 1)
                            newSlice[i, j] = '#';
                        else
                            newSlice[i, j] = '.';
                    }
                    else
                        newSlice[i, j] = slice[i, j];
                }
            }
        return newSlice;
    }
}
static class Globals
{
    public static List<Line> lines = new List<Line>();
    public static List<Sand> sandList = new List<Sand>();
    public static char[,] slice = new char[,] { };
}