using Newtonsoft.Json;
using Xunit.Abstractions;

namespace Day15.Tests;

using MapState = (MapObject[,] Map, (int, int) RobotPosition);

public class UnitTest1
{
    private readonly ITestOutputHelper _output;

    public UnitTest1(ITestOutputHelper output)
    {
        _output = output;
    }
    
    [Fact]
    public void LoadTest1()
    {
        using var sr = new StringReader(TestInput2);
        var (mapState, movements) = Solver.LoadInput(sr);
        Assert.Equal(8, mapState.Map.GetLength(0));
        Assert.Equal(8, mapState.Map.GetLength(1));
        Assert.Equal(2, mapState.RobotPosition.Item1);
        Assert.Equal(2, mapState.RobotPosition.Item2);
        
        Assert.Collection(movements,
            m => Assert.Equal(AttemptedMovement.Left, m),
            m => Assert.Equal(AttemptedMovement.Up, m),
            m => Assert.Equal(AttemptedMovement.Up, m),
            m => Assert.Equal(AttemptedMovement.Right, m),
            m => Assert.Equal(AttemptedMovement.Right, m),
            m => Assert.Equal(AttemptedMovement.Right, m),
            m => Assert.Equal(AttemptedMovement.Down, m),
            m => Assert.Equal(AttemptedMovement.Down, m),
            m => Assert.Equal(AttemptedMovement.Left, m),
            m => Assert.Equal(AttemptedMovement.Down, m),
            m => Assert.Equal(AttemptedMovement.Right, m),
            m => Assert.Equal(AttemptedMovement.Right, m),
            m => Assert.Equal(AttemptedMovement.Down, m),
            m => Assert.Equal(AttemptedMovement.Left, m),
            m => Assert.Equal(AttemptedMovement.Left, m));
    }

    [Fact]
    public void Simulate1()
    {
        using var sr = new StringReader(TestInput2);
        var (mapState, movements) = Solver.LoadInput(sr);

        var sw = new StringWriter();
        Solver.WriteState(mapState, sw);
        _output.WriteLine(sw.ToString());
        
        foreach (var movement in movements)
        {
            mapState = Solver.Simulate(mapState, movement);
            
            sw = new StringWriter();
            Solver.WriteState(mapState, sw);
            _output.WriteLine(sw.ToString());
        }
        Assert.Equal(4, mapState.RobotPosition.Item1);
        Assert.Equal(4, mapState.RobotPosition.Item2);
    }

    [Theory]
    [InlineData(TestInput2, 2028)]
    [InlineData(TestInput1, 10092)]
    public void TestPart1Cases(string input, long expectedResult)
    {
        using var sr = new StringReader(input);
        var (mapState, movements) = Solver.LoadInput(sr);
        Assert.Equal(expectedResult, Solver.SolvePart1(mapState, movements));
    }

    [Fact]
    public void TestEvaluate()
    {
        using var sr = new StringReader(TestInput3);
        var (mapState, movements) = Solver.LoadInput(sr);
        Assert.Equal(104, Solver.Evaluate(mapState));
    }
    
    public const string TestInput2 =
        """
        ########
        #..O.O.#
        ##@.O..#
        #...O..#
        #.#.O..#
        #...O..#
        #......#
        ########

        <^^>>>vv<v>>v<<
        """;

    public const string TestInput1 =
        """
        ##########
        #..O..O.O#
        #......O.#
        #.OO..O.O#
        #..O@..O.#
        #O#..O...#
        #O..O..O.#
        #.OO.O.OO#
        #....O...#
        ##########

        <vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
        vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
        ><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
        <<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
        ^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
        ^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
        >^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
        <><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
        ^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
        v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^
        """;

    public const string TestInput3 =
        """
        #######
        #...O..
        #......


        """;
}