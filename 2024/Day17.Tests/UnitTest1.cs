namespace Day17.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        using var sr = new StringReader(TestInput1);
        var state = ProcessorState.Load(sr);
        state = Solver.Step(state);
        Assert.Equal(1, state.RegisterB);
    }
    
    [Fact]
    public void Test2()
    {
        using var sr = new StringReader(TestInput2);
        var state = ProcessorState.Load(sr);
        for (int i = 0; i < 3; i++)
            state = Solver.Step(state);
        Assert.Equal("0,1,2", state.Output);
    }
    
    [Fact]
    public void Test3()
    {
        using var sr = new StringReader(TestInput3);
        var state = ProcessorState.Load(sr);
        state = Solver.Run(state);
        Assert.Equal("4,2,5,6,7,7,7,7,3,1,0", state.Output);
    }
    
    [Fact]
    public void Test4()
    {
        using var sr = new StringReader(TestInput4);
        var state = ProcessorState.Load(sr);
        state = Solver.Run(state);
        Assert.Equal(26, state.RegisterB);
    }
    
    [Fact]
    public void Test5()
    {
        using var sr = new StringReader(TestInput5);
        var state = ProcessorState.Load(sr);
        state = Solver.Run(state);
        Assert.Equal(44354, state.RegisterB);
    }

    [Fact]
    public void Test0()
    {
        using var sr = new StringReader(TestInput);
        var state = ProcessorState.Load(sr);
        state = Solver.Run(state);
        Assert.Equal("4,6,3,5,6,3,5,2,1,0", state.Output);
    }

    [Fact]
    public void TestCombo()
    {
        var ps = new ProcessorState([], 0, 1, 2, 3, "");
        Assert.Equal(0, Solver.EvaluateComboOperand(ps, 0));
        Assert.Equal(1, Solver.EvaluateComboOperand(ps, 1));
        Assert.Equal(2, Solver.EvaluateComboOperand(ps, 2));
        Assert.Equal(3, Solver.EvaluateComboOperand(ps, 3));
        
        Assert.Equal(1, Solver.EvaluateComboOperand(ps, 4));
        Assert.Equal(2, Solver.EvaluateComboOperand(ps, 5));
        Assert.Equal(3, Solver.EvaluateComboOperand(ps, 6));
        ps = ps with { RegisterA = 10, RegisterB = 11, RegisterC = 12 };
        Assert.Equal(10, Solver.EvaluateComboOperand(ps, 4));
        Assert.Equal(11, Solver.EvaluateComboOperand(ps, 5));
        Assert.Equal(12, Solver.EvaluateComboOperand(ps, 6));
    }

    [Fact]
    public void TestAdv()
    {
        var ps = new ProcessorState([0, 0], 0, 1024, 0, 0, "");
        var ps0 = ps;
        var ps1 = Solver.Step(ps0);
        Assert.Equal(1024, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);

        ps0 = ps with { ProgramInstructions = [0, 1] };
        ps1 = Solver.Step(ps0);
        Assert.Equal(512, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [0, 2] };
        ps1 = Solver.Step(ps0);
        Assert.Equal(256, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [0, 3] };
        ps1 = Solver.Step(ps0);
        Assert.Equal(128, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [0, 5] };
        ps1 = Solver.Step(ps0);
        Assert.Equal(1024, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [0, 5], RegisterB = 4};
        ps1 = Solver.Step(ps0);
        Assert.Equal(64, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [0, 5], RegisterB = 5};
        ps1 = Solver.Step(ps0);
        Assert.Equal(32, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [0, 5], RegisterB = 6};
        ps1 = Solver.Step(ps0);
        Assert.Equal(16, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [0, 5], RegisterB = 7};
        ps1 = Solver.Step(ps0);
        Assert.Equal(8, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [0, 5], RegisterB = 8};
        ps1 = Solver.Step(ps0);
        Assert.Equal(4, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [0, 5], RegisterB = 9};
        ps1 = Solver.Step(ps0);
        Assert.Equal(2, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [0, 5], RegisterB = 10};
        ps1 = Solver.Step(ps0);
        Assert.Equal(1, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [0, 5], RegisterB = 11};
        ps1 = Solver.Step(ps0);
        Assert.Equal(0, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
    }

    [Fact]
    public void TestBxl()
    {
        var ps = new ProcessorState([1, 0], 0, 0, 0, 0, "");
        var ps0 = ps;
        var ps1 = Solver.Step(ps0);
        Assert.Equal(0, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);

        ps0 = ps with { ProgramInstructions = [1, 1] };
        ps1 = Solver.Step(ps0);
        Assert.Equal(1, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [1, 2] };
        ps1 = Solver.Step(ps0);
        Assert.Equal(2, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [1, 7] };
        ps1 = Solver.Step(ps0);
        Assert.Equal(7, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [1, 7], RegisterB = 7};
        ps1 = Solver.Step(ps0);
        Assert.Equal(0, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [1, 7], RegisterB = 1023};
        ps1 = Solver.Step(ps0);
        Assert.Equal(1016, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [1, 0], RegisterB = 1023};
        ps1 = Solver.Step(ps0);
        Assert.Equal(1023, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [1, 4], RegisterB = 1023};
        ps1 = Solver.Step(ps0);
        Assert.Equal(1019, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
    }
    
    [Fact]
    public void TestBst()
    {
        var ps = new ProcessorState([2, 0], 0, 0, 0, 0, "");
        var ps0 = ps;
        var ps1 = Solver.Step(ps0);
        Assert.Equal(0, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [2, 1] };
        ps1 = Solver.Step(ps0);
        Assert.Equal(1, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [2, 2] };
        ps1 = Solver.Step(ps0);
        Assert.Equal(2, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [2, 3] };
        ps1 = Solver.Step(ps0);
        Assert.Equal(3, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [2, 4] };
        ps1 = Solver.Step(ps0);
        Assert.Equal(0, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [2, 4], RegisterA = 1 };
        ps1 = Solver.Step(ps0);
        Assert.Equal(1, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [2, 4], RegisterA = 10 };
        ps1 = Solver.Step(ps0);
        Assert.Equal(2, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [2, 5], RegisterB = 17 };
        ps1 = Solver.Step(ps0);
        Assert.Equal(1, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [2, 6], RegisterC = 11 };
        ps1 = Solver.Step(ps0);
        Assert.Equal(3, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
    }

    [Fact]
    public void TestJnz()
    {
        var ps = new ProcessorState([3, 1], 0, 0, 0, 0, "");
        var ps0 = ps;
        var ps1 = Solver.Step(ps0);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);

        ps0 = ps with { ProgramInstructions = [3, 1], RegisterA = 1 };
        ps1 = Solver.Step(ps0);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(1, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [3, 1], RegisterA = 1024 };
        ps1 = Solver.Step(ps0);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(1, ps1.InstructionPointer);
    }
    
    [Fact]
    public void TestBxc()
    {
        var ps = new ProcessorState([4, 0], 0, 0, 0, 0, "");
        var ps0 = ps;
        var ps1 = Solver.Step(ps0);
        Assert.Equal(0, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);

        ps0 = ps with { ProgramInstructions = [4, 0], RegisterB = 1, RegisterC = 1 };
        ps1 = Solver.Step(ps0);
        Assert.Equal(0, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [4, 0], RegisterB = 2, RegisterC = 1 };
        ps1 = Solver.Step(ps0);
        Assert.Equal(3, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [4, 0], RegisterB = 2047, RegisterC = 1 };
        ps1 = Solver.Step(ps0);
        Assert.Equal(2046, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
    }

    [Fact]
    public void TestOut()
    {
        var ps = new ProcessorState([5, 0], 0, 0, 0, 0, "");
        var ps0 = ps;
        var ps1 = Solver.Step(ps0);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("0", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);

        ps0 = ps with { ProgramInstructions = [5, 1] };
        ps1 = Solver.Step(ps0);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("1", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [5, 2] };
        ps1 = Solver.Step(ps0);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("2", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [5, 3] };
        ps1 = Solver.Step(ps0);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("3", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [5, 4] };
        ps1 = Solver.Step(ps0);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("0", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [5, 5] };
        ps1 = Solver.Step(ps0);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("0", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [5, 6] };
        ps1 = Solver.Step(ps0);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("0", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [5, 4], RegisterA = 15};
        ps1 = Solver.Step(ps0);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("7", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [5, 5], RegisterB = 66 };
        ps1 = Solver.Step(ps0);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("2", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [5, 6], RegisterC = 67};
        ps1 = Solver.Step(ps0);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("3", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
    }
    
    [Fact]
    public void TestBdv()
    {
        var ps = new ProcessorState([6, 0], 0, 1024, 0, 0, "");
        var ps0 = ps;
        var ps1 = Solver.Step(ps0);
        Assert.Equal(1024, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);

        ps0 = ps with { ProgramInstructions = [6, 1] };
        ps1 = Solver.Step(ps0);
        Assert.Equal(512, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [6, 2] };
        ps1 = Solver.Step(ps0);
        Assert.Equal(256, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [6, 3] };
        ps1 = Solver.Step(ps0);
        Assert.Equal(128, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [6, 5] };
        ps1 = Solver.Step(ps0);
        Assert.Equal(1024, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [6, 5], RegisterB = 4};
        ps1 = Solver.Step(ps0);
        Assert.Equal(64, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [6, 5], RegisterB = 5};
        ps1 = Solver.Step(ps0);
        Assert.Equal(32, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [6, 5], RegisterB = 6};
        ps1 = Solver.Step(ps0);
        Assert.Equal(16, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [6, 5], RegisterB = 7};
        ps1 = Solver.Step(ps0);
        Assert.Equal(8, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [6, 5], RegisterB = 8};
        ps1 = Solver.Step(ps0);
        Assert.Equal(4, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [6, 5], RegisterB = 9};
        ps1 = Solver.Step(ps0);
        Assert.Equal(2, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [6, 5], RegisterB = 10};
        ps1 = Solver.Step(ps0);
        Assert.Equal(1, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [6, 5], RegisterB = 11};
        ps1 = Solver.Step(ps0);
        Assert.Equal(0, ps1.RegisterB);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterC, ps1.RegisterC);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
    }
    
    [Fact]
    public void TestCdv()
    {
        var ps = new ProcessorState([7, 0], 0, 1024, 0, 0, "");
        var ps0 = ps;
        var ps1 = Solver.Step(ps0);
        Assert.Equal(1024, ps1.RegisterC);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);

        ps0 = ps with { ProgramInstructions = [7, 1] };
        ps1 = Solver.Step(ps0);
        Assert.Equal(512, ps1.RegisterC);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [7, 2] };
        ps1 = Solver.Step(ps0);
        Assert.Equal(256, ps1.RegisterC);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [7, 3] };
        ps1 = Solver.Step(ps0);
        Assert.Equal(128, ps1.RegisterC);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [7, 5] };
        ps1 = Solver.Step(ps0);
        Assert.Equal(1024, ps1.RegisterC);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [7, 5], RegisterB = 4};
        ps1 = Solver.Step(ps0);
        Assert.Equal(64, ps1.RegisterC);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [7, 5], RegisterB = 5};
        ps1 = Solver.Step(ps0);
        Assert.Equal(32, ps1.RegisterC);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [7, 5], RegisterB = 6};
        ps1 = Solver.Step(ps0);
        Assert.Equal(16, ps1.RegisterC);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [7, 5], RegisterB = 7};
        ps1 = Solver.Step(ps0);
        Assert.Equal(8, ps1.RegisterC);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [7, 5], RegisterB = 8};
        ps1 = Solver.Step(ps0);
        Assert.Equal(4, ps1.RegisterC);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [7, 5], RegisterB = 9};
        ps1 = Solver.Step(ps0);
        Assert.Equal(2, ps1.RegisterC);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [7, 5], RegisterB = 10};
        ps1 = Solver.Step(ps0);
        Assert.Equal(1, ps1.RegisterC);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
        
        ps0 = ps with { ProgramInstructions = [7, 5], RegisterB = 11};
        ps1 = Solver.Step(ps0);
        Assert.Equal(0, ps1.RegisterC);
        Assert.Equal(ps0.RegisterA, ps1.RegisterA);
        Assert.Equal(ps0.RegisterB, ps1.RegisterB);
        Assert.Equal("", ps1.Output);
        Assert.Equal(2, ps1.InstructionPointer);
    }

    [Fact]
    public void Part2Test1()
    {
        using var sr = new StringReader(TestInputP2);
        var ps = ProcessorState.Load(sr);
        Assert.False(Solver.DoesPrintItself(ps));
        Assert.True(Solver.DoesPrintItself(ps with { RegisterA = 117440 }));
    } 

    public const string TestInput =
        """
        Register A: 729
        Register B: 0
        Register C: 0
        
        Program: 0,1,5,4,3,0
        """;

    public const string TestInput1 =
        """
        Register A: 0
        Register B: 0
        Register C: 9
        
        Program: 2,6
        """;
    
    public const string TestInput2 =
        """
        Register A: 10
        Register B: 0
        Register C: 0
        
        Program: 5,0,5,1,5,4
        """;
    
    public const string TestInput3 =
        """
        Register A: 2024
        Register B: 0
        Register C: 0
        
        Program: 0,1,5,4,3,0
        """;
    
    public const string TestInput4 =
        """
        Register A: 0
        Register B: 29
        Register C: 0
        
        Program: 1,7
        """;
    
    public const string TestInput5 =
        """
        Register A: 0
        Register B: 2024
        Register C: 43690

        Program: 4,0
        """;

    public const string TestInputP2 =
        """
        Register A: 2024
        Register B: 0
        Register C: 0

        Program: 0,3,5,4,3,0
        """;
}