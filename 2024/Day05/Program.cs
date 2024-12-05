const string inputFile = "input1.txt";

List<(int, int)> orderingRules = [];
List<List<int>> pageOrders = [];

await using var fh = File.OpenRead(inputFile);
using var sr = new StreamReader(fh);

while (await sr.ReadLineAsync() is { } line && !string.IsNullOrEmpty(line))
{
    var ls = line.AsSpan();
    var parts = ls.Split('|');
    if (!parts.MoveNext()) throw new Exception();
    var p1 = int.Parse(ls[parts.Current]);
    if (!parts.MoveNext()) throw new Exception();
    var p2 = int.Parse(ls[parts.Current]);
    orderingRules.Add((p1, p2));
}

while (await sr.ReadLineAsync() is { } line && !string.IsNullOrEmpty(line))
{
    var pages = new List<int>();
    var ls = line.AsSpan();
    var parts = ls.Split(',');
    while (parts.MoveNext())
        pages.Add(int.Parse(ls[parts.Current]));
    pageOrders.Add(pages);
}

{
    Console.WriteLine("Part 1");

    
    var passedSum = 0;

    foreach (var po in pageOrders)
    {
        var rules = orderingRules.Select(pages => new RuleStatus(pages.Item1, pages.Item2)).ToList();
        Console.WriteLine(string.Join(',', po));
        int idx = 0;
        foreach (var p in po)
        {
            idx++;
            rules.Where(r => r.Page1 == p && r.Position1 == null).ToList().ForEach(r => r.Position1 = idx);
            rules.Where(r => r.Page2 == p && r.Position2 == null).ToList().ForEach(r => r.Position2 = idx);
        }
        
        var failingRules = rules.Where(r => r.Position1.HasValue && r.Position2.HasValue && r.Position2.Value < r.Position1.Value).ToList();

        if (!failingRules.Any())
            passedSum += po[(po.Count / 2)];
    }
    Console.WriteLine(passedSum);
}

record RuleStatus(int Page1, int Page2)
{
    public int? Position1 { get; set; }
    public int? Position2 { get; set; }
};