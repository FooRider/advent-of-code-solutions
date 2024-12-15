namespace Day11.Tests;

public class UnitTest1
{
    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(9, 1)]
    [InlineData(10, 2)]
    [InlineData(11, 2)]
    [InlineData(99, 2)]
    [InlineData(100, 3)]
    [InlineData(999, 3)]
    [InlineData(1_000, 4)]
    [InlineData(9_999, 4)]
    [InlineData(10_000, 5)]
    [InlineData(99_999, 5)]
    [InlineData(100_000, 6)]
    [InlineData(999_999, 6)]
    [InlineData(1_000_000, 7)]
    [InlineData(9_999_999, 7)]
    [InlineData(10_000_000, 8)]
    [InlineData(99_999_999, 8)]
    [InlineData(100_000_000, 9)]
    [InlineData(999_999_999, 9)]
    [InlineData(1_000_000_000, 10)]
    [InlineData(9_999_999_999, 10)]
    [InlineData(10_000_000_000, 11)]
    [InlineData(99_999_999_999, 11)]
    [InlineData(100_000_000_000, 12)]
    [InlineData(999_999_999_999, 12)]
    [InlineData(1_000_000_000_000, 13)]
    [InlineData(9_999_999_999_999, 13)]
    [InlineData(10_000_000_000_000, 14)]
    [InlineData(99_999_999_999_999, 14)]
    [InlineData(100_000_000_000_000, 15)]
    [InlineData(999_999_999_999_999, 15)]
    public void NumberOfDigitsTest(long number, int expected)
    {
        Assert.Equal(Day11Solver.NumberOfDigits(number), expected);
        Assert.Equal(expected, number.ToString().Length);
    }
}