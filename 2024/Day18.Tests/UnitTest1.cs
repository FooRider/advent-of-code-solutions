namespace Day18.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        using var sr = new StringReader(TestInput1);
        var input = Solver.LoadInput(sr).Take(12).ToList();
        var width = 7;
        var height = 7;
        Assert.Equal(22, Solver.SolvePart1(input, width, height));
    }

    private const string TestInput1 =
        """
        5,4
        4,2
        4,5
        3,0
        2,1
        6,3
        2,4
        1,5
        0,6
        3,3
        2,6
        5,1
        1,2
        5,5
        2,5
        6,5
        1,4
        0,4
        6,4
        1,1
        6,1
        1,0
        0,5
        1,6
        2,0
        """;
}