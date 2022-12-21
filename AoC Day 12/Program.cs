// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Globalization;
using System.Linq;
using AoC_Day_12;

int loops = 0;
string[] input = File.ReadAllLines("input.txt");
char[,] map = new char[input[0].Length, input.Length];
//string[] loopingMoves = { "lrl", "rlr", "udu", "dud" };
var paths = new Pathing[] { };
var tempPath = new Pathing();
var me = new Position();
var end = new Position();

for (int i = 0; i < input[0].Length; i++)
{
    for (int j = 0; j < input.Length; j++)
    {
        map[i, j] = input[j].ToCharArray()[i];
        if (map[i, j] == 'S')
            me = new Position(i, j);
        if (map[i, j] == 'E')
            end = new Position(i, j);
    }
}
tempPath.map = new char[input[0].Length, input.Length];
Array.Copy(map, tempPath.map, map.Length);
tempPath.me = me;
tempPath.end = end;
paths = paths.Append(tempPath).ToArray();
List<Pathing> finalPaths = new List<Pathing> { };

do
{
    loops++;
    Debug.WriteLine("Step " + loops);
    var tempPaths = new List<Pathing> { };
    foreach (Pathing path1 in paths)
    {
       /* int left = path.GetLefts();
        int right = path.GetRights();
        int up = path.GetUps();
        int down = path.GetDowns();*/
        if (tempPaths.Where(p => p.path.OrderBy(c => c) == path1.path.OrderBy(c=>c)).Count() == 0)
            tempPaths.Add(path1);
    }
    //var aveDist = tempPaths.Average(x => x.GetDist());
    //tempPaths = (from temp in tempPaths where (temp.GetDist() <= aveDist * 1.5) select temp).OrderBy(p => p.GetDist()).ThenByDescending(p => p.me.x).ToList();
    tempPaths = tempPaths.OrderBy(p => p.GetDist()).ThenByDescending(p => p.path.Length).ToList();
    var path = tempPaths[0];
    Debug.WriteLine(String.Format("Exploring {0} paths", tempPaths.Count()));
    Mapping.DisplayMap(tempPaths[0].map);
    var newPaths = new List<Pathing> { };
    //foreach (Pathing path in tempPaths)
    //ForEach(tempPaths, path =>
        var parallelPath = new Pathing(path);
        //int i = Array.IndexOf(paths, path);
        if (path.exploring)
        {
            //Debug.WriteLine("Exploring path: " + i);
            double[] distance = new double[4] { -1, -1, -1, -1 };
            //double dist = Math.Sqrt(Math.Pow(path.end.x - (path.me.x), 2) + Math.Pow(path.end.y - path.me.y, 2));
            if (0 <= path.me.x - 1)
                if ((path.map[path.me.x - 1, path.me.y] >= 'b' && path.map[path.me.x - 1, path.me.y] <= path.me.z + 1))
                    distance[0] = 1;//Math.Sqrt(Math.Pow(path.end.x - (path.me.x-1), 2) + Math.Pow(path.end.y - path.me.y, 2));
                else if (path.me.z == 'z' && path.map[path.me.x - 1, path.me.y] == 'E')
                    distance[0] = 1;stagdevop
            if (path.me.x + 1 < map.GetLength(0))
                if ((path.map[path.me.x + 1, path.me.y] >= 'b' && path.map[path.me.x + 1, path.me.y] <= path.me.z + 1))
                    distance[1] = 1;//Math.Sqrt(Math.Pow(path.end.x - (path.me.x+1), 2) + Math.Pow(path.end.y - path.me.y, 2));
                else if (path.me.z == 'z' && path.map[path.me.x + 1, path.me.y] == 'E')
                    distance[1] = 1;
            if (0 <= path.me.y - 1)
                if ((path.map[path.me.x, path.me.y - 1] >= 'b' && path.map[path.me.x, path.me.y - 1] <= path.me.z + 1))
                    distance[2] = 1;//Math.Sqrt(Math.Pow(path.end.x - (path.me.x), 2) + Math.Pow(path.end.y - path.me.y -1, 2));
                else if (path.me.z == 'z' && path.map[path.me.x, path.me.y - 1] == 'E')
                    distance[2] = 1;
            if (path.me.y + 1 < map.GetLength(1))
                if ((path.map[path.me.x, path.me.y + 1] >= 'b' && path.map[path.me.x, path.me.y + 1] <= path.me.z + 1))
                    distance[3] = 1;//Math.Sqrt(Math.Pow(path.end.x - (path.me.x), 2) + Math.Pow(path.end.y - path.me.y +1, 2));
                else if (path.me.z == 'z' && path.map[path.me.x, path.me.y + 1] == 'E')
                    distance[3] = 1;
            if (distance == new double[4] { -1, -1, -1, -1 })
            {
                if ((path.end.y - path.me.y) > (path.end.x - path.me.x))
                {
                    if (path.me.x > path.end.x)
                    {
                        distance[0] = 1;
                    }
                    else
                    {
                        distance[1] = 1;
                    }
                }
                else
                {
                    if (path.me.y > path.end.y)
                    {
                        distance[2] = 1;
                    }
                    else
                    {
                        distance[3] = 1;
                    }
                }
            }
            List<double> newDistance = new List<double>();
            /*if ((from dir in distance where (dir > -1 && dir <= (dist)) select dir).Count() > 0)
                newDistance = (from dir in distance where ((dir > -1) && (dir < dist)) select dir).ToList();
            else*/
            newDistance = (from dir in distance where (dir > 0) select dir).ToList();
            //Mapping.DisplayMap(parallelPath.map);
            for (int j = 0; j < newDistance.Count(); j++)
            {
                var newPath = new Pathing(parallelPath);
                //paths = Pathing.AddPath(paths, path, out newIndex);
                switch (Array.IndexOf(distance, newDistance[j]))
                {
                    case 0:
                        //Debug.WriteLine("Moving left");
                        //paths[newIndex].MoveLeft();
                        newPath.MoveLeft();
                        if (newPath.me.z == 'E')
                        {
                            newPath.success = true;
                            newPath.exploring = false;
                            finalPaths.Add(newPath);
                        }
                        newPaths.Add(newPath);
                        break;
                    case 1:
                        //Debug.WriteLine("Moving right");
                        //paths[newIndex].MoveRight();
                        newPath.MoveRight();
                        if (newPath.me.z == 'E')
                        {
                            newPath.success = true;
                            newPath.exploring = false;
                            finalPaths.Add(newPath);
                        }
                        newPaths.Add(newPath);
                        break;
                    case 2:
                        //Debug.WriteLine("Moving up");
                        //paths[newIndex].MoveUp();
                        newPath.MoveUp();
                        if (newPath.me.z == 'E')
                        {
                            newPath.success = true;
                            newPath.exploring = false;
                            finalPaths.Add(newPath);
                        }
                        newPaths.Add(newPath);
                        break;
                    case 3:
                        //Debug.WriteLine("Moving down");
                        //paths[newIndex].MoveDown();
                        newPath.MoveDown();
                        if (newPath.me.z == 'E')
                        {
                            newPath.success = true;
                            newPath.exploring = false;
                            finalPaths.Add(newPath);
                        }
                        newPaths.Add(newPath);
                        break;
                }
                distance[Array.IndexOf(distance, newDistance[j])] = -1;
                //Mapping.DisplayMap(path.map);
            }
        }
    }
    //Debug.WriteLine("");
    //Debug.WriteLine(paths.Length);
    paths = null;
    paths = newPaths.Where(p => p.exploring).OrderBy(p => p.GetDist()).ToArray();
    //Debug.WriteLine(paths.Length);
} while (paths.Count() > 0 && finalPaths.Count() == 0);
Debug.WriteLine("");
if (finalPaths.Count() == 0)
{
    Debug.WriteLine("");
    Debug.WriteLine("No paths found");
}
else
{
    Debug.WriteLine("");
    Debug.WriteLine(String.Format("Lowest steps: {0}", finalPaths.Min(path => path.path.Length)));
}

public struct Position
{
    public int x;
    public int y;
    public int z = 'a';
    public Position() { }
    public Position(int x_, int y_)
    {
        x = x_;
        y = y_;
    }
}
public struct Pathing
{
    public string path = "";
    public bool looped = false;
    public bool exploring = true;
    public bool success = false;
    public Position me = new Position();
    public Position end = new Position();
    public char[,] map = new char[,] { };
    public Pathing()
    {
    }
    public Pathing(Pathing path_)
    {
        this.path = new string(path_.path);
        this.looped = false;
        this.me = new Position(path_.me.x, path_.me.y);
        this.end = path_.end;
        this.map = new char[path_.map.GetLength(0), path_.map.GetLength(1)];
        Array.Copy(path_.map, this.map, path_.map.Length);
        this.success = path_.success;
        this.exploring = path_.exploring;
    }
    public Pathing(string path_, Position me_, Position end_, char[,] map_)
    {
        this.path = new string(path_);
        this.looped = false;
        this.me = new Position(me_.x, me_.y);
        this.end = end_;
        this.map = new char[map_.GetLength(0), map_.GetLength(1)];
        Array.Copy(map_, map, map_.Length);
        this.success = false;
        this.exploring = true;
    }
    public void MoveLeft()
    {
        this.map[me.x, me.y] = '<';
        this.me.x--;
        this.me.z = map[me.x, me.y];
        this.path = path + 'l';
        this.map[me.x, me.y] = 'S';
    }
    public void MoveRight()
    {
        this.map[me.x, me.y] = '>';
        this.me.x++;
        this.me.z = map[me.x, me.y];
        this.path = path + 'r';
        this.map[me.x, me.y] = 'S';
    }
    public void MoveUp()
    {
        this.map[me.x, me.y] = '^';
        this.me.y--;
        this.me.z = map[me.x, me.y];
        this.path = path + 'u';
        this.map[me.x, me.y] = 'S';
    }
    public void MoveDown()
    {
        this.map[me.x, me.y] = 'V';
        this.me.y++;
        this.me.z = map[me.x, me.y];
        this.path = path + 'd';
        this.map[me.x, me.y] = 'S';
    }
    public int GetLefts()
    {
        return this.path.Split('l').Length;
    }
    public int GetRights()
    {
        return this.path.Split('r').Length;
    }
    public int GetUps()
    {
        return this.path.Split('u').Length;
    }
    public int GetDowns()
    {
        return this.path.Split('d').Length;
    }
    public double GetDist()
    {
        return Math.Sqrt(Math.Pow(this.end.x - this.me.x, 2) + Math.Pow(this.end.y - this.me.y, 2));
    }
    public static Pathing[] AddPath(Pathing[] paths, Pathing path)
    {
        var tempPaths = new List<Pathing>(paths);
        tempPaths.Add(new Pathing(path));
        //newIndex = tempPaths.Count-1;
        return tempPaths.ToArray();
    }
}
public class PositionComparer : IEqualityComparer<Position>
{
    public bool Equals(Position position1, Position position2)
    {
        return position1.x == position2.x && position1.y == position2.y;
    }
    public int GetHashCode(Position position)
    {
        return position.x.GetHashCode() ^ position.y.GetHashCode();
    }
}
public class Mapping
{
    public static void DisplayMap(char[,] map_)
    {
        string[] line = new string[] { };
        Debug.WriteLine("");
        for (int i = 0; i < map_.GetLength(1); i++)
        {
            /*for (int j = 0; j< map_.GetLength(0); j++)
            {
                Debug.Write(map_[j,i]);
            }*/
            line = line.Append(String.Join("", Enumerable.Range(0, map_.GetLength(0)).Select(x => map_[x, i]).ToArray())).ToArray();
        }
        Debug.WriteLine(String.Join('\n', line));
        Debug.WriteLine("");
    }
}