using System.Linq;

var start = DateTime.Now;
Console.WriteLine("Start: " + start);
var input = File.ReadAllLines("test.txt");
int time = 24;
var blueprints = new List<Blueprint>();

foreach (var line in input)
{
    var parse = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    blueprints.Add(new(int.Parse(parse[6]), int.Parse(parse[12]), int.Parse(parse[18]), int.Parse(parse[21]), int.Parse(parse[27]), int.Parse(parse[30])));
}

int maxOre;
int maxClay;
int maxObs;
int score = 0;

var listBots = new List<int>();

var listBots2 = new List<int>();

//var scoresCache = new Dictionary<(Blueprint blueprint, int ore, int oreBots, int clay, int clayBots, int obsidian, int obsidianBots, int geode, int geodeBots, int round, int maxRounds), int>();

for (int i = 0; i < blueprints.Count; i++)
{
    listBots = new List<int>();
    Console.WriteLine("Checking Blueprint " + (i + 1));
    int ore = 0;
    int oreBots = 1;
    int clay = 0;
    int clayBots = 0;
    int obs = 0;
    int obsBots = 0;
    int geode = 0;
    int geodeBots = 0;

    int round = 0;

    var blueprint = blueprints[i];

    maxOre = new[] { blueprint.OreOre, blueprint.ClayOre, blueprint.ObsidianOre, blueprint.GeodeOre }.Max();
    maxClay = blueprint.ObsidianClay;
    maxObs = blueprint.GeodeObsidian;

    int newScore = doBlueprint(blueprint, ore, oreBots, clay, clayBots, obs, obsBots, geode, geodeBots, round, time);
    listBots2.Add(listBots.Max());
    score += newScore * (i + 1);
    Console.WriteLine("Finished Blueprint " + (i + 1));
}

var quality = score;

var part1 = DateTime.Now;
Console.WriteLine("Part 1: " + quality);
Console.WriteLine("Part 1 time: " + (part1 - start).TotalMilliseconds);

time = 32;
var blueprintsp2 = new List<Blueprint>();
for (int i = 0; i < Math.Min(3, blueprints.Count); i++)
    blueprintsp2.Add(blueprints[i]);
score = 1;

for (int i = 0; i < blueprintsp2.Count; i++)
{
    Console.WriteLine("Checking Blueprint " + (i + 1));
    int ore = 0;
    int oreBots = 1;
    int clay = 0;
    int clayBots = 0;
    int obs = 0;
    int obsBots = 0;
    int geode = 0;
    int geodeBots = 0;

    int round = 0;

    var blueprint = blueprintsp2[i];

    maxOre = new[] { blueprint.OreOre, blueprint.ClayOre, blueprint.ObsidianOre, blueprint.GeodeOre }.Max();
    maxClay = blueprint.ObsidianClay;
    maxObs = blueprint.GeodeObsidian;

    int newScore = doBlueprint(blueprint, ore, oreBots, clay, clayBots, obs, obsBots, geode, geodeBots, round, time);
    score *= newScore;
    Console.WriteLine("Finished Blueprint " + (i + 1));
}

var part2 = DateTime.Now;
Console.WriteLine("Part 2: " + score);
Console.WriteLine("Part 2 time: " + (part2 - part1).TotalMilliseconds);

int doBlueprint(Blueprint blueprint, int ore, int oreBots, int clay, int clayBots, int obsidian, int obsidianBots, int geode, int geodeBots, int round, int maxRounds)
{
    bool prevCanOre = false;
    bool prevCanClay = false;
    bool prevCanObs = false;
    bool prevCanGeo = false;

    bool canOre = false;
    bool canClay = false;
    bool canObs = false;
    bool canGeo = false;

    int score = geode;
    int roundScore = 0;
    /*if (scoresCache.Where(s => s.Key == (blueprint, ore, oreBots, clay, clayBots, obsidian, obsidianBots, geode, geodeBots, round, maxRounds)).Any())
        return scoresCache[(blueprint, ore, oreBots, clay, clayBots, obsidian, obsidianBots, geode, geodeBots, round, maxRounds)];*/

    for (int i = round; i < maxRounds; i++)
    {
        canOre = blueprint.OreOre <= ore;
        canClay = blueprint.ClayOre <= ore;
        canObs = blueprint.ObsidianOre <= ore && blueprint.ObsidianClay <= clay;
        canGeo = blueprint.GeodeOre <= ore && blueprint.GeodeObsidian <= obsidian;

        ore += oreBots;
        clay += clayBots;
        obsidian += obsidianBots;
        geode += geodeBots;

        if (canGeo && !prevCanGeo)
        {
            roundScore = doBlueprint(blueprint, ore - blueprint.GeodeOre, oreBots, clay, clayBots, obsidian - blueprint.GeodeObsidian, obsidianBots, geode, geodeBots + 1, i + 1, maxRounds);
            score = Math.Max(score, roundScore);
            prevCanGeo = canGeo;
        }
        if (canObs && !prevCanObs && obsidianBots < maxObs)
        {
            roundScore = doBlueprint(blueprint, ore - blueprint.ObsidianOre, oreBots, clay - blueprint.ObsidianClay, clayBots, obsidian, obsidianBots + 1, geode, geodeBots, i + 1, maxRounds);
            score = Math.Max(score, roundScore);
            prevCanObs = canObs;
        }
        if (canClay && !prevCanClay && clayBots < maxClay)
        {
            roundScore = doBlueprint(blueprint, ore - blueprint.ClayOre, oreBots, clay, clayBots + 1, obsidian, obsidianBots, geode, geodeBots, i + 1, maxRounds);
            score = Math.Max(score, roundScore);
            prevCanClay = canClay;
        }
        if (canOre && !prevCanOre && oreBots < maxOre)
        {
            roundScore = doBlueprint(blueprint, ore - blueprint.OreOre, oreBots + 1, clay, clayBots, obsidian, obsidianBots, geode, geodeBots, i + 1, maxRounds);
            score = Math.Max(score, roundScore);
            prevCanOre = canOre;
        }

        score = Math.Max(score, geode);
    }
    //scoresCache[(blueprint, ore, oreBots, clay, clayBots, obsidian, obsidianBots, geode, geodeBots, round, maxRounds)] = score;
    listBots.Add(geodeBots);
    return score;
}

record struct Blueprint(int OreOre, int ClayOre, int ObsidianOre, int ObsidianClay, int GeodeOre, int GeodeObsidian){ }
/*{
    public int OreOre, ClayOre;
    public int ObsidianOre, ObsidianClay;
    public int GeodeOre, GeodeObsidian;

    public Blueprint(int ore, int clay, int oOre, int oClay, int geOre, int geObsidian)
    {
        OreOre = ore;
        ClayOre = clay;
        ObsidianOre = oOre;
        ObsidianClay = oClay;
        GeodeOre = geOre;
        GeodeObsidian = geObsidian;
    }}
*/