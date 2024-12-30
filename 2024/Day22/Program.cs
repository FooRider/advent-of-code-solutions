using Day22;

using var fh = File.OpenText("input1.txt");
var input = new List<long>();
while (fh.ReadLine() is {} line)
    input.Add(long.Parse(line));

var solution1 = Solver.Part1(input);
Console.WriteLine($"Part 1: {solution1}");

var solution2 = Solver.SolvePart2(input);
Console.WriteLine($"Part 2: {solution2}");