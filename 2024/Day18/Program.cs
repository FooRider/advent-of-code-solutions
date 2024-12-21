
using System.Collections.Immutable;
using Day18;

using var sr = File.OpenText("input1.txt");
var input = Solver.LoadInput(sr).ToImmutableArray();

{
    var input1 = input.Take(1024).ToImmutableArray();
    var res = Solver.SolvePart1(input1, 71, 71);
    Console.WriteLine($"Part 1: {res}");
}

{
    var possible = 1024;
    var impossible = input.Length;

    while (impossible >= possible + 1)
    {
        var t1 = (impossible + possible) / 2;
        var t2 = t1 + 1;

        bool t1Possible = Solver.CheckSolutionExists(input[..t1], 71, 71);
        bool t2Possible = Solver.CheckSolutionExists(input[..t2], 71, 71);

        if (t1Possible && !t2Possible)
        {
            Console.WriteLine($"Part 2: {input[t2 - 1].Item1},{input[t2 - 1].Item2}");
            break;
        }

        if (t1Possible && t2Possible)
            possible = t2;
        else if (!t1Possible && !t2Possible)
            impossible = t1;
        else
            throw new Exception();
    }
}