using Xunit.Abstractions;
using Node1 = Common.Graphs.Node<string, int>;

namespace Day23.Tests;

public class UnitTest1
{
    private readonly ITestOutputHelper _output;

    public UnitTest1(ITestOutputHelper output)
    {
        _output = output;
    }
    
    [Fact]
    public void Test1()
    {
        var nodes = Solver.LoadGraph(TestInput1);
        Assert.NotEmpty(nodes);
        var triplets = Solver.ListInterconnectedTriplets(nodes);
        Assert.Equal(12, triplets.Count);
        AssertContains("aq", "cg", "yn");
        AssertContains("aq", "vc", "wq");
        AssertContains("co", "de", "ka");
        AssertContains("co", "de", "ta");
        AssertContains("co", "ka", "ta");
        AssertContains("de", "ka", "ta");
        AssertContains("kh", "qp", "ub");
        AssertContains("qp", "td", "wh");
        AssertContains("tb", "vc", "wq");
        AssertContains("tc", "td", "wh");
        AssertContains("td", "wh", "yn");
        AssertContains("ub", "vc", "wq");

        var result = Solver.SolvePart1(nodes);
        Assert.Equal(7, result);

        void AssertContains(string n1, string n2, string n3)
        {
            Assert.Contains(triplets, t => t.Any(n => n.Value == n1) && t.Any(n => n.Value == n2) && t.Any(n => n.Value == n3));
        }
    }

    [Fact]
    public void TestK4()
    {
        var nodes = Solver.LoadGraph(TestInputK4);
        var oneTuplets = nodes.Select(n => new List<Node1>([n])).ToList();
        var twoTuplets = Solver.FindLargerCompleteSubgraphs(oneTuplets);
        Assert.Equal(6, twoTuplets.Count);
        var threeTuplets = Solver.FindLargerCompleteSubgraphs(twoTuplets);
        Assert.Equal(4, threeTuplets.Count);
        var fourTuplets = Solver.FindLargerCompleteSubgraphs(threeTuplets);
        Assert.Single(fourTuplets);
    }
    
    [Fact]
    public void TestK5()
    {
        var nodes = Solver.LoadGraph(TestInputK5);
        var oneTuplets = nodes.Select(n => new List<Node1>([n])).ToList();
        var twoTuplets = Solver.FindLargerCompleteSubgraphs(oneTuplets);
        Assert.Equal(10, twoTuplets.Count);
        var threeTuplets = Solver.FindLargerCompleteSubgraphs(twoTuplets);
        Assert.Equal(10, threeTuplets.Count);
        var fourTuplets = Solver.FindLargerCompleteSubgraphs(threeTuplets);
        Assert.Equal(5, fourTuplets.Count);
        var fiveTuplets = Solver.FindLargerCompleteSubgraphs(fourTuplets);
        Assert.Single(fiveTuplets);
    }

    [Fact]
    public void TestPart2()
    {
        var nodes = Solver.LoadGraph(TestInput1);
        var largestComplete = Solver.FindLargestCompleteSubgraphs(nodes);
        var subgraph = Assert.Single(largestComplete);
        Assert.Equal(4, subgraph.Count);
        var password = string.Join(',', subgraph.Select(n => n.Value));
        Assert.Equal("co,de,ka,ta", password);
    } 

    public const string TestInput1 = """
                                     kh-tc
                                     qp-kh
                                     de-cg
                                     ka-co
                                     yn-aq
                                     qp-ub
                                     cg-tb
                                     vc-aq
                                     tb-ka
                                     wh-tc
                                     yn-cg
                                     kh-ub
                                     ta-co
                                     de-co
                                     tc-td
                                     tb-wq
                                     wh-td
                                     ta-ka
                                     td-qp
                                     aq-cg
                                     wq-ub
                                     ub-vc
                                     de-ta
                                     wq-aq
                                     wq-vc
                                     wh-yn
                                     ka-de
                                     kh-ta
                                     co-tc
                                     wh-qp
                                     tb-vc
                                     td-yn
                                     """;

    public const string TestInputK4 = """
                                      aa-bb
                                      aa-cc
                                      aa-dd
                                      bb-cc
                                      bb-dd
                                      cc-dd
                                      """;
    
    public const string TestInputK5 = """
                                      aa-bb
                                      aa-cc
                                      aa-dd
                                      aa-ee
                                      bb-cc
                                      bb-dd
                                      bb-ee
                                      cc-dd
                                      cc-ee
                                      dd-ee
                                      """;
}