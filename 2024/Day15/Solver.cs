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

    public static void WriteState(MapState mapState, TextWriter output, bool wide = false)
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
                        MapObject.Wall => "#",
                        MapObject.Box => wide ? "[]" : "O",
                        MapObject.Empty => "."
                    });
                if (wide && mapState.Map[rowIdx, colIdx] == MapObject.Box) colIdx++;
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
    
    public static long SolvePart2(MapState mapState, IEnumerable<AttemptedMovement> movements)
    {
        mapState = WidenMap(mapState);
        foreach (var movement in movements)
            mapState = SimulateWide(mapState, movement);
        return Evaluate(mapState);
    }
    

    public static MapState WidenMap(MapState narrowMap)
    {
        var map = new MapObject[narrowMap.Map.GetLength(0), narrowMap.Map.GetLength(1) * 2];
        for (int rowIdx = 0; rowIdx < narrowMap.Map.GetLength(0); rowIdx++)
        for (int colIdx = 0; colIdx < narrowMap.Map.GetLength(1); colIdx++)
        {
            var left = narrowMap.Map[rowIdx, colIdx];
            var right = left switch
            {
                MapObject.Wall => MapObject.Wall,
                _ => MapObject.Empty
            };
            map[rowIdx, 2 * colIdx] = left;
            map[rowIdx, 2 * colIdx + 1] = right;
        }
            
        return (map, (narrowMap.RobotPosition.Item1, narrowMap.RobotPosition.Item2 * 2));
    }

    public static (bool, List<(int, int)>) CanMove(MapObject[,] map, (int, int) currentPosition, AttemptedMovement movement)
    {
        var movementVect = MovementToVector(movement);

        var (y0, x0) = (currentPosition.Item1, currentPosition.Item2);
        var (y1, x1) = (y0 + movementVect.Item1, x0 + movementVect.Item2);
        if (map[y1, x1] == MapObject.Wall)
            return (false, []);

        if (movement is AttemptedMovement.Up or AttemptedMovement.Down)
        {
            if (map[y1, x1] == MapObject.Box)
            {
                var (cm1, btm1) = CanMove(map, (y1, x1), movement);
                var (cm2, btm2) = CanMove(map, (y1, x1 + 1), movement);
                if (!(cm1 && cm2))
                    return (false, []);
                var boxesToMove = btm1.Concat(btm2).Concat([(y1, x1)]).Distinct().ToList();
                return (true, boxesToMove);
            }

            if (map[y1, x1 - 1] == MapObject.Box)
            {
                var (cm1, btm1) = CanMove(map, (y1, x1 - 1), movement);
                var (cm2, btm2) = CanMove(map, (y1, x1), movement);
                if (!(cm1 && cm2))
                    return (false, []);
                var boxesToMove = btm1.Concat(btm2).Concat([(y1, x1 - 1)]).Distinct().ToList();
                return (true, boxesToMove);
            }
        }
        else if (movement is AttemptedMovement.Left)
        {
            if (map[y1, x1 - 1] == MapObject.Box)
            {
                var (cm1, btm1) = CanMove(map, (y1, x1 - 1), movement);
                if (!cm1) 
                    return (false, []);
                var boxesToMove = btm1.Concat([(y1, x1 - 1)]).Distinct().ToList();
                return (true, boxesToMove);
            }
        }
        else if (movement is AttemptedMovement.Right)
        {
            if (map[y1, x1] == MapObject.Box)
            {
                var (cm1, btm1) = CanMove(map, (y1, x1 + 1), movement);
                if (!cm1) 
                    return (false, []);
                var boxesToMove = btm1.Concat([(y1, x1)]).Distinct().ToList();
                return (true, boxesToMove);
            }
        }

        return (true, []);
    } 
    
    public static MapState SimulateWide(MapState initialState, AttemptedMovement movement)
    {
        var (canMove, boxesToMove) = CanMove(initialState.Map, initialState.RobotPosition, movement);
        
        if (!canMove)
            return initialState;
    
        var movementVect = MovementToVector(movement);
        var newMap = (MapObject[,])initialState.Map.Clone();
        foreach (var box in boxesToMove)
            newMap[box.Item1, box.Item2] = MapObject.Empty;
        foreach (var box in boxesToMove)
            newMap[box.Item1 + movementVect.Item1, box.Item2 + movementVect.Item2] = MapObject.Box;
        var robotDesiredPosition = (initialState.RobotPosition.Item1 + movementVect.Item1,
            initialState.RobotPosition.Item2 + movementVect.Item2);
        return (newMap, robotDesiredPosition);
    }
}

public enum MapObject { Wall, Empty, Box }
public enum AttemptedMovement { Up, Down, Left, Right }