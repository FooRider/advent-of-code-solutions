using System.Text;

namespace Common.Graphs;
public record Node<TN, TE>(TN Value)
{
    public List<Edge<TN, TE>> OutgoingEdges { get; } = [];
    public List<Edge<TN, TE>> IncomingEdges { get; } = [];
    
    public void LinkTargetNode(TE value, Node<TN, TE> target)
    {
        var e = new Edge<TN, TE>(value, this, target);
        OutgoingEdges.Add(e);
        target.IncomingEdges.Add(e);
    }
}

public record Edge<TN, TE>(TE Value, Node<TN, TE> Source, Node<TN, TE> Target);

public static class GraphSearch
{
    public static IReadOnlyCollection<(Node<TN, TE> Node, int Rank)> BreadthFirstSearch<TN, TE>(
        Node<TN, TE> startNode,
        Predicate<(Node<TN, TE> Node, int Rank)> resultPredicate)
    {
        List<(Node<TN, TE> Node, int Rank)> visitedNodes = [(startNode, 0)];
        
        var edgesToTraverse = new Queue<(Edge<TN, TE>, int Rank)>(
            startNode.OutgoingEdges.Select(e => (e, 1)));

        while (edgesToTraverse.TryDequeue(out var edgeT))
        {
            var (edge, nextNodeRank) = edgeT;
            if (visitedNodes.Any(vn => vn.Node == edge.Target))
                continue;
            visitedNodes.Add((edge.Target, nextNodeRank));
            foreach (var nextEdge in edge.Target.OutgoingEdges)
                edgesToTraverse.Enqueue((nextEdge, nextNodeRank + 1));
        }
        
        var result = visitedNodes.Where(vn => resultPredicate((vn.Node, vn.Rank))).ToList();
        return result;
    }
    
    public static IReadOnlyCollection<(Node<TN, TE> Node, int Rank)> BreadthFirstSearch2<TN, TE>(
        Node<TN, TE> startNode,
        Func<Edge<TN, TE>, int> edgePrice,
        Predicate<(Node<TN, TE> Node, int Rank)> resultPredicate,
        Predicate<(Node<TN, TE> Node, int Rank)>? stopPredicate = null)
    {
        List<(Node<TN, TE> Node, int Rank)> visitedNodes = [(startNode, 0)];
        
        var edgesToTraverse = new PriorityQueue<Edge<TN, TE>, int>(startNode.OutgoingEdges.Select(e => (e, edgePrice(e))));

        while (edgesToTraverse.TryDequeue(out var edge, out var nextNodeRank))
        {
            if (visitedNodes.Any(vn => vn.Node == edge.Target))
                continue;
            visitedNodes.Add((edge.Target, nextNodeRank));
            if (stopPredicate?.Invoke((edge.Target, nextNodeRank)) ?? false) break;
            foreach (var nextEdge in edge.Target.OutgoingEdges)
                edgesToTraverse.Enqueue(nextEdge, nextNodeRank + edgePrice(nextEdge));
        }
        
        var result = visitedNodes.Where(vn => resultPredicate((vn.Node, vn.Rank))).ToList();
        return result;
    }

    public static IReadOnlyCollection<IReadOnlyCollection<(Node<TN, TE> Node, int Rank)>> DepthFirstSearch<TN, TE>(
        Node<TN, TE> startNode,
        Predicate<(Node<TN, TE> Node, int Rank)> targetPredicate)
        => DepthFirstSearch([(startNode, 0)], targetPredicate);
    
    private static IReadOnlyCollection<IReadOnlyCollection<(Node<TN, TE> Node, int Rank)>> DepthFirstSearch<TN, TE>(
        IReadOnlyCollection<(Node<TN, TE> Node, int Rank)> currentPath,
        Predicate<(Node<TN, TE> Node, int Rank)> targetPredicate)
    {
        var currentNode = currentPath.Last();
        if (targetPredicate(currentNode))
            return [currentPath];

        var result = new List<IReadOnlyCollection<(Node<TN, TE> Node, int Rank)>>();
        foreach (var edge in currentNode.Node.OutgoingEdges)
        {
            var nextNodeRank = currentNode.Rank + 1;
            if (currentPath.Any(vn => vn.Node == edge.Target))
                continue;
            
            result.AddRange(DepthFirstSearch(
                currentPath.Concat([(edge.Target, nextNodeRank)]).ToList(), 
                targetPredicate));
        }

        return result;
    }
}