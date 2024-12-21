using System.Collections.Immutable;

namespace Day19;

public static class Solver
{
    public static (ImmutableArray<string> Available, ImmutableArray<string> Desired) Load(TextReader input)
    {
        var available = input.ReadLine()!.Split(",").Select(t => t.Trim()).ToList();
        input.ReadLine();
        var desired = new List<string>();
        while (input.ReadLine() is { } line && !string.IsNullOrWhiteSpace(line))
            desired.Add(line);
        
        return ([..available], [..desired]);
    }

    public static bool CanCombine(
        string desiredPattern, 
        ImmutableArray<string> availablePatterns,
        HashSet<string> impossibleDesigns)
    {
        if (desiredPattern == "")
            return true;

        if (impossibleDesigns.Contains(desiredPattern))
            return false;

        foreach (var availablePattern in availablePatterns.Where(ap => desiredPattern.StartsWith(ap)))
        {
            if (CanCombine(desiredPattern[(availablePattern.Length)..], availablePatterns, impossibleDesigns)) 
                return true;
        }
        
        impossibleDesigns.Add(desiredPattern);
        return false;
    }
    
    public static long WaysToCombine(string desiredPattern, 
        ImmutableArray<string> availablePatterns,
        Dictionary<string, long> cache)
    {
        if (desiredPattern == "")
            return 1;

        if (cache.ContainsKey(desiredPattern))
            return cache[desiredPattern];

        long result = 0;
        foreach (var availablePattern in availablePatterns.Where(ap => desiredPattern.StartsWith(ap)))
        {
            result += WaysToCombine(desiredPattern[(availablePattern.Length)..], availablePatterns, cache);
        }
        
        cache[desiredPattern] = result;
        return result;
    }
}