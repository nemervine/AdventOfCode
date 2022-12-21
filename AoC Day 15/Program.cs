string[] input;
int row;
bool testing = false;
int minX = 0;
int minY = 0;
int maxX = 4000000;
int maxY = 4000000;
int multiplier = 4000000;


if (testing)
{
    input = File.ReadAllLines("test.txt");
    row = 10;
    maxX = 20;
    maxY = 20;
}
else
{
    input = File.ReadAllLines("input.txt");
    row = 2000000;
}

var sensors = new List<Sensor>();

foreach (string line in input)
{
    var senx = int.Parse(line.Split(':')[0].Split(",")[0].Split("=")[1]);
    var seny = int.Parse(line.Split(':')[0].Split(",")[1].Split("=")[1]);
    var beax = int.Parse(line.Split(':')[1].Split(",")[0].Split("=")[1]);
    var beay = int.Parse(line.Split(':')[1].Split(",")[1].Split("=")[1]);
    sensors.Add(new(new(senx, seny), new(beax, beay)));
}

//for (int i = minY; i <= maxY; i++)
var yRange = Enumerable.Range(minY, maxY + 1).OrderBy(y => Math.Abs(y - (maxY - minY) / 2));
int done = 0;


var limit = new Range(minY, maxY);
var start = DateTime.Now;
Console.WriteLine(start);
Parallel.ForEach(yRange.AsParallel().AsOrdered(), (i, state) =>
{
    var covered = sensors.Select(s => s.GetSlice(i));
    var gap = FindGap(covered, limit);
    if (gap is int x)
    {
        var end = DateTime.Now;
        Console.WriteLine(end);
        Console.WriteLine(end - start);
        Console.WriteLine((long)x * multiplier + i);
        state.Stop();
        return;
    }
    done++;
    //Console.WriteLine(Math.Round((decimal)done / yRange.Count() * 100, 4) + "%");
});

static int? FindGap(IEnumerable<Range> ranges, Range limit)
{
    var ordered = ranges.Select(r => r.Intersect(limit))
                        .Where(r => !r.IsEmpty)
                        .OrderBy(r => r.Min)
                        .ThenBy(r => r.Max);

    int max = limit.Min - 1;
    foreach (var r in ordered)
    {
        if (max + 1 < r.Min)
            return max + 1;

        max = Math.Max(max, r.Max);
    }

    return max < limit.Max
      ? max + 1
      : null;
}
record struct Point(int X, int Y)
{
    public int Distance(Point other)
    {
        return Math.Abs(other.X - X) + Math.Abs(other.Y - Y);
    }
}
record struct Range(int Min, int Max)
{
    public static Range Empty = new(0, -1);

    public bool IsEmpty => Min > Max;

    public IEnumerable<int> Values
      => IsEmpty ? Enumerable.Empty<int>() : Enumerable.Range(Min, Max - Min + 1);

    public bool Overlaps(Range other)
      => !IsEmpty
      && !other.IsEmpty
      && Min <= other.Max
      && Max >= other.Min;

    public Range Intersect(Range other)
      => Overlaps(other) ? new(Math.Max(Min, other.Min), Math.Min(Max, other.Max)) : Empty;
}
record Sensor(Point Position, Point ClosestBeacon)
{
    public int BeaconDistance { get; } = Position.Distance(ClosestBeacon);

    public Range GetSlice(int y)
    {
        var dy = Math.Abs(y - Position.Y);
        if (dy > BeaconDistance)
            return Range.Empty;

        var dx = BeaconDistance - dy;
        return new(Position.X - dx, Position.X + dx);
    }

    /*    public static Sensor Parse(string line)
        {
            var parts = line.Split(' ');

            var sensorPos = new Point(
              int.Parse(parts[2][2..^1]),
              int.Parse(parts[3][2..^1]));

            var beaconPos = new Point(
              int.Parse(parts[8][2..^1]),
              int.Parse(parts[9][2..]));

            return new(sensorPos, beaconPos);
        }*/
}