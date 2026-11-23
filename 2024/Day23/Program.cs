using Day23;

using var fh = File.OpenText("input1.txt");
var nodes = Solver.LoadGraph(fh);

// var part1 = Solver.SolvePart1(nodes);
// Console.WriteLine($"Part 1: {part1}");

var part2 = Solver.SolvePart2(nodes);
Console.WriteLine($"Part 2: {part2}");