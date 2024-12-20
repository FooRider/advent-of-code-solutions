
using Day16;

using var fh = File.OpenText("input1.txt");
var (map, startPosition, endPosition) = Solver.LoadInput(fh);

var (startNode, targetNodes) = Solver.ToGraphPart1(map, startPosition, endPosition);
//var p1 = Solver.SolvePart1(startNode, targetNodes);
var (p1, p2) = Solver.SolvePart12(startNode, targetNodes);
Console.WriteLine($"Part 1: {p1}");
Console.WriteLine($"Part 2: {p2}");