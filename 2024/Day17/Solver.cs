using System.Collections.Immutable;
using System.Diagnostics;
using System.Xml;

namespace Day17;

public static class Solver
{
    public static ProcessorState Run(ProcessorState state)
    {
        while (state.InstructionPointer < state.ProgramInstructions.Length)
            state = Step(state);
        return state;
    }
    
    public static ProcessorState Step(ProcessorState state)
    {
        var instruction = state.ProgramInstructions[state.InstructionPointer];
        var operand = state.ProgramInstructions[state.InstructionPointer + 1]; 
        return instruction switch
        {
            0 => state with // adv
            {
                RegisterA = state.RegisterA / (1 << (int)EvaluateComboOperand(state, operand)),
                InstructionPointer = state.InstructionPointer + 2
            },
            1 => state with // bxl
            {
                RegisterB = state.RegisterB ^ operand,
                InstructionPointer = state.InstructionPointer + 2
            },
            2 => state with // bst
            {
                RegisterB = EvaluateComboOperand(state, operand) % 8,
                InstructionPointer = state.InstructionPointer + 2
            },
            3 when state.RegisterA != 0 => state with // jnz
            {
                InstructionPointer = operand
            },
            3 when state.RegisterA == 0 => state with // jnz
            {
                InstructionPointer = state.InstructionPointer + 2
            },
            4 => state with // bxc
            {
                RegisterB = state.RegisterB ^ state.RegisterC,
                InstructionPointer = state.InstructionPointer + 2
            },
            5 => state with //  out
            {
                Output = state.Output switch
                {
                    "" => (EvaluateComboOperand(state, operand) % 8).ToString(),
                    _ => state.Output + "," + (EvaluateComboOperand(state, operand) % 8)
                },
                InstructionPointer = state.InstructionPointer + 2,
            },
            6 => state with // bdv
            {
                RegisterB = state.RegisterA / (1 << (int)EvaluateComboOperand(state, operand)),
                InstructionPointer = state.InstructionPointer + 2,
            },
            7 => state with // cdv
            {
                RegisterC = state.RegisterA / (1 << (int)EvaluateComboOperand(state, operand)),
                InstructionPointer = state.InstructionPointer + 2,
            }
        };
    }

    public static long EvaluateComboOperand(ProcessorState state, byte operand) =>
        operand switch
        {
            < 4 => operand,
            4 => state.RegisterA,
            5 => state.RegisterB,
            6 => state.RegisterC,
            _ => throw new Exception()
        };

    public static bool DoesPrintItself(ProcessorState state)
        => DoesPrintItself(state, out var _);
    
    public static bool DoesPrintItself(ProcessorState state, out int achievedLength)
    {
        int outWriteIdx = achievedLength = 0;
        while (state.InstructionPointer < state.ProgramInstructions.Length)
        {
            var nextState = Solver.Step(state);
            if (nextState.Output.Length > state.Output.Length)
            {
                if (outWriteIdx >= state.ProgramInstructions.Length)
                    return false;
                
                var newOutput = long.Parse(nextState.Output[^1..]);
                if (newOutput != state.ProgramInstructions[outWriteIdx])
                    return false;
                outWriteIdx++;
                achievedLength = outWriteIdx;
            }
            
            state = nextState;
        }

        return outWriteIdx == state.ProgramInstructions.Length;
    }

    public static long Part2SpecificSolver(ProcessorState state)
    {
        var adepts = GetAdepts(state.ProgramInstructions.AsSpan(), 0);
        foreach (var adept in adepts.OrderBy(a => a))
        {
            Console.WriteLine($"{adept} -> {Solver.DoesPrintItself(state with { RegisterA = adept})}");
        }

        return adepts.Min();
        
        IEnumerable<long> GetAdepts(ReadOnlySpan<byte> remainingOutput, long desiredA)
        {
            if (remainingOutput.Length == 0)
                return [desiredA];
            
            var result = new List<long>();
            for (int i = 0; i < (1 << 6); i++)
            {
                long probedA = (desiredA << 3) ^ i;
                var (output, nextA) = Iterate(probedA);
                if (output == remainingOutput[^1] && nextA == desiredA)
                {
                    Console.WriteLine($"{probedA}\t{Convert.ToString(probedA, 8)}\tnext: {nextA}");
                    result.AddRange(GetAdepts(remainingOutput[..^1], probedA));
                }
            }
            return result;
        }
        
        // My program: 2,4, 1,2, 7,5, 4,3, 0,3, 1,7, 5,5, 3,0
        // BST 4    -> BST a    -> b = a % 8
        // BXL 2    -> BXL 2    -> b = b ^ 2
        // CDV 5    -> CDV b    -> c = a >> b
        // BXC 3    -> BXC      -> b = b ^ c
        // ADV 3    -> ADV 3    -> a = a >> 3
        // BXL 7    -> BXL 7    -> b = b ^ 7
        // OUT 5    -> OUT b    -> OUT b % 8
        // JNZ 0    -> JNZ 0
        (byte output, long nextA) Iterate(long a)
        {
            long b = (a % 8) ^ 2;
            b = ((b ^ (a >> (int)b)) ^ 7) % 8;
            return ((byte)b, a >> 3);
        }
    }
}

public record struct ProcessorState(
    ImmutableArray<byte> ProgramInstructions,
    int InstructionPointer,
    long RegisterA,
    long RegisterB,
    long RegisterC,
    string Output)
{
    public static ProcessorState Load(TextReader input)
    {
        var line = input.ReadLine() ?? throw new Exception();
        if (!line.StartsWith("Register A: ")) throw new Exception();
        long.TryParse(line.Substring("Register A: ".Length), out var registerA);
        
        line = input.ReadLine() ?? throw new Exception();
        if (!line.StartsWith("Register B: ")) throw new Exception();
        long.TryParse(line.Substring("Register B: ".Length), out var registerB);
        
        line = input.ReadLine() ?? throw new Exception();
        if (!line.StartsWith("Register C: ")) throw new Exception();
        long.TryParse(line.Substring("Register C: ".Length), out var registerC);
        
        line = input.ReadLine() ?? throw new Exception();
        if (line != "") throw new Exception();

        var instructions = new List<byte>();
        line = input.ReadLine() ?? throw new Exception();
        if (!line.StartsWith("Program: ")) throw new Exception();
        var ls = line.AsSpan()[("Program: ".Length)..];
        var instructionEnumerator = ls.Split(',');
        while (instructionEnumerator.MoveNext())
            instructions.Add(byte.Parse(ls[instructionEnumerator.Current]));

        return new ProcessorState(
            [..instructions], 0,
            registerA, registerB, registerC, "");
    }
};