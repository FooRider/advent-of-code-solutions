
using Day17;

using var fh = File.OpenText("input1.txt");
var state = ProcessorState.Load(fh);

var stateP1 = Solver.Run(state);
Console.WriteLine($"Part 1: {stateP1.Output}");

var solution = Solver.Part2SpecificSolver(state);
Console.WriteLine($"Part 2: {solution}");