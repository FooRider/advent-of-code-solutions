using Common.Graphs;

using Node1 = Common.Graphs.Node<(int Y, int X, Day16.Orientation O), int>;

namespace Day16;

public static class Solver
{
    public static (MapObject[,], (int, int), (int, int)) LoadInput(TextReader input)
    {
        (int, int) startPosition = (-1, -1);
        (int, int) endPosition = (-1, -1);
        List<MapObject[]> map = [];
        int rowIdx = 0;
        while (input.ReadLine() is { } line && line != "")
        {
            map.Add(line.Select(ch => ch switch
            {
                '#' => MapObject.Wall,
                _ => MapObject.Empty
            }).ToArray());
            var startPos = line.IndexOf('S');
            if (startPos >= 0)
                startPosition = (rowIdx, startPos);
            var endPos = line.IndexOf('E');
            if (endPos >= 0)
                endPosition = (rowIdx, endPos);
            rowIdx++;
        }

        var mapArray = new MapObject[map.Count, map.First().Length];
        for (rowIdx = 0; rowIdx < map.Count; rowIdx++)
        for (int colIdx = 0; colIdx < map.First().Length; colIdx++)
            mapArray[rowIdx, colIdx] = map[rowIdx][colIdx];

        return (mapArray, startPosition, endPosition);
    }

    public static (Node1 StartNode, List<Node1> TargetNodes) ToGraphPart1(MapObject[,] map, (int, int) startPosition, (int, int) endPosition)
    {
        List<Node1> allNodes = [];

        Node1? startNode = null;
        List<Node1> endNodes = [];
        
        for (int rowIdx = 0; rowIdx < map.GetLength(0); rowIdx++)
        for (int colIdx = 0; colIdx < map.GetLength(1); colIdx++)
        {
            if (map[rowIdx, colIdx] == MapObject.Wall) continue;

            var nn = new Node1((rowIdx, colIdx, Orientation.North));
            var ne = new Node1((rowIdx, colIdx, Orientation.East));
            var ns = new Node1((rowIdx, colIdx, Orientation.South));
            var nw = new Node1((rowIdx, colIdx, Orientation.West));
            
            nn.LinkTargetNode(1000, ne); nn.LinkTargetNode(1000, nw);
            nw.LinkTargetNode(1000, nn); nw.LinkTargetNode(1000, ns);
            ns.LinkTargetNode(1000, nw); ns.LinkTargetNode(1000, ne);
            ne.LinkTargetNode(1000, ns); ne.LinkTargetNode(1000, nn);

            if ((rowIdx, colIdx) == startPosition)
                startNode = ne;
            
            if ((rowIdx, colIdx) == endPosition)
                endNodes.AddRange([nn, ne, ns, nw]);
            
            allNodes.AddRange([nn, ne, ns, nw]);

            var westernNode = allNodes.FirstOrDefault(n => n.Value == (rowIdx, colIdx - 1, Orientation.West));
            if (westernNode != null)
            {
                nw.LinkTargetNode(1, westernNode);
                var westernNodeBack = allNodes
                    .First(n => n.Value == (rowIdx, colIdx - 1, Orientation.East));
                westernNodeBack.LinkTargetNode(1, ne);
            }

            var norhernNode = allNodes.FirstOrDefault(n => n.Value == (rowIdx - 1, colIdx, Orientation.North));
            if (norhernNode != null)
            {
                nn.LinkTargetNode(1, norhernNode);
                var northernNodeBack = allNodes
                    .First(n => n.Value == (rowIdx - 1, colIdx, Orientation.South));
                northernNodeBack.LinkTargetNode(1, ns);
            }
        }
        
        if (startNode is not {} sn) throw new Exception();

        return (sn, endNodes);
    }

    public static long SolvePart1(Node1 startNode, List<Node1> targetNodes)
    {
        var resNodes = GraphSearch.BreadthFirstSearch2(
            startNode, e => e.Value, n => targetNodes.Contains(n.Node));
        return resNodes.OrderBy(rn => rn.Rank).First().Rank;
    }
}

public enum MapObject { Wall, Empty }
public enum Orientation { North, East, South, West }