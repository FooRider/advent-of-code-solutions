using Common.Graphs;

using MyNode = Common.Graphs.Node<(int Height, int X, int Y), bool>;

using var fh = File.OpenText("input1.txt");

var nodes = new List<MyNode>();
var previousLineNodes = new List<MyNode>();
int y = -1;
while (await fh.ReadLineAsync() is { } line)
{
    y++;
    var lineNodes = line
        .Select(ch => (int)(ch - '0'))
        .Select((height, x) => new MyNode((height, x, y)))
        .ToList();
    nodes.AddRange(lineNodes);

    for (int x = 0; x < lineNodes.Count; x++)
    {
        if (x > 0 && ShouldBeLinked(lineNodes[x], lineNodes[x - 1]))
            lineNodes[x].LinkTargetNode(true, lineNodes[x - 1]);
        if (x < lineNodes.Count - 1 && ShouldBeLinked(lineNodes[x], lineNodes[x + 1]))
            lineNodes[x].LinkTargetNode(true, lineNodes[x + 1]);
        if (previousLineNodes.Count > x && ShouldBeLinked(lineNodes[x], previousLineNodes[x]))
            lineNodes[x].LinkTargetNode(true, previousLineNodes[x]);
        if (previousLineNodes.Count > x && ShouldBeLinked(previousLineNodes[x], lineNodes[x]))
            previousLineNodes[x].LinkTargetNode(true, lineNodes[x]);
    }
    previousLineNodes = lineNodes;
}

{
    Console.WriteLine("Part 1");

    int sumScore = 0;
    foreach (var trailHead in nodes.Where(n => n.Value.Height == 0))
    {
        var peaks = GraphSearch.BreadthFirstSearch(
            trailHead, nt => nt.Node.Value.Height == 9);
        sumScore += peaks.Count;
    }
    Console.WriteLine(sumScore);
}

{
    Console.WriteLine("Part 2");

    int sumScore = 0;
    foreach (var trailHead in nodes.Where(n => n.Value.Height == 0))
    {
        var distinctPaths = GraphSearch.DepthFirstSearch(
            trailHead, nt => nt.Node.Value.Height == 9);
        sumScore += distinctPaths.Count;
    }
    Console.WriteLine(sumScore);
}


bool ShouldBeLinked(MyNode source, MyNode target)
{
    if (source.Value.Height + 1 != target.Value.Height) return false;

    return (Math.Abs(source.Value.Y - target.Value.Y), Math.Abs(source.Value.X - target.Value.X)) switch
    {
        (0, 1) => true,
        (1, 0) => true,
        _ => false
    };
}