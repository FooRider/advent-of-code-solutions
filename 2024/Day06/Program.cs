const string inputFile = "input1.txt";

await using var fh = File.OpenRead(inputFile);
using var sr = new StreamReader(fh);

var (map, g) = Map.LoadFromLines(ReadLines(sr));


{
    Console.WriteLine("Part 1");
    List<(int, int, Direction)> guardPositions = [];
    Guard? guard = g;
    while (guard != null)
    {
        //Console.WriteLine(guard);
        //Map.Debug(map, guard.Value, guardPositions);
        if (guardPositions.Contains((guard.Value.X, guard.Value.Y, guard.Value.Dir)))
            break;
        guardPositions.Add((guard.Value.X, guard.Value.Y, guard.Value.Dir));
        guard = guard.Value.Step(map);
    }
    Console.WriteLine(guardPositions.Select(t => (t.Item1, t.Item2)).Distinct().Count());
}

{
    Console.WriteLine("Part 2");
    List<(int, int, Direction)> guardPositions = [];
    List<(int, int)> newBarrierPositions = [];
    Guard? guard = g;
    while (guard != null)
    {
        //Console.WriteLine(guard);
        //Map.Debug(map, guard.Value, guardPositions);
        if (guardPositions.Contains((guard.Value.X, guard.Value.Y, guard.Value.Dir)))
            break;
        guardPositions.Add((guard.Value.X, guard.Value.Y, guard.Value.Dir));
        
        var nextPosition = guard.Value.Step(map);
        if (nextPosition is {} position 
            && !guardPositions.Any(p => p.Item1 == position.X && p.Item2 == position.Y)
            && !newBarrierPositions.Contains((position.X, position.Y))
            && !(position.X == g.X && position.Y == g.Y))
        {
            if (guard.Value.Simulate(map, new Barrier(position.X, position.Y), guardPositions) == SimulationResult.GuardLoop)
            {
                newBarrierPositions.Add((position.X, position.Y));
            }
        }
        guard = nextPosition;
    }
    Console.WriteLine(newBarrierPositions.Count);
}


IEnumerable<string> ReadLines(TextReader tr)
{
    var line = tr.ReadLine();
    while (line is not null)
    {
        yield return line;
        line = tr.ReadLine();
    }
}

enum Direction { Up, Right, Down, Left };
enum SimulationResult { GuardLeft, GuardLoop }
readonly record struct Guard(int X, int Y, Direction Dir)
{
    public Guard? Step(Map map)
    {
        var nextPosition = Dir switch
        {
            Direction.Up    => (X, Y - 1),
            Direction.Right => (X + 1, Y),
            Direction.Down  => (X, Y + 1),
            Direction.Left  => (X - 1, Y),
        };

        if (map.Barriers.Any(b => b.X == nextPosition.Item1 && b.Y == nextPosition.Item2))
            return new Guard(X, Y, Rotate(Dir));

        if (nextPosition.Item1 < 0 || nextPosition.Item1 >= map.Width || nextPosition.Item2 < 0 || nextPosition.Item2 >= map.Height)
            return null;

        return new Guard(nextPosition.Item1, nextPosition.Item2, Dir);
    }

    public SimulationResult Simulate(Map map, Barrier newBarrier, IReadOnlyCollection<(int, int, Direction)> positionsSoFar)
    {
        var modifiedMap = map with { Barriers = map.Barriers.Concat([newBarrier]).ToList() };
        var guardPositions = new List<(int, int, Direction)>(positionsSoFar);

        Guard? guard = Step(modifiedMap);
        while (true)
        {
            if (guard == null)
                return SimulationResult.GuardLeft;
            if (guardPositions.Contains((guard.Value.X, guard.Value.Y, guard.Value.Dir)))
                return SimulationResult.GuardLoop;
            guardPositions.Add((guard.Value.X, guard.Value.Y, guard.Value.Dir));
            guard = guard.Value.Step(modifiedMap);
            
        }
    }

    public static Direction Rotate(Direction dir) => dir switch
    {
        Direction.Up => Direction.Right,
        Direction.Right => Direction.Down,
        Direction.Down => Direction.Left,
        Direction.Left => Direction.Up,
    };
    
    public static Direction GetDirection(char ch) =>
        ch switch
        {
            '^' => Direction.Up,
            'v' => Direction.Down,
            '<' => Direction.Left,
            '>' => Direction.Right,
            _ => throw new ArgumentException($"Invalid direction {ch}")
        };
};
record Barrier(int X, int Y);
record Map(int Width, int Height, IReadOnlyCollection<Barrier> Barriers)
{
    public static (Map, Guard) LoadFromLines(IEnumerable<string> lines)
    {
        int? width = null;
        int y = 0;
        List<Barrier> barriers = [];
        Guard? guard = null;
        foreach (var line in lines)
        {
            width ??= line.Length;
            int x = 0;
            foreach (var ch in line)
            {
                if (ch == '#') barriers.Add(new Barrier(x, y));
                if (ch is 'v' or '^' or '<' or '>') guard = new Guard(x, y, Guard.GetDirection(ch));
                x++;
            }
            y++;
        }

        if (guard is null)
            throw new ArgumentException($"Could not find guard in map");
        if (width is null)
            throw new ArgumentException($"Could not load map");

        var map = new Map(width.Value, y, barriers);

        return (map, guard.Value);
    }

    public static void Debug(
        Map map, 
        Guard guard, 
        IReadOnlyCollection<(int, int, Direction)> positions,
        int extension = 5)
    {
        for (int y = guard.Y - extension; y <= guard.Y + extension; y++)
        {
            for (int x = guard.X - extension; x <= guard.X + extension; x++)
            {
                if (map.Barriers.Any(b => b.X == x && b.Y == y))
                    Console.Write('#');
                else
                {
                    var prevPositions = positions.Where(p => p.Item1 == x && p.Item2 == y).ToList();
                    if (prevPositions.Count == 0)
                        Console.Write(' ');
                    else if (prevPositions.Count == 1)
                        Console.Write(prevPositions[0].Item3 switch
                        {
                            Direction.Up => '^',
                            Direction.Right => '>',
                            Direction.Down => 'v',
                            Direction.Left => '<',
                        });
                    else
                        Console.Write('X');
                }
            }
            Console.WriteLine();
        }
    }
}