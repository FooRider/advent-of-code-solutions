
using Day19;

using var fh = File.OpenText("input1.txt");
var (available, desired) = Solver.Load(fh);

var impossibleDesigns = new HashSet<string>();
{
    int possibleDesigns = 0;
    foreach (var d in desired)
    {
        bool possible = Solver.CanCombine(d, available, impossibleDesigns);
        if (possible)
            possibleDesigns++;
    }
    
    Console.WriteLine($"Part 1: {possibleDesigns}");
}
{
    long result = 0;
    Dictionary<string, long> cache = [];
    foreach (var d in desired)
    {
        var partial = Solver.WaysToCombine(d, available, cache);
        result += partial;
    }
    Console.WriteLine($"Part 2: {result}");
}