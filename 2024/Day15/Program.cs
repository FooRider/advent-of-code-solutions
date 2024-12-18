using Day15;

using var fh = File.OpenText("input1.txt");

var (mapState, movements) = Solver.LoadInput(fh);

var part1 = Solver.SolvePart1(mapState, movements);
Console.WriteLine($"Part 1: {part1}");

var part2 = Solver.SolvePart2(mapState, movements);
Console.WriteLine($"Part 2: {part2}");