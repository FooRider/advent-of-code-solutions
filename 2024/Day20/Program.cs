
using Day20;

using var fh = File.OpenText("input1.txt");
var (map, start, end) = Solver.LoadMap(fh);
var timesSaved = Solver.CalculateSavedTimePerCheat((map, start, end));
var p1 = timesSaved.Count(t => t >= 100);
Console.WriteLine($"Part 1: {p1}");


timesSaved = Solver.CalculateSavedTimePerLongCheat((map, start, end), 100);
var p2 = timesSaved.LongCount(t => t >= 100);
Console.WriteLine($"Part 2: {p2}");