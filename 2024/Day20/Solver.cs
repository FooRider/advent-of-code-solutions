using System.Collections.Immutable;
using Common.Graphs;

namespace Day20;

using Node1 = Common.Graphs.Node<(int, int), int>;

public static class Solver
{
    public static (MapPosition[,] Map, (int, int) Start, (int, int) End) LoadMap(string input)
    {
        using var sr = new StringReader(input);
        return LoadMap(sr);
    }

    public static (MapPosition[,] Map, (int, int) Start, (int, int) End) LoadMap(TextReader input)
    {
        var startPos = (-1, -1);
        var endPos = (-1, -1);
        
        List<MapPosition[]> lines = new();
        while (input.ReadLine() is { } line && !string.IsNullOrWhiteSpace(line))
        {
            lines.Add(line.Select(ch => ch == '#' ? MapPosition.Wall : MapPosition.Space).ToArray());
            int startIdx = line.IndexOf('S');
            if (startIdx >= 0)
                startPos = (lines.Count - 1, startIdx);
            int endIdx = line.IndexOf('E');
            if (endIdx >= 0)
                endPos = (lines.Count - 1, endIdx);
        }
        
        var map = new MapPosition[lines.Count, lines[0].Length];
        int rowIdx = 0;
        foreach (var line in lines)
        {
            for (int colIdx = 0; colIdx < line.Length; colIdx++) 
                map[rowIdx, colIdx] = line[colIdx];
            rowIdx++;
        }

        return (map, startPos, endPos);
    }

    public static (Node1 Start, Node1 End, ImmutableArray<Node1> AllNodes, Node1?[,] Nodes2d) EmptyToGraph(
        MapPosition[,] map, 
        (int, int) start,
        (int, int) end)
    {
        Node1? startNode = null;
        Node1? endNode = null;
        var allNodes = new List<Node1>();
        var nodes2d = new Node1?[map.GetLength(0), map.GetLength(1)];
        
        for (int rowIdx = 0; rowIdx < map.GetLength(0); rowIdx++)
        for (int colIdx = 0; colIdx < map.GetLength(1); colIdx++)
        {
            if (map[rowIdx, colIdx] == MapPosition.Wall) continue;
            var node = new Node1((rowIdx, colIdx));
            allNodes.AddRange(node);
            nodes2d[rowIdx, colIdx] = node;
            if ((rowIdx, colIdx) == start) startNode = node;
            if ((rowIdx, colIdx) == end) endNode = node;

            if (rowIdx > 0 && map[rowIdx - 1, colIdx] == MapPosition.Space)
            {
                if (nodes2d[rowIdx - 1, colIdx] is { } node2)
                {
                    node.LinkTargetNode(0, node2);
                    node2.LinkTargetNode(0, node);
                }
            }

            if (colIdx > 0 && map[rowIdx, colIdx - 1] == MapPosition.Space)
            {
                if (nodes2d[rowIdx, colIdx - 1] is { } node2)
                {
                    node.LinkTargetNode(0, node2);
                    node2.LinkTargetNode(0, node);
                }
            }
        }
        
        return (startNode!, endNode!, [..allNodes], nodes2d);
    }

    public static IReadOnlyCollection<int> CalculateSavedTimePerCheat((MapPosition[,] map, (int, int) start, (int, int) end) input)
    {
        var result = new List<int>();
        
        var (map, start, end) = input;
        var (startNode, endNode, l1Nodes, l1Nodes2d) = Solver.EmptyToGraph(map, start, end);
        var evaluatedNodesSE = GraphSearch.BreadthFirstSearch2(
            startNode,
            _ => 1, 
            _ => true, 
            x => x.Node == endNode);
        var (_, noCheatRank) = evaluatedNodesSE.Single(x => x.Node == endNode);
        
        var evaluatedNodesES = GraphSearch.BreadthFirstSearch2(
            endNode,
            _ => 1, 
            _ => true,  
            x => x.Node == startNode);
        var (_, noCheatRankES) = evaluatedNodesSE.Single(x => x.Node == endNode);

        var evaluatedSE = new int?[map.GetLength(0), map.GetLength(1)];
        foreach (var en in evaluatedNodesSE)
            evaluatedSE[en.Node.Value.Item1, en.Node.Value.Item2] = en.Rank;
        var evaluatedES = new int?[map.GetLength(0), map.GetLength(1)];
        foreach (var en in evaluatedNodesES)
            evaluatedES[en.Node.Value.Item1, en.Node.Value.Item2] = en.Rank;
        
        for (var row = 0; row < map.GetLength(0); row++)
        for (var col = 0; col < map.GetLength(1); col++)
        {
            if (map[row, col] != MapPosition.Wall) continue;

            foreach (var (n1r, n1c) in GetNeighbours(row, col, map.GetLength(0), map.GetLength(1)))
            foreach (var (n2r, n2c) in GetNeighbours(row, col, map.GetLength(0), map.GetLength(1)))
            {
                if (n1r == n2r && n1c == n2c) continue;
                if (map[n1r, n1c] == MapPosition.Wall) continue;
                if (map[n2r, n2c] == MapPosition.Wall) continue;
                
                var n1FromStart = evaluatedSE[n1r, n1c];
                var n2FromEnd = evaluatedES[n2r, n2c];
                
                if (!n1FromStart.HasValue || !n2FromEnd.HasValue) continue;

                var timeIfShortcutUsed = n1FromStart.Value + 2 + n2FromEnd.Value;
                if (timeIfShortcutUsed < noCheatRank)
                {
                    result.AddRange(noCheatRank - timeIfShortcutUsed);
                }
            }
        }
        return result;

        IEnumerable<(int, int)> GetNeighbours(int row, int col, int height, int width)
        {
            if (row > 0) yield return (row - 1, col);
            if (row < height - 1) yield return (row + 1, col);
            if (col > 0) yield return (row, col - 1);
            if (col < width - 1) yield return (row, col + 1);
        }
    }
}

public enum MapPosition
{
    Space,
    Wall
}