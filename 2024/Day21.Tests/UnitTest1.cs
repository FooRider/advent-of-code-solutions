using Xunit.Abstractions;

namespace Day21.Tests;

public class UnitTest1
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTest1(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    public void Test1()
    {
        Assert.Equal((0, 0), Solver.FindChar(Solver.NumericKeypad, '7'));
        Assert.Equal((0, 1), Solver.FindChar(Solver.NumericKeypad, '8'));
        Assert.Equal((0, 2), Solver.FindChar(Solver.NumericKeypad, '9'));
        Assert.Equal((1, 0), Solver.FindChar(Solver.NumericKeypad, '4'));
        Assert.Equal((1, 1), Solver.FindChar(Solver.NumericKeypad, '5'));
        Assert.Equal((1, 2), Solver.FindChar(Solver.NumericKeypad, '6'));
        Assert.Equal((2, 0), Solver.FindChar(Solver.NumericKeypad, '1'));
        Assert.Equal((2, 1), Solver.FindChar(Solver.NumericKeypad, '2'));
        Assert.Equal((2, 2), Solver.FindChar(Solver.NumericKeypad, '3'));
        Assert.Equal((3, 0), Solver.FindChar(Solver.NumericKeypad, ' '));
        Assert.Equal((3, 1), Solver.FindChar(Solver.NumericKeypad, '0'));
        Assert.Equal((3, 2), Solver.FindChar(Solver.NumericKeypad, 'A'));
        
        Assert.Equal((0, 0), Solver.FindChar(Solver.DirectionalKeypad, ' '));
        Assert.Equal((0, 1), Solver.FindChar(Solver.DirectionalKeypad, '^'));
        Assert.Equal((0, 2), Solver.FindChar(Solver.DirectionalKeypad, 'A'));
        Assert.Equal((1, 0), Solver.FindChar(Solver.DirectionalKeypad, '<'));
        Assert.Equal((1, 1), Solver.FindChar(Solver.DirectionalKeypad, 'v'));
        Assert.Equal((1, 2), Solver.FindChar(Solver.DirectionalKeypad, '>'));
    }

    [Theory]
    [InlineData('A', '0', "<A")]
    [InlineData('A', '1', "^<<A")]
    [InlineData('A', '2', "<^A")]
    [InlineData('A', '3', "^A")]
    [InlineData('A', '4', "^^<<A")]
    [InlineData('A', '5', "<^^A")]
    [InlineData('A', '6', "^^A")]
    [InlineData('A', '7', "^^^<<A")]
    [InlineData('A', '8', "<^^^A")]
    [InlineData('A', '9', "^^^A")]
    
    [InlineData('7', '0', ">vvvA")]
    [InlineData('7', '1', "vvA")]
    [InlineData('7', '2', "vv>A")]
    [InlineData('7', '3', "vv>>A")]
    [InlineData('7', '4', "vA")]
    [InlineData('7', '5', "v>A")]
    [InlineData('7', '6', "v>>A")]
    [InlineData('7', '7', "A")]
    [InlineData('7', '8', ">A")]
    [InlineData('7', '9', ">>A")]
    
    [InlineData('8', '0', "vvvA")]
    [InlineData('8', '1', "<vvA")]
    [InlineData('8', '2', "vvA")]
    [InlineData('8', '3', "vv>A")]
    [InlineData('8', '4', "<vA")]
    [InlineData('8', '5', "vA")]
    [InlineData('8', '6', "v>A")]
    [InlineData('8', '7', "<A")]
    [InlineData('8', '8', "A")]
    [InlineData('8', '9', ">A")]
    
    public void Test2(char prevChar, char targetChar, string expectedSequence)
    {
        var k1State0 = Solver.FindChar(Solver.NumericKeypad, prevChar);
        var k1State1 = Solver.FindChar(Solver.NumericKeypad, targetChar);
        var k2Seq = Solver.GetDirectionalButtonSequence(Solver.NumericKeypad, k1State0, k1State1);
        Assert.Equal(expectedSequence, k2Seq);
    }

    [Fact]
    public void Test2a()
    {
        Assert.Equal("^>A", Solver.GetDirectionalButtonSequence(Solver.NumericKeypad, (1, 0), (0, 1)));
        Assert.Equal("<Av<A", Solver.GetDirectionalButtonSequence(Solver.DirectionalKeypad, "^<"));
    }

    [Theory]
    [InlineData(Solver.NumericKeypad, "029A", "<A^A^^>AvvvA")]
    [InlineData(Solver.DirectionalKeypad, "^", "<A")]
    [InlineData(Solver.DirectionalKeypad, ">", "vA")]
    [InlineData(Solver.DirectionalKeypad, "v", "<vA")]
    [InlineData(Solver.DirectionalKeypad, "<", "v<<A")]
    public void Test3(string layout, string inputSequence, string expectedSequence)
    {
        Assert.Equal(expectedSequence, Solver.GetDirectionalButtonSequence(layout, inputSequence));
    }

    [Theory]
    [InlineData("029A", 68)]
    [InlineData("980A", 60)]
    [InlineData("179A", 68)]
    [InlineData("456A", 64)]
    [InlineData("379A", 64)]
    public void Test4(string input, long expectedValue)
    {
        var x = Solver.GetNthDBS(
            new[] {Solver.NumericKeypad, Solver.DirectionalKeypad, Solver.DirectionalKeypad},
            input);
        Assert.Equal(expectedValue, x.Length);
    }
    
    [Theory]
    [InlineData("029A", 68*29)]
    [InlineData("980A", 60*980)]
    [InlineData("179A", 68*179)]
    [InlineData("456A", 64*456)]
    [InlineData("379A", 64*379)]
    public void Test5(string input, long value)
    {
        Assert.Equal(value, Solver.SolveSinglePart1(input));
    }
}