
using Day14;

using var sr = File.OpenText("input1.txt");
var robots = Solver.ReadRobots(sr).ToList();
var part1 = Solver.SolvePart1(robots, 101, 103);
Console.WriteLine($"Part 1: {part1}");

for (var i = 0; i < 20_000; i++)
{
    var positions = robots
        .Select(r => Solver.GetPart1PositionAfterTime(r, 101, 103, i))
        .OrderBy(p => p.Item2).ThenBy(p => p.Item1)
        .Distinct();
    var longestStreak = LongestContiguous(positions);
    if (longestStreak > 5)
        Console.WriteLine($"time: {i}\tlongest streak {longestStreak}");

    int LongestContiguous(IEnumerable<(int, int)> orderedPositions)
    {
        int longestStreak = 0;
        int currentStreak = 0;
        var prev = (-1, -1);
        
        var enumerator = orderedPositions.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (prev.Item2 == enumerator.Current.Item2 && prev.Item1 + 1 == enumerator.Current.Item1)
            {
                currentStreak++;
            }
            else
            {
                if (currentStreak > longestStreak)
                    longestStreak = currentStreak;
                currentStreak = 1;
            }
            prev = enumerator.Current;
        }
        if (currentStreak > longestStreak)
            longestStreak = currentStreak;
        return longestStreak;
    }
}