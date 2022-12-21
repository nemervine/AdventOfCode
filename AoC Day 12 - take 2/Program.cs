// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

int loops = 0;
string[] input = File.ReadAllLines("input.txt");
char[,] map = new char[input[0].Length, input.Length];
var end = new Tile();
var startTiles = new Tile[] { };

for (int i = 0; i < input[0].Length; i++)
{
    for (int j = 0; j < input.Length; j++)
    {
        map[i, j] = input[j].ToCharArray()[i];
        if (map[i, j] == 'a')
        {
            var start = new Tile();
            start.X = i;
            start.Y = j;
            startTiles = startTiles.Append(start).ToArray();
        }
        /*if (map[i, j] == 'S')
        {
            var start = new Tile();
            start.X = i;
            start.Y = j;
            startTiles = startTiles.Append(start).ToArray(); //startTiles.Add(start);
            map[i, j] = 'a';
        }*/
        if (map[i, j] == 'E')
        {
            end.X = i;
            end.Y = j;
        }
    }
}

foreach (var tile in startTiles)
{
    //tile.Map = new char[,] { };
    tile.SetMap(map);
    tile.Z = 'a';
    tile.SetDistance(end.X, end.Y);
}

end.Map = map;

var activeTiles = startTiles.ToList<Tile>();
var visitedTiles = new List<Tile>();

while (activeTiles.Any())
{
    loops++;
    Debug.WriteLine(loops);
    var checkTile = activeTiles.OrderBy(x => x.CostDistance).First();
    checkTile.DisplayMap();
    if (checkTile.X == end.X && checkTile.Y == end.Y)
    {
        Debug.WriteLine("We are at the end");
        checkTile.DisplayMap();
        Debug.WriteLine("Steps taken: " + (checkTile.Cost-1));
        return;
        /*int steps = 0;
        while (true)
        {
            steps++;
            var tile = checkTile;
            tile = tile.Parent;
            
            if (tile == null)
            {
                
                return;
            }
        }*/
    }
    visitedTiles.Add(checkTile);
    activeTiles.Remove(checkTile);

    var walkableTiles = GetWalkableTiles(checkTile, end);

    foreach (var walkableTile in walkableTiles)
    {
        if (visitedTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
            continue;

        if (activeTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
        {
            var existingTile = activeTiles.First(x => x.X == walkableTile.X && x.Y == walkableTile.Y);
            if (existingTile.CostDistance > checkTile.CostDistance)
            {
                activeTiles.Remove(existingTile);
                activeTiles.Add(existingTile);
            }
        }
        else
            activeTiles.Add(walkableTile);
    }
}
Debug.WriteLine("No path found");

static List<Tile> GetWalkableTiles(Tile currentTile, Tile targetTile)
{
    var possibleTiles = new List<Tile>()
    {
        new Tile { X = currentTile.X, Y = currentTile.Y - 1, Parent = currentTile, Cost = currentTile.Cost + 1, Map = new char[currentTile.Map.GetLength(0),currentTile.Map.GetLength(1)] },
        new Tile { X = currentTile.X, Y = currentTile.Y + 1, Parent = currentTile, Cost = currentTile.Cost + 1, Map = new char[currentTile.Map.GetLength(0),currentTile.Map.GetLength(1)] },
        new Tile { X = currentTile.X - 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1, Map = new char[currentTile.Map.GetLength(0),currentTile.Map.GetLength(1)] },
        new Tile { X = currentTile.X + 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1, Map = new char[currentTile.Map.GetLength(0),currentTile.Map.GetLength(1)] },
    };
    var maxX = currentTile.Map.GetLength(0);
    var maxY = currentTile.Map.GetLength(1);

    possibleTiles.ForEach(tile => tile.SetDistance(targetTile.X, targetTile.Y));
    possibleTiles = possibleTiles.Where(tile => tile.X >= 0 && tile.X <= maxX).Where(tile => tile.Y >= 0 && tile.Y <= maxY).ToList();
    if (possibleTiles.Count > 0)
        possibleTiles.ForEach(tile => tile.SetZ());

    var returnTiles = possibleTiles.Where(t => (t.Z != '`' && ((currentTile.Z - 2) <= t.Z && t.Z <= (currentTile.Z + 1)) || (currentTile.Z == 'z' && t.Z == 'E'))).ToList();

    return returnTiles;
}
class Tile
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }
    public char[,] Map { get; set; }
    public int Cost { get; set; }
    public int Distance { get; set; }
    public int CostDistance => Cost + Distance;
    public Tile Parent { get; set; }

    public void SetDistance(int targetX, int targetY)
    {
        this.Distance = Math.Abs(targetX - X) + Math.Abs(targetY - Y);
    }
    public void SetZ()
    {
        Array.Copy(this.Parent.Map, this.Map, this.Parent.Map.Length);
        try
        {
            if (this.Parent.Y == this.Y)
            {
                if (this.X < this.Parent.X)
                {
                    this.Map[this.Parent.X, this.Parent.Y] = '<';
                }
                else if (this.X > this.Parent.X)
                {
                    this.Map[this.Parent.X, this.Parent.Y] = '>';
                }
            }
            else if (this.Parent.X == this.X)
            {
                if (this.Parent.Y < this.Y)
                {
                    this.Map[this.Parent.X, this.Parent.Y] = 'V';
                }
                else if (this.Parent.Y > this.Y)
                {
                    this.Map[this.Parent.X, this.Parent.Y] = '^';
                }
            }
            this.Z = this.Map[this.X, this.Y];
            this.Map[this.X, this.Y] = 'S';
        }
        catch
        {
            this.Z = '`';
        }
    }
    public void SetMap(char[,] map)
    {
        this.Map = new char[map.GetLength(0),map.GetLength(1)];
        Array.Copy(map, this.Map, map.Length);
        this.Z = 'a';
        this.Map[this.X, this.Y] = 'S';
    }
    public void DisplayMap()
    {
        Debug.WriteLine("");
        string[] display = new string[] { };
        for (int i = 0; i < this.Map.GetLength(1); i++)
        {
            display = display.Append(String.Join("", Enumerable.Range(0, this.Map.GetLength(0)).Select(x => this.Map[x, i]).ToArray())).ToArray();
        }
        Debug.WriteLine(String.Join('\n', display));
        Debug.WriteLine("");
    }
}