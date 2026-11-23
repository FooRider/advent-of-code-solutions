using System.Collections.Concurrent;
using Common.Graphs;

namespace Day23;

using Node1 = Node<string, int>;

public static class Solver
{
    public static IReadOnlyCollection<Node1> LoadGraph(string input)
    {
        using var sr = new StringReader(input);
        return LoadGraph(sr);
    }

    public static IReadOnlyCollection<Node1> LoadGraph(TextReader input)
    {
        var result = new List<Node1>();
        while (input.ReadLine() is { } line)
        {
            var parts = line.Split('-');
            var n1n = parts[0];
            var n2n = parts[1];
            
            if (!result.Any(n => n.Value == n1n))
                result.Add(new Node1(n1n));
            if (!result.Any(n => n.Value == n2n))
                result.Add(new Node1(n2n));
            
            var n1 = result.First(n => n.Value == n1n);
            var n2 = result.First(n => n.Value == n2n);
            
            n1.LinkTargetNode(0, n2);
            n2.LinkTargetNode(0, n1);
        }

        return result;
    }

    public static IReadOnlyCollection<IReadOnlyCollection<Node1>> ListInterconnectedTriplets(
        IReadOnlyCollection<Node1> nodes)
    {
        var result = new List<IReadOnlyCollection<Node1>>();
        var testedNodes = new HashSet<Node1>();

        foreach (var n1 in nodes)
        {
            testedNodes.Add(n1);
            var adepts = n1.OutgoingEdges.Select(e => e.Target).Where(n => !testedNodes.Contains(n)).ToList();
            var testedNodes2 = new HashSet<Node1>();
            foreach (var n2 in adepts)
            {
                testedNodes2.Add(n2);
                var thirdNodes = n2.OutgoingEdges.Select(e => e.Target)
                    .Where(n => !testedNodes2.Contains(n) && !testedNodes.Contains(n) && adepts.Contains(n))
                    .ToList();
                foreach (var n3 in thirdNodes)
                {
                    if (!result.Any(t => t.Contains(n1) && t.Contains(n2) && t.Contains(n3)))
                        result.Add([n1, n2, n3]);
                }
            }
        }

        return result;
    }

    public static int SolvePart1(IReadOnlyCollection<Node1> nodes)
    {
        var triplets = ListInterconnectedTriplets(nodes);
        return triplets.Count(t => t.Any(n => n.Value.StartsWith('t')));
    }

    public static string SolvePart2(IReadOnlyCollection<Node1> nodes)
    {
        var subgraphs = FindLargestCompleteSubgraphs(nodes);
        var subgraph = subgraphs.Single();
        return string.Join(',', subgraph.Select(n => n.Value));
    }

    public static IReadOnlyCollection<IReadOnlyCollection<Node1>> FindLargestCompleteSubgraphs(IReadOnlyCollection<Node1> nodes)
    {
        IReadOnlyCollection<IReadOnlyCollection<Node1>> nTuplets = nodes.Select(n => new List<Node1>([n])).ToList();
        while (true)
        {
            var np1Tuplets = FindLargerCompleteSubgraphs(nTuplets);
            if (!np1Tuplets.Any())
                break;
            nTuplets = np1Tuplets;
        }
        return nTuplets;
    }
    
    public static IReadOnlyCollection<IReadOnlyCollection<Node1>> FindLargerCompleteSubgraphs(
        IReadOnlyCollection<IReadOnlyCollection<Node1>> nTuplets)
    {
        Console.WriteLine($"{nTuplets.First().Count}\t{nTuplets.Count}");
        var np1Tuplets = new List<IReadOnlyCollection<Node1>>();
        
        var allPossible = nTuplets.SelectMany(n => n).ToHashSet();
        foreach (var t in nTuplets)
        {
            var nTuplet = t.ToList();
            var adepts = nTuplet
                .SelectMany(n => n.OutgoingEdges.Select(e => e.Target))
                .Where(n => allPossible.Contains(n))
                .Where(n => !nTuplet.Contains(n))
                .ToList();

            var alreadyCounted = new ConcurrentDictionary<string, int>();
            //foreach (var adept in adepts)
            
            Parallel.ForEach(adepts, adept =>
            {
                if (nTuplet.All(n => adept.OutgoingEdges.Any(e => e.Target == n)))
                {
                    var completeNp1Tuple = nTuplet.Concat([adept]).OrderBy(n => n.Value).ToList();
                    var password = string.Join(',', completeNp1Tuple.Select(n => n.Value));
                    if (!alreadyCounted.ContainsKey(password))
                    {
                        alreadyCounted[password] = 0;
                        np1Tuplets.Add(completeNp1Tuple);
                    }
                }
            });
        }

        return np1Tuplets;
    }

    public static IEnumerable<IReadOnlyCollection<Node1>> BronKerbosch(
        IReadOnlyCollection<Node1> graph,
        IReadOnlyCollection<Node1> r,
        IReadOnlyCollection<Node1> candidates,
        IReadOnlyCollection<Node1> excluded)
    {
        if (!candidates.Any() && !excluded.Any())
        {
            yield return r.ToList();
        }
        else
        {
            var pivot = candidates.Union(excluded)
                .OrderByDescending(n => n.OutgoingEdges.Count)
                .First();

            var candidatesExceptPivotNeighbours =
                candidates.Except(pivot.OutgoingEdges.Select(e => e.Target))
                    .ToList();
        }
    }
}