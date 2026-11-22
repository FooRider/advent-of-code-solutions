using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Day21;

public static class Solver
{
    public const string NumericKeypad =
        """
        789
        456
        123
         0A
        """;

    public const string DirectionalKeypad =
        """
         ^A
        <v>
        """;

    private static Dictionary<string, Dictionary<char, (int Row, int Col)>> layoutCache = new();
    private static Dictionary<char, (int Row, int Col)> CreateCache(string layout)
    {
        var res = new Dictionary<char, (int Row, int Col)>();
        
        var lines = layout.Split('\n');
        int row = 0;
        foreach (var line in lines)
        {
            for (int col = 0; col < line.Length; col++)
                res[line[col]] = (row, col);
            row++;
        }
        
        return res;
    }

    public static (int Row, int Col) FindChar(string layout, char ch)
    {
        if (!layoutCache.ContainsKey(layout))
            layoutCache[layout] = CreateCache(layout);
        return layoutCache[layout][ch];
    }

    public static string GetDirectionalButtonSequence(
        string layout,
        (int Row, int Col) source, 
        (int Row, int Col) target)
    {
        var rowDiff = target.Row - source.Row;
        var colDiff = target.Col - source.Col;
        bool? verticalFirst = null;
        
        if (layout == NumericKeypad)
        {
            if (source.Row == 3 && target.Col == 0) verticalFirst = true;
            else if (source.Col == 0 && target.Row == 3) verticalFirst = false;
        }
        else if (layout == DirectionalKeypad)
        {
            if (source.Col == 0) verticalFirst = false;
            else if (target.Col == 0) verticalFirst = true;
        }

        if (verticalFirst is null)
        {
            if (colDiff < 0) verticalFirst = false;
            else verticalFirst = true;
        }
        
        var sb = new StringBuilder();
        if (verticalFirst!.Value)
        {
            for (int i = 0; i < Math.Abs(rowDiff); i++) sb.Append(rowDiff < 0 ? '^' : 'v');
            for (int i = 0; i < Math.Abs(colDiff); i++) sb.Append(colDiff < 0 ? '<' : '>');
        }
        else
        {
            for (int i = 0; i < Math.Abs(colDiff); i++) sb.Append(colDiff < 0 ? '<' : '>');
            for (int i = 0; i < Math.Abs(rowDiff); i++) sb.Append(rowDiff < 0 ? '^' : 'v');
        }
        sb.Append('A');
        return sb.ToString();
    }

    public static string GetDirectionalButtonSequence(string layout, string inputSequence)
    {
        var sb = new StringBuilder();
        var state0 = FindChar(layout, 'A');
        foreach (var ch in inputSequence)
        {
            var state1 = FindChar(layout, ch);
            sb.Append(GetDirectionalButtonSequence(layout, state0, state1));
            state0 = state1;
        }
        return sb.ToString();
    }

    public static string GetNthDBS(string[] layouts, string inputSequence)
    {
        foreach (var layout in layouts)
            inputSequence = GetDirectionalButtonSequence(layout, inputSequence);
        return inputSequence;
    }

    public static long SolveSinglePart1(string input)
    {
        var len = GetNthDBS(
            new[] {NumericKeypad, DirectionalKeypad, DirectionalKeypad},
            input).Length;
        var numVal = long.Parse(input.TrimStart('0').TrimEnd('A'));
        return len * numVal;
    }

    public static Dictionary<(char, char), string> BuildCacheOfShortestPaths(string layout)
    {
        var res = new Dictionary<(char, char), string>();
        foreach (var ch0 in layout.Where(ch => !char.IsWhiteSpace(ch)))
        foreach (var ch1 in layout.Where(ch => !char.IsWhiteSpace(ch)))
            res[(ch0, ch1)] = GetDirectionalButtonSequence(layout, FindChar(layout, ch0), FindChar(layout, ch1));
        return res;
    }

    public static long SolveSinglePart2(string input)
    {
        var cn = BuildCacheOfShortestPaths(NumericKeypad);
        var ck = BuildCacheOfShortestPaths(DirectionalKeypad);

        var dirInput = GetNthDBS(new[] { NumericKeypad }, input);
        dir
        
        
        var layouts = new List<string>();
        layouts.Add(NumericKeypad);
        for (int i = 0; i < 25; i++)
            layouts.Add(DirectionalKeypad);
        var len = GetNthDBS(
            layouts.ToArray(),
            input).Length;
        var numVal = long.Parse(input.TrimStart('0').TrimEnd('A'));
        return len * numVal;
    }
}