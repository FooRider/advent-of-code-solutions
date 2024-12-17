namespace Day15;

using MapState = (MapObject[,] Map, (int, int) RobotPosition);

public static class Solver
{
    public static (MapState Map, IEnumerable<AttemptedMovement> Movements) LoadInput(TextReader input)
    {
        (int, int) robotPosition = (-1, -1);
        List<MapObject[]> map = [];
        int rowIdx = 0;
        while (input.ReadLine() is { } line && line != "")
        {
            map.Add(line.Select(ch => ch switch
            {
                '#' => MapObject.Wall,
                'O' => MapObject.Box,
                _ => MapObject.Empty
            }).ToArray());
            var robotPos = line.IndexOf('@');
            if (robotPos >= 0)
                robotPosition = (rowIdx, robotPos);
            rowIdx++;
        }

        List<AttemptedMovement> movements = input.ReadToEnd()
            .Where(ch => ch is '<' or '>' or '^' or 'v')
            .Select(ch => ch switch
            {
                '<' => AttemptedMovement.Left,
                '>' => AttemptedMovement.Right,
                '^' => AttemptedMovement.Up,
                'v' => AttemptedMovement.Down,
                _ => throw new Exception()
            })
            .ToList();

        var mapArray = new MapObject[map.Count, map.First().Length];
        for (rowIdx = 0; rowIdx < map.Count; rowIdx++)
        for (int colIdx = 0; colIdx < map.First().Length; colIdx++)
            mapArray[rowIdx, colIdx] = map[rowIdx][colIdx];
        
        return ((mapArray, robotPosition), movements);
    }

    public static MapState Simulate(MapState initialState, AttemptedMovement movement)
    {
        var movementVect = MovementToVector(movement);

        var boxesToMove = new List<(int, int)>();
        var cursor = (initialState.RobotPosition.Item1 + movementVect.Item1, initialState.RobotPosition.Item2 + movementVect.Item2);
        var robotDesiredPosition = cursor;
        while (initialState.Map[cursor.Item1, cursor.Item2] == MapObject.Box)
        {
            boxesToMove.Add((cursor.Item1, cursor.Item2));
            cursor = (cursor.Item1 + movementVect.Item1, cursor.Item2 + movementVect.Item2);
        }

        if (initialState.Map[cursor.Item1, cursor.Item2] == MapObject.Wall) // no movement possible
            return initialState;
        if (!boxesToMove.Any())
            return (initialState.Map, robotDesiredPosition);

        var newMap = (MapObject[,])initialState.Map.Clone();
        newMap[cursor.Item1, cursor.Item2] = MapObject.Box;
        newMap[boxesToMove.First().Item1, boxesToMove.First().Item2] = MapObject.Empty;
        return (newMap, robotDesiredPosition);
    }

    private static (int, int) MovementToVector(AttemptedMovement movement)
        => movement switch
        {
            AttemptedMovement.Up => (-1, 0),
            AttemptedMovement.Down => (1, 0),
            AttemptedMovement.Left => (0, -1),
            AttemptedMovement.Right => (0, 1),
            _ => throw new ArgumentOutOfRangeException(nameof(movement), movement, null)
        };

    public static void WriteState(MapState mapState, TextWriter output)
    {
        for (int rowIdx = 0; rowIdx < mapState.Map.GetLength(0); rowIdx++)
        {
            for (int colIdx = 0; colIdx < mapState.Map.GetLength(1); colIdx++)
            {
                if ((rowIdx, colIdx) == mapState.RobotPosition)
                    output.Write('@');
                else
                    output.Write(mapState.Map[rowIdx, colIdx] switch
                    {
                        MapObject.Wall => '#',
                        MapObject.Box => 'O',
                        MapObject.Empty => '.'
                    });
            }
            output.WriteLine();
        }
        output.WriteLine();
    }

    public static long Evaluate(MapState mapState)
    {
        long sum = 0;
        for (int rowIdx = 0; rowIdx < mapState.Map.GetLength(0); rowIdx++)
        for (int colIdx = 0; colIdx < mapState.Map.GetLength(1); colIdx++)
            if (mapState.Map[rowIdx, colIdx] == MapObject.Box)
                sum += 100 * rowIdx + colIdx;
        return sum;
    }

    public static long SolvePart1(MapState mapState, IEnumerable<AttemptedMovement> movements)
    {
        foreach (var movement in movements)
            mapState = Solver.Simulate(mapState, movement);
        return Evaluate(mapState);
    }
}

public enum MapObject { Wall, Empty, Box }
public enum AttemptedMovement { Up, Down, Left, Right }