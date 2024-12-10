Console.WriteLine("Hello World!");

record Node<TN, TE>(TN Value)
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

record Edge<TN, TE>(TE Value, Node<TN, TE> Source, Node<TN, TE> Target);