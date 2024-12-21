using Common.Graphs;
using Node1 = Common.Graphs.Node<(int X, int Y), int>;

namespace Day18;

public static class Solver
{
    public static IReadOnlyCollection<(int, int)> LoadInput(TextReader tr)
    {
        var res = new List<(int, int)>();
        while (tr.ReadLine() is { } line&& !string.IsNullOrWhiteSpace(line))
        {
            var parts = line.Split(',');
            res.Add((int.Parse(parts[0]), int.Parse(parts[1])));
        }
        return res;
    }

    public static int SolvePart1(IReadOnlyCollection<(int, int)> input, int width, int height)
    {
        var (startNode, endNode) = Part1ToNodes(input, width, height);
        var resNodes = GraphSearch.BreadthFirstSearch(startNode, x => x.Node == endNode);
        return resNodes.First().Rank;
    }

    public static bool CheckSolutionExists(IReadOnlyCollection<(int, int)> input, int width, int height)
    {
        var (startNode, endNode) = Part1ToNodes(input, width, height);
        var resNodes = GraphSearch.BreadthFirstSearch(startNode, x => x.Node == endNode);
        return resNodes.Any();
    }
    
    public static (Node1 start, Node1 end) Part1ToNodes(
        IReadOnlyCollection<(int, int)> corruptedPositions, 
        int width,
        int height)
    {
        Node1? startNode = null;
        Node1? endNode = null;
        
        var allNodes = new List<Node1>();
        
        var corrupted = corruptedPositions.ToHashSet();
        for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
        {
            if (corrupted.Contains((x, y)))
                continue;

            var node = new Node1((x, y));
            allNodes.Add(node);
            if (x == 0 && y == 0) startNode = node;
            if (x == width - 1 && y == height - 1) endNode = node;

            if (x > 0 && !corrupted.Contains((x - 1, y)))
            {
                var nnode = allNodes.First(n => n.Value == (x - 1, y));
                nnode.LinkTargetNode(0, node);
                node.LinkTargetNode(0, nnode);
            }
            if (y > 0 && !corrupted.Contains((x, y - 1)))
            {
                var nnode = allNodes.First(n => n.Value == (x, y - 1));
                nnode.LinkTargetNode(0, node);
                node.LinkTargetNode(0, nnode);
            }
        }
        
        return (startNode!, endNode!);
    }

    public static IEnumerable<(int, int)> PossibleNeighbours((int, int) position, int width, int height)
    {
        var (x, y) = position;
        if (x > 0) yield return (x - 1, y);
        if (x < width - 1) yield return (x + 1, y);
        if (y > 0) yield return (x, y - 1);
        if (y < height - 1) yield return (x, y + 1);
    }
}