namespace Day12.Tests;

public class SolverTests
{
    private const string TestInput0 = """
                                      AAAA
                                      BBCD
                                      BBCC
                                      EEEC
                                      """;

    private const string TestInput1 = """
                                      OOOOO
                                      OXOXO
                                      OOOOO
                                      OXOXO
                                      OOOOO
                                      """;

    private const string TestInput2 = """
                                      RRRRIICCFF
                                      RRRRIICCCF
                                      VVRRRCCFFF
                                      VVRCCCJFFF
                                      VVVVCJJCFE
                                      VVIVCCJJEE
                                      VVIIICJJEE
                                      MIIIIIJJEE
                                      MIIISIJEEE
                                      MMMISSJEEE
                                      """;
    
    [Theory]
    [InlineData(TestInput0, 4, 4)]
    [InlineData(TestInput1, 5, 5)]
    [InlineData(TestInput2, 10, 10)]
    public async Task TestLoad(string testInput, int columns, int rows)
    {
        using var sr = new StringReader(testInput);
        var map = await Solver.LoadMapAsync(sr);
        Assert.Equal(columns, map.GetLength(0));
        Assert.Equal(rows, map.GetLength(1));
    }

    [Fact]
    public async Task TestExtractRegion1()
    {
        using var sr = new StringReader(TestInput0);
        var map = await Solver.LoadMapAsync(sr);

        var mask = new bool[map.GetLength(0), map.GetLength(1)];
        for (var x = 0; x < map.GetLength(0); x++)
        for (var y = 0; y < map.GetLength(1); y++)
            mask[x, y] = true;

        var r1 = Solver.ExtractRegion(map, mask, 0, 0);
        Assert.Equal('A', r1.Item1);
        Assert.Equal(4, AllValues(r1.Item2).Count(v => v));
        
        var r2 = Solver.ExtractRegion(map, mask, 3, 3);
        Assert.Equal('C', r2.Item1);
        Assert.Equal(4, AllValues(r2.Item2).Count(v => v));
    }
    
    [Theory]
    [InlineData(TestInput0, 5)]
    [InlineData(TestInput1, 5)]
    [InlineData(TestInput2, 11)]
    public async Task TestRegionCount(string testInput, int expectedRegionCount)
    {
        using var sr = new StringReader(testInput);
        var map = await Solver.LoadMapAsync(sr);
        var regionCount = Solver.ExtractAllRegions(map).Count();
        Assert.Equal(expectedRegionCount, regionCount);
    }

    [Theory]
    [InlineData(TestInput0, 140)]
    [InlineData(TestInput1, 772)]
    [InlineData(TestInput2, 1930)]
    public async Task Part1TestCases(string testInput, long expectedAnswer)
    {
        using var sr = new StringReader(testInput);
        var map = await Solver.LoadMapAsync(sr);
        var answer = Solver.Part1(map);
        Assert.Equal(expectedAnswer, answer);
    }
    
    private IEnumerable<T> AllValues<T>(T[,] array) 
    {
        for (int i = 0; i < array.GetLength(0); i++)
        for (int j = 0; j < array.GetLength(1); j++)
            yield return array[i, j];
    }
}