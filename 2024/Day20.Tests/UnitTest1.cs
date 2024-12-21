using Common.Graphs;

namespace Day20.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var (map, start, end) = Solver.LoadMap(TestInput1);
        var (startNode, endNode, l1Nodes, l1Nodes2d) = Solver.EmptyToGraph(map, start, end);
        var evaluatedNodesSE = GraphSearch.BreadthFirstSearch2(
            startNode,
            _ => 1, 
            _ => true, 
            x => x.Node == endNode);
        var (_, noCheatRank) = evaluatedNodesSE.Single(x => x.Node == endNode);
        Assert.Equal(84, noCheatRank);
        
        var evaluatedNodesES = GraphSearch.BreadthFirstSearch2(
            endNode,
            _ => 1, 
            _ => true,  
            x => x.Node == startNode);
        var (_, noCheatRankES) = evaluatedNodesSE.Single(x => x.Node == endNode);
        Assert.Equal(84, noCheatRankES);
        
        var evaluatedSE = new int?[map.GetLength(0), map.GetLength(1)];
        foreach (var en in evaluatedNodesSE)
            evaluatedSE[en.Node.Value.Item1, en.Node.Value.Item2] = en.Rank;
        var evaluatedES = new int?[map.GetLength(0), map.GetLength(1)];
        foreach (var en in evaluatedNodesES)
            evaluatedES[en.Node.Value.Item1, en.Node.Value.Item2] = en.Rank;
        
        for (var row = 0; row < map.GetLength(0); row++)
        for (var col = 0; col < map.GetLength(1); col++)
        {
            if (evaluatedSE[row, col] == null) continue;
            if (evaluatedES[row, col] == null) continue;

            var concatenated = evaluatedSE[row, col].Value + evaluatedES[row, col].Value;
            Assert.Equal(84, concatenated);
        }
    }

    [Fact]
    public void Test2()
    {
        var (map, start, end) = Solver.LoadMap(TestInput1);
        var res = Solver.CalculateSavedTimePerCheat((map, start, end));
        var resGrouped = res.GroupBy(timeSaved => timeSaved)
            .ToDictionary(g => g.Key, g => g.Count());
        
        Assert.Equal(14, resGrouped[2]);
        Assert.Equal(14, resGrouped[4]);
        Assert.Equal(2, resGrouped[6]);
        Assert.Equal(4, resGrouped[8]);
        Assert.Equal(2, resGrouped[10]);
        Assert.Equal(3, resGrouped[12]);
        Assert.Equal(1, resGrouped[20]);
        Assert.Equal(1, resGrouped[36]);
        Assert.Equal(1, resGrouped[38]);
        Assert.Equal(1, resGrouped[40]);
        Assert.Equal(1, resGrouped[64]);
        
        Assert.Equal(11, resGrouped.Count);
    }

    public const string TestInput1 =
        """
        ###############
        #...#...#.....#
        #.#.#.#.#.###.#
        #S#...#.#.#...#
        #######.#.#.###
        #######.#.#...#
        #######.#.###.#
        ###..E#...#...#
        ###.#######.###
        #...###...#...#
        #.#####.#.###.#
        #.#...#.#.#...#
        #.#.#.#.#.#.###
        #...#...#...###
        ###############

        """;
    
    [Fact]
    public void TestA()
    {
        var (map, start, end) = Solver.LoadMap(TestInputA);
        var res = Solver.CalculateSavedTimePerCheat((map, start, end));
        var resGrouped = res.GroupBy(timeSaved => timeSaved)
            .ToDictionary(g => g.Key, g => g.Count());
        
        Assert.Equal(1, resGrouped[2]);
        Assert.Equal(1, resGrouped.Count);
    }

    public const string TestInputA =
        """
        S#E
        ...
        """;
}