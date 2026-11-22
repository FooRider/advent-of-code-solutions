
using Day21;

var fh = File.OpenText("input1.txt");
var input = new List<string>();
while (fh.ReadLine() is {} line)
    input.AddRange(line);
    
var p1 = input.Sum(Solver.SolveSinglePart1);
Console.WriteLine($"Part 1: {p1}");

var p2 = input.Sum(Solver.SolveSinglePart2);
Console.WriteLine($"Part 2: {p2}");