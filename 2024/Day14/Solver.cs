namespace Day14;

public class Solver
{
    public static (int, int) GetPart1PositionAfterTime(Robot robot, int mapWidth, int mapHeight, int time)
        => (
            ((robot.InitialPosition[0] + robot.Velocity[0] * time % mapWidth) + mapWidth) % mapWidth,
            ((robot.InitialPosition[1] + robot.Velocity[1] * time % mapHeight) + mapHeight) % mapHeight
        );

    public static int GetQuadrant((int, int) position, int mapWidth, int mapHeight)
        => (Math.Sign(position.Item1 - (mapWidth / 2)), Math.Sign(position.Item2 - (mapHeight / 2))) switch
        {
            (-1, -1) => 1,
            (1, -1) => 2,
            (-1, 1) => 3,
            (1, 1) => 4,
            _ => 0
        };

    public static IEnumerable<Robot> ReadRobots(TextReader input)
    {
        while (Robot.TryParse(input) is { } robot)
            yield return robot;
    }

    public static long SolvePart1(IEnumerable<Robot> robots, int mapWidth, int mapHeight)
    {
        var quadrants = robots
            .Select(r => Solver.GetPart1PositionAfterTime(r, mapWidth, mapHeight, 100))
            //.Distinct()
            .Select(p => GetQuadrant(p, mapWidth, mapHeight))
            .GroupBy(q => q)
            .ToDictionary(q => q.Key, q => q.Count());
        for (int q = 1; q <= 4; q++) 
            quadrants.TryAdd(q, 0);
        return quadrants[1] * quadrants[2] * quadrants[3] * quadrants[4];
    }
}

public record struct Robot(int[] InitialPosition, int[] Velocity)
{
    public static Robot? TryParse(TextReader tr)
    {
        if (tr.ReadLine() is not { } lineStr)
            return null;
        var lineSpan = lineStr.AsSpan();
        var delimPos = lineSpan.IndexOf(' ');
        return new Robot(
            ReadIntVector(lineSpan.Slice(2, delimPos - 2)),
            ReadIntVector(lineSpan.Slice(delimPos + 2 + 1)));
    }

    private static int[] ReadIntVector(ReadOnlySpan<char> input)
    {
        var delim = input.IndexOf(',');
        var i1 = input[..delim];
        var i2 = input[(delim + 1)..];
        return [int.Parse(i1), int.Parse(i2)];
    }
}