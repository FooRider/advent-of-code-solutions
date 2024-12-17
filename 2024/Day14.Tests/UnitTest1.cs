using Xunit.Abstractions;

namespace Day14.Tests;

public class UnitTest1
{
    private readonly ITestOutputHelper _outputHelper;

    public UnitTest1(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }
    
    [Fact]
    public async Task TestLoading()
    {
        using var sr = new StringReader(Input1);

        var r = Assert.NotNull(Robot.TryParse(sr));
        Assert.Equal(0, r.InitialPosition[0]);
        Assert.Equal(4, r.InitialPosition[1]);
        Assert.Equal(3, r.Velocity[0]);
        Assert.Equal(-3, r.Velocity[1]);
        for (int i = 0; i < 11; i++)
            Assert.NotNull(Robot.TryParse(sr));
        Assert.Null(Robot.TryParse(sr));
    }

    [Fact]
    public void TestSimulation()
    {
        var robot = new Robot([2, 4], [2, -3]);
        int mw = 11;
        int mh = 7;
        Assert.Equal((2, 4), Solver.GetPart1PositionAfterTime(robot, mw, mh, 0));
        Assert.Equal((4, 1), Solver.GetPart1PositionAfterTime(robot, mw, mh, 1));
        Assert.Equal((6, 5), Solver.GetPart1PositionAfterTime(robot, mw, mh, 2));
        Assert.Equal((8, 2), Solver.GetPart1PositionAfterTime(robot, mw, mh, 3));
        Assert.Equal((10, 6), Solver.GetPart1PositionAfterTime(robot, mw, mh, 4));
        Assert.Equal((1, 3), Solver.GetPart1PositionAfterTime(robot, mw, mh, 5));
    }

    [Fact]
    public void TestQuadrantAssignment()
    {
        int mw = 11;
        int mh = 7;
        Assert.Equal(1, Solver.GetQuadrant((0, 2), mw, mh));
        Assert.Equal(2, Solver.GetQuadrant((6, 0), mw, mh));
        Assert.Equal(2, Solver.GetQuadrant((9, 0), mw, mh));
        Assert.Equal(3, Solver.GetQuadrant((3, 5), mw, mh));
        Assert.Equal(3, Solver.GetQuadrant((4, 5), mw, mh));
        Assert.Equal(3, Solver.GetQuadrant((1, 6), mw, mh));
        Assert.Equal(4, Solver.GetQuadrant((6, 6), mw, mh));
        Assert.Equal(0, Solver.GetQuadrant((5, 0), mw, mh));
        Assert.Equal(0, Solver.GetQuadrant((5, 3), mw, mh));
        Assert.Equal(0, Solver.GetQuadrant((0, 3), mw, mh));
        Assert.Equal(0, Solver.GetQuadrant((10, 3), mw, mh));
    }

    [Fact]
    public async Task TestPart1()
    {
        int mw = 11;
        int mh = 7;
        using var sr = new StringReader(Input1);

        var positions = new List<(int, int)>();
        
        Dictionary<int, int> quadrants = new() { {0, 0}, {1, 0}, {2, 0}, {3, 0}, {4, 0} };
        while (Robot.TryParse(sr) is { } robot)
        {
            var simulatedPosition = Solver.GetPart1PositionAfterTime(robot, mw, mh, 100);
            positions.Add(simulatedPosition);
        }
        foreach (var position in positions) //.Distinct()) 
        {
            var quadrant = Solver.GetQuadrant(position, mw, mh);
            quadrants[quadrant]++;
        }
        Assert.Equal(1, quadrants[1]);
        Assert.Equal(3, quadrants[2]);
        Assert.Equal(4, quadrants[3]);
        Assert.Equal(1, quadrants[4]);
    }

    [Fact]
    public async Task TestPart1Full()
    {
        using var sr = new StringReader(Input1);
        var robots = Solver.ReadRobots(sr);
        Assert.Equal(12, Solver.SolvePart1(robots, 11, 7));
    }

    public const string Input1 = 
        """
        p=0,4 v=3,-3
        p=6,3 v=-1,-3
        p=10,3 v=-1,2
        p=2,0 v=2,-1
        p=0,0 v=1,3
        p=3,0 v=-2,-2
        p=7,6 v=-1,-3
        p=3,0 v=-1,-2
        p=9,3 v=2,3
        p=7,3 v=-1,2
        p=2,4 v=2,-3
        p=9,5 v=-3,-3
        """;
}