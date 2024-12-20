using Common.Graphs;

namespace Day16.Tests;

public class UnitTest1
{
    [Theory]
    [InlineData(Input1, 7036, 45)]
    [InlineData(Input2, 11048, 64)]
    public void Test1(string input, long expectedPart1, long expectedPart2)
    {
        using var sr = new StringReader(input);
        var (map, start, end) = Solver.LoadInput(sr);
        var (startNode, targetNodes) = Solver.ToGraphPart1(map, start, end);
        var result1a = Solver.SolvePart1(startNode, targetNodes);
        var (result1b, result2) = Solver.SolvePart12(startNode, targetNodes);
        Assert.Equal(expectedPart1, result1a);
        Assert.Equal(expectedPart1, result1b);
        Assert.Equal(expectedPart2, result2);
    }

    [Fact]
    public void TestGraphP1()
    {
        using var sr = new StringReader(Input1);
        var (map, start, end) = Solver.LoadInput(sr);
        var (startNode, targetNodes) = Solver.ToGraphPart1(map, start, end);
        
        var res = GraphSearch.BreadthFirstSearch(startNode, _ => true);

        foreach (var (n, rank) in res)
        {
            var rotCount = 0;
            foreach (var e in n.OutgoingEdges)
            {
                if (e.Target.Value.X == n.Value.X && e.Target.Value.Y == n.Value.Y)
                {
                    Assert.NotEqual(n.Value.O, e.Target.Value.O);
                    Assert.Equal(1000, e.Value);
                    switch (n.Value.O)
                    {
                        case Orientation.East:
                            Assert.True(e.Target.Value.O is Orientation.North or Orientation.South); break;
                        case Orientation.West:
                            Assert.True(e.Target.Value.O is Orientation.North or Orientation.South); break;
                        case Orientation.North:
                            Assert.True(e.Target.Value.O is Orientation.East or Orientation.West); break;
                        case Orientation.South:
                            Assert.True(e.Target.Value.O is Orientation.East or Orientation.West); break;
                    }

                    rotCount++;
                }
                else
                {
                    if (e.Target.Value.X == n.Value.X)
                    {
                        Assert.Equal(n.Value.X, e.Target.Value.X);
                        Assert.NotEqual(n.Value.Y, e.Target.Value.Y);
                    }
                    else
                    {
                        Assert.Equal(n.Value.Y, e.Target.Value.Y);
                        Assert.NotEqual(n.Value.X, e.Target.Value.X);
                    }

                    Assert.Equal(1, e.Value);
                }
            }

            Assert.Equal(2, rotCount);
        }
    }

    public const string Input1 =
        """
        ###############
        #.......#....E#
        #.#.###.#.###.#
        #.....#.#...#.#
        #.###.#####.#.#
        #.#.#.......#.#
        #.#.#####.###.#
        #...........#.#
        ###.#.#####.#.#
        #...#.....#.#.#
        #.#.#.###.#.#.#
        #.....#...#.#.#
        #.###.#.#.#.#.#
        #S..#.....#...#
        ###############
        """;

    public const string Input2 =
        """
        #################
        #...#...#...#..E#
        #.#.#.#.#.#.#.#.#
        #.#.#.#...#...#.#
        #.#.#.#.###.#.#.#
        #...#.#.#.....#.#
        #.#.#.#.#.#####.#
        #.#...#.#.#.....#
        #.#.#####.#.###.#
        #.#.#.......#...#
        #.#.###.#####.###
        #.#.#...#.....#.#
        #.#.#.#####.###.#
        #.#.#.........#.#
        #.#.#.#########.#
        #S#.............#
        #################
        """;
}