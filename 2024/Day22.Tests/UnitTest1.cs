namespace Day22.Tests;

public class UnitTest1
{
    [Theory]
    [InlineData(42, 15, 37)]
    public void TestMix(long a, long b, long expected)
    {
        Assert.Equal(expected, Solver.Mix(a, b));
    }

    [Theory]
    [InlineData(100000000, 16113920)]
    public void TestPrune(long a, long expected)
    {
        Assert.Equal(expected, Solver.Prune(a));
    }

    [Fact]
    public void TestPRN()
    {
        long sn = 123;
        sn = Solver.Iterate(sn);
        Assert.Equal(15887950, sn);
        sn = Solver.Iterate(sn);
        Assert.Equal(16495136, sn);
        sn = Solver.Iterate(sn);
        Assert.Equal(527345, sn);
        sn = Solver.Iterate(sn);
        Assert.Equal(704524, sn);
        sn = Solver.Iterate(sn);
        Assert.Equal(1553684, sn);
        sn = Solver.Iterate(sn);
        Assert.Equal(12683156, sn);
        sn = Solver.Iterate(sn);
        Assert.Equal(11100544, sn);
        sn = Solver.Iterate(sn);
        Assert.Equal(12249484, sn);
        sn = Solver.Iterate(sn);
        Assert.Equal(7753432, sn);
        sn = Solver.Iterate(sn);
        Assert.Equal(5908254, sn);
    }

    [Theory]
    [InlineData(1, 8685429)]
    [InlineData(10, 4700978)]
    [InlineData(100, 15273692)]
    [InlineData(2024, 8667524)]
    public void TestPRN2(long initial, long expected)
    {
        var sn = initial;
        for (int i = 0; i < 2000; i++)
            sn = Solver.Iterate(sn);
        Assert.Equal(expected, sn);
        Assert.Equal(expected, Solver.Iterate(initial, 2000));
    }

    [Fact]
    public void TestPart1()
    {
        var inputs = new long[] { 1, 10, 100, 2024 };
        Assert.Equal(37327623, Solver.Part1(inputs));
    }

    [Fact]
    public void TestPriceChanges()
    {
        var x = Solver.CalculatePricesAndChanges(123, 9);
        Assert.Collection(
            x,
            a => Assert.Equal((-3, 0), a),
            a => Assert.Equal((6, 6), a),
            a => Assert.Equal((-1, 5), a),
            a => Assert.Equal((-1, 4), a),
            a => Assert.Equal((0, 4), a),
            a => Assert.Equal((2, 6), a),
            a => Assert.Equal((-2, 4), a),
            a => Assert.Equal((0, 4), a),
            a => Assert.Equal((-2, 2), a));
    }
    
    [Fact]
    public void TestPriceChanges2()
    {
        var pcs = Solver.CalculatePricesAndChanges(123, 9);
        var x = Solver.PrecalculatePart2Input(pcs);
        Assert.Collection(
            x,
            a => Assert.Equal((-3, 6, -1, -1, 4), a),
            a => Assert.Equal((6, -1, -1, 0, 4), a),
            a => Assert.Equal((-1, -1, 0, 2, 6), a),
            a => Assert.Equal((-1, 0, 2, -2, 4), a),
            a => Assert.Equal((0, 2, -2, 0, 4), a),
            a => Assert.Equal((2, -2, 0, -2, 2), a));
    }

    [Fact]
    public void TestPart2()
    {
        Assert.Equal(23, Solver.SolvePart2([1, 2, 3, 2024]));
    }
}