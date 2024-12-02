using System.Collections.Immutable;

var inputFile = "input1.txt";

await using var fh = File.OpenRead(inputFile);
using var sr = new StreamReader(fh);

var reports = new List<ImmutableList<int>>();
while (await sr.ReadLineAsync() is { } line)
{
    var levels = new List<int>();
    var ls = line.AsSpan();
    var enumerator = ls.Split(' ');
    while (enumerator.MoveNext())
        levels.Add(int.Parse(ls[enumerator.Current]));
    reports.Add(levels.ToImmutableList());
}

var reportsByAbsoluteSafety = reports
    .GroupBy(IsAbsolutelySafe)
    .ToDictionary(g => g.Key);

{
    Console.WriteLine("Part 1");
    Console.WriteLine(reportsByAbsoluteSafety[true].Count());
}

{
    Console.WriteLine("Part 2");
    var safeOnlyWithDampening = reportsByAbsoluteSafety[false].Count(r => GetLevelsWithOneSkipped(r).Any(IsAbsolutelySafe));
    Console.WriteLine(reportsByAbsoluteSafety[true].Count() + safeOnlyWithDampening);
}

bool IsAbsolutelySafe(IEnumerable<int> report) =>
    !report.Aggregate((null, false, null),
        ((int? Direction, bool UnsafeAlready, int? PreviousLevel) acc, int level) =>
            acc switch
            {
                (_, true, _) => (acc.Direction, true, level),
                (null, _, null) => (null, false, level),
                (null, _, { } prev) => (
                    Math.Sign(level - prev), 
                    (Math.Abs(level - prev) is < 1 or > 3), 
                    level),
                ({ } dir, _, { } prev) => (
                    dir,
                    (Math.Abs(level - prev) is < 1 or > 3) || Math.Sign(level - prev) != dir,
                    level)
            }
    ).UnsafeAlready;

IEnumerable<IEnumerable<int>> GetLevelsWithOneSkipped(IReadOnlyCollection<int> inputReport)
{
    for (int i = 0; i < inputReport.Count; i++)
        yield return inputReport.Take(i).Concat(inputReport.Skip(i + 1));
}