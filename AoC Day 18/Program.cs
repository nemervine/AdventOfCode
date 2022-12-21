var input = File.ReadAllLines("input.txt");

var cubes = new HashSet<(int X, int Y, int Z)>();
foreach (var line in input)
{
    var cube = line.Split(',');
    cubes.Add(new(int.Parse(cube[0]), int.Parse(cube[1]), int.Parse(cube[2])));
}
int exposed = 0;

Part1();
exposed = 0;
Part2();

void Part1()
{
    foreach (var cube in cubes)
    {
        if (cubes.Where(v => (v.X == cube.X) && (v.Y == cube.Y) && (v.Z == cube.Z - 1)).Count() == 0)
            exposed++;
        if (cubes.Where(v => (v.X == cube.X) && (v.Y == cube.Y) && (v.Z == cube.Z + 1)).Count() == 0)
            exposed++;
        if (cubes.Where(v => (v.X == cube.X) && (v.Y == cube.Y - 1) && (v.Z == cube.Z)).Count() == 0)
            exposed++;
        if (cubes.Where(v => (v.X == cube.X) && (v.Y == cube.Y + 1) && (v.Z == cube.Z)).Count() == 0)
            exposed++;
        if (cubes.Where(v => (v.X == cube.X - 1) && (v.Y == cube.Y) && (v.Z == cube.Z)).Count() == 0)
            exposed++;
        if (cubes.Where(v => (v.X == cube.X + 1) && (v.Y == cube.Y) && (v.Z == cube.Z)).Count() == 0)
            exposed++;
    }
    Console.WriteLine(exposed);
}

void Part2()
{
    var answer = 0;
    var neighbors = new List<(int x, int y, int z)> { (1, 0, 0), (-1, 0, 0), (0, 1, 0), (0, -1, 0), (0, 0, 1), (0, 0, -1) };
    var minX = cubes.Min(v => v.X);
    var minY = cubes.Min(v => v.Y);
    var minZ = cubes.Min(v => v.Z);
    var maxX = cubes.Max(v => v.X);
    var maxY = cubes.Max(v => v.Y);
    var maxZ = cubes.Max(v => v.Z);

    var xRange = Enumerable.Range(minX, maxX + 1).ToList();
    var yRange = Enumerable.Range(minY, maxY + 1).ToList();
    var zRange = Enumerable.Range(minZ, maxZ + 1).ToList();

    bool isOutside((int x, int y, int z) cube)
    {
        if (cubes.Contains(cube)) return false;

        var checkedCubes = new HashSet<(int x, int y, int z)>();
        var queue = new Queue<(int x, int y, int z)>();
        queue.Enqueue(cube);
        while (queue.Any())
        {
            var tempCube = queue.Dequeue();
            if (checkedCubes.Contains(tempCube)) continue;
            checkedCubes.Add(tempCube);
            if (!xRange.Contains(tempCube.x) || !yRange.Contains(tempCube.y) || !zRange.Contains(tempCube.z))
               return true;
            if (!cubes.Contains(tempCube))
                foreach (var (dx, dy, dz) in neighbors)
                    queue.Enqueue((tempCube.x + dx, tempCube.y + dy, tempCube.z + dz));
        }
        return false;
    }
    foreach (var (x, y, z) in cubes)
        foreach (var (dx, dy, dz) in neighbors)
            if (isOutside((x + dx, y + dy, z + dz)))
                answer++;

    Console.WriteLine(answer);
}