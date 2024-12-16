
using Day13;

using var fh = File.OpenText("input1.txt");

long sumPart1 = 0;
long sumPart2 = 0;
while (await Solver.TryLoadAsync(fh) is { } cm)
{
    sumPart1 += Solver.SolveClawMachinePart1(cm);
    sumPart2 += Solver.SolveClawMachinePart2(cm);
}

Console.WriteLine($"Part 1: {sumPart1}");
Console.WriteLine($"Part 2: {sumPart2}");