using System.Text;
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

    [Fact]
    public void TestPart2()
    {
        using var sr = new StringReader(TestInput2);
        var (mapState, movements) = Solver.LoadInput(sr);
        mapState = Solver.WidenMap(mapState);
        mapState.Map[4, 10] = MapObject.Box;
        mapState.Map[4, 12] = MapObject.Box;
        mapState.Map[5, 11] = MapObject.Box;
        
        var sw = new StringWriter();
        Solver.WriteState(mapState, sw, true);
        _output.WriteLine(sw.ToString());

        var (canMove, boxesToMove) = Solver.CanMove(mapState.Map, (2, 4), AttemptedMovement.Left);
        Assert.False(canMove); Assert.Empty(boxesToMove);
        (canMove, boxesToMove) = Solver.CanMove(mapState.Map, (2, 5), AttemptedMovement.Left);
        Assert.True(canMove); Assert.Empty(boxesToMove);
        (canMove, boxesToMove) = Solver.CanMove(mapState.Map, (2, 5), AttemptedMovement.Up);
        Assert.True(canMove); Assert.Empty(boxesToMove);
        (canMove, boxesToMove) = Solver.CanMove(mapState.Map, (2, 6), AttemptedMovement.Up);
        Assert.False(canMove); Assert.Empty(boxesToMove);
        
        (canMove, boxesToMove) = Solver.CanMove(mapState.Map, (2, 7), AttemptedMovement.Right);
        Assert.True(canMove);
        Assert.Collection(boxesToMove, i => Assert.Equal((2, 8), i));
        
        (canMove, boxesToMove) = Solver.CanMove(mapState.Map, (2, 10), AttemptedMovement.Left);
        Assert.True(canMove);
        Assert.Collection(boxesToMove, i => Assert.Equal((2, 8), i));
        
        (canMove, boxesToMove) = Solver.CanMove(mapState.Map, (1, 8), AttemptedMovement.Down);
        Assert.True(canMove);
        Assert.Collection(boxesToMove.OrderBy(i => i.Item1), 
            i => Assert.Equal((2, 8), i),
            i => Assert.Equal((3, 8), i),
            i => Assert.Equal((4, 8), i),
            i => Assert.Equal((5, 8), i));
        
        (canMove, boxesToMove) = Solver.CanMove(mapState.Map, (1, 9), AttemptedMovement.Down);
        Assert.True(canMove);
        Assert.Collection(boxesToMove.OrderBy(i => i.Item1), 
            i => Assert.Equal((2, 8), i),
            i => Assert.Equal((3, 8), i),
            i => Assert.Equal((4, 8), i),
            i => Assert.Equal((5, 8), i));
        
        (canMove, boxesToMove) = Solver.CanMove(mapState.Map, (6, 8), AttemptedMovement.Up);
        Assert.True(canMove);
        Assert.Collection(boxesToMove.OrderBy(i => i.Item1), 
            i => Assert.Equal((2, 8), i),
            i => Assert.Equal((3, 8), i),
            i => Assert.Equal((4, 8), i),
            i => Assert.Equal((5, 8), i));
        
        (canMove, boxesToMove) = Solver.CanMove(mapState.Map, (6, 9), AttemptedMovement.Up);
        Assert.True(canMove);
        Assert.Collection(boxesToMove.OrderBy(i => i.Item1), 
            i => Assert.Equal((2, 8), i),
            i => Assert.Equal((3, 8), i),
            i => Assert.Equal((4, 8), i),
            i => Assert.Equal((5, 8), i));
        
        (canMove, boxesToMove) = Solver.CanMove(mapState.Map, (6, 11), AttemptedMovement.Up);
        Assert.True(canMove);
        Assert.Collection(boxesToMove.OrderBy(i => i.Item1).ThenBy(i => i.Item2), 
            i => Assert.Equal((4, 10), i),
            i => Assert.Equal((4, 12), i),
            i => Assert.Equal((5, 11), i));
    }

    [Fact]
    public void Simulate2()
    {
        using var sr = new StringReader(TestInput1);
        var (mapState, movements) = Solver.LoadInput(sr);
        mapState = Solver.WidenMap(mapState);
        
        var sw = new StringWriter();
        Solver.WriteState(mapState, sw, true);
        _output.WriteLine(sw.ToString());

        foreach (var attemptedMovement in movements)
        {
            mapState = Solver.SimulateWide(mapState, attemptedMovement);
            sw = new StringWriter();
            Solver.WriteState(mapState, sw, true);
            _output.WriteLine(sw.ToString());
        }
        
        Assert.Equal(9021, Solver.Evaluate(mapState));
    }

    [Fact]
    public void TestPart2Cases()
    {
        using var sr = new StringReader(TestInput1);
        var (mapState, movements) = Solver.LoadInput(sr);
        Assert.Equal(9021, Solver.SolvePart2(mapState, movements));
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