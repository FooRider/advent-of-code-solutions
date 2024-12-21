namespace Day19.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        using var sr = new StringReader(TestInput1);
        var (available, desired) = Solver.Load(sr);
        
        Assert.True(Solver.CanCombine(desired[0], available, []));
        Assert.True(Solver.CanCombine(desired[1], available, []));
        Assert.True(Solver.CanCombine(desired[2], available, []));
        Assert.True(Solver.CanCombine(desired[3], available, []));
        Assert.False(Solver.CanCombine(desired[4], available, []));
        Assert.True(Solver.CanCombine(desired[5], available, []));
        Assert.True(Solver.CanCombine(desired[6], available, []));
        Assert.False(Solver.CanCombine(desired[7], available, []));
    }
    
    [Fact]
    public void Test2()
    {
        using var sr = new StringReader(TestInput1);
        var (available, desired) = Solver.Load(sr);
        
        Assert.Equal(2, Solver.WaysToCombine(desired[0], available, []));
        Assert.Equal(1, Solver.WaysToCombine(desired[1], available, []));
        Assert.Equal(4, Solver.WaysToCombine(desired[2], available, []));
        Assert.Equal(6, Solver.WaysToCombine(desired[3], available, []));
        Assert.Equal(0, Solver.WaysToCombine(desired[4], available, []));
        Assert.Equal(1, Solver.WaysToCombine(desired[5], available, []));
        Assert.Equal(2, Solver.WaysToCombine(desired[6], available, []));
        Assert.Equal(0, Solver.WaysToCombine(desired[7], available, []));
    }

    public const string TestInput1 =
        """
        r, wr, b, g, bwu, rb, gb, br

        brwrr
        bggr
        gbbr
        rrbgbr
        ubwu
        bwurrg
        brgr
        bbrgwb
        """;
}