var input = File.ReadAllText("input.txt").ToCharArray();
var start = DateTime.Now;
var width = 7;
var rocks = 1000000000000;
var spawnX = 2;
var spawnY = 3;

var rockTypes = new List<Points[]>
{
    new Points[] { new Points(0, 0), new Points(1, 0), new Points(2, 0), new Points(3, 0) },
    new Points[] { new Points(0, 1), new Points(1, 0), new Points(1, 1), new Points(1, 2), new Points(2, 1) },
    new Points[] { new Points(0, 0), new Points(1, 0), new Points(2, 0), new Points(2, 1), new Points(2, 2) },
    new Points[] { new Points(0, 0), new Points(0, 1), new Points(0, 2), new Points(0, 3) },
    new Points[] { new Points(0, 0), new Points(0, 1), new Points(1, 0), new Points(1, 1) }
};

int rockCounter = 0;
int jetCounter = 0;
int maxHeight = 0;
var settledList = new List<(Points settled, int type)>();
bool movedSide;
bool movedDown;
Rock currentRock;
var cycles = new List<(long dropped, int rockType, int jetCounter, long height0, long height1, long height2, long height3, long height4, long height5, long height6)>();
long setupHeight = 0;
long repeatHeight = 0;
long repeatInterval = 0;
long repeatCount = 0;
bool cycleFound = false;

for (long i = 0; i < rocks; i++)
{
    var startJet = jetCounter;
    currentRock = new Rock(spawnX, spawnY + maxHeight, rockCounter, true);
    while (currentRock.Falling)
    {
        movedSide = false;
        movedDown = false;
        if (input[jetCounter] == '<')
            TryLeft(currentRock);
        if (input[jetCounter] == '>')
            TryRight(currentRock);
        jetCounter++;
        if (jetCounter == input.Length)
            jetCounter = 0;
        TryDown(currentRock);
        if (!movedDown)
        {
            currentRock.Falling = false;
            foreach (var point in rockTypes[currentRock.RockType])
                settledList.Add((new(point.X + currentRock.X, point.Y + currentRock.Y), rockCounter));
            if (settledList.Count != settledList.Distinct().Count())
                return;
        }
    }
    maxHeight = settledList.Max(p => p.settled.Y) + 1;
    cycles.Add((i + 1, rockCounter, startJet,
        (settledList.Where(p => p.settled.X == 0).Any()) ? settledList.Where(p => p.settled.X == 0).Max(p => p.settled.Y) + 1 : 0,
        (settledList.Where(p => p.settled.X == 1).Any()) ? settledList.Where(p => p.settled.X == 1).Max(p => p.settled.Y) + 1 : 0,
        (settledList.Where(p => p.settled.X == 2).Any()) ? settledList.Where(p => p.settled.X == 2).Max(p => p.settled.Y) + 1 : 0,
        (settledList.Where(p => p.settled.X == 3).Any()) ? settledList.Where(p => p.settled.X == 3).Max(p => p.settled.Y) + 1 : 0,
        (settledList.Where(p => p.settled.X == 4).Any()) ? settledList.Where(p => p.settled.X == 4).Max(p => p.settled.Y) + 1 : 0,
        (settledList.Where(p => p.settled.X == 5).Any()) ? settledList.Where(p => p.settled.X == 5).Max(p => p.settled.Y) + 1 : 0,
        (settledList.Where(p => p.settled.X == 6).Any()) ? settledList.Where(p => p.settled.X == 6).Max(p => p.settled.Y) + 1 : 0));
    rockCounter++;
    if (rockCounter == rockTypes.Count)
        rockCounter = 0;
    if (!cycleFound)
    {
        //Console.WriteLine("Checking for cycle");
        foreach (var drop in cycles)
        {
            var temp = cycles.Where(p => p.rockType == drop.rockType && p.jetCounter == drop.jetCounter).ToList();
            if (temp.Count > 2)
            {
                repeatInterval = temp.Last().dropped - temp[temp.Count-2].dropped;

                var height0 = temp.Where(t => temp.IndexOf(t) > 0).Select(t => t.height0 - temp[temp.IndexOf(t) - 1].height0);
                var height1 = temp.Where(t => temp.IndexOf(t) > 0).Select(t => t.height1 - temp[temp.IndexOf(t) - 1].height1);
                var height2 = temp.Where(t => temp.IndexOf(t) > 0).Select(t => t.height2 - temp[temp.IndexOf(t) - 1].height2);
                var height3 = temp.Where(t => temp.IndexOf(t) > 0).Select(t => t.height3 - temp[temp.IndexOf(t) - 1].height3);
                var height4 = temp.Where(t => temp.IndexOf(t) > 0).Select(t => t.height4 - temp[temp.IndexOf(t) - 1].height4);
                var height5 = temp.Where(t => temp.IndexOf(t) > 0).Select(t => t.height5 - temp[temp.IndexOf(t) - 1].height5);
                var height6 = temp.Where(t => temp.IndexOf(t) > 0).Select(t => t.height6 - temp[temp.IndexOf(t) - 1].height6);

                repeatHeight = height0.Last();
                repeatCount = (rocks - i) / repeatInterval;
                i += (repeatCount * repeatInterval);
                cycleFound = true;
                Console.WriteLine("Cycle found");
                break;
            }
        }
    }
}

var end = DateTime.Now;
Console.WriteLine(maxHeight-1 + ((repeatCount) * (repeatHeight)));
Console.WriteLine((end - start).TotalSeconds);

void TryLeft(Rock rock)
{
    bool canMove = false;
    foreach (var point in rockTypes[rock.RockType])
    {
        if (point.X + rock.X - 1 >= 0)
        {
            canMove = true;
        }
        else
        {
            canMove = false;
            break;
        }
        if (!settledList.Where(p => (p.settled.X == point.X + rock.X - 1) && (p.settled.Y == point.Y + rock.Y)).Any())
        {
            canMove = true;
        }
        else
        {
            canMove = false;
            break;
        }
    }
    if (canMove)
    {
        currentRock.X--;
        movedSide = true;
    }
}

void TryRight(Rock rock)
{
    bool canMove = false;
    foreach (var point in rockTypes[rock.RockType])
    {
        if (point.X + rock.X + 1 < width)
        {
            canMove = true;
        }
        else
        {
            canMove = false;
            break;
        }
        if (!settledList.Where(p => (p.settled.X == point.X + rock.X + 1) && (p.settled.Y == point.Y + rock.Y)).Any())
        {
            canMove = true;
        }
        else
        {
            canMove = false;
            break;
        }
    }
    if (canMove)
    {
        currentRock.X++;
        movedSide = true;
    }
}
void TryDown(Rock rock)
{
    bool canMove = false;
    foreach (var point in rockTypes[rock.RockType])
    {
        if (point.Y + rock.Y - 1 >= 0)
        {
            canMove = true;
        }
        else
        {
            canMove = false;
            break;
        }
        if (!settledList.Where(p => (p.settled.X == point.X + rock.X) && (p.settled.Y == point.Y + rock.Y - 1)).Any())
        {
            canMove = true;
        }
        else
        {
            canMove = false;
            break;
        }
    }
    if (canMove)
    {
        currentRock.Y--;
        movedDown = true;
    }
}

record struct Points(int X, int Y);
record struct Rock(int X, int Y, int RockType, bool Falling);