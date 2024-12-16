using Xunit.Abstractions;
using MathNet.Numerics.LinearAlgebra;

namespace Day13.Tests;

public class SolverTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public SolverTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task TestMatrices()
    {
        using var sr = new StringReader(TestInput1);
        var cm1 = await Solver.TryLoadAsync(sr);
        cm1 = await Solver.TryLoadAsync(sr);
        cm1 = await Solver.TryLoadAsync(sr);
        cm1 = await Solver.TryLoadAsync(sr);

        var a = Vector<double>.Build.DenseOfArray(cm1!.Value.A.Select(v => (double)v).ToArray());
        var b = Vector<double>.Build.DenseOfArray(cm1!.Value.B.Select(v => (double)v).ToArray());
        var prize1 = Vector<double>.Build.DenseOfArray(cm1!.Value.Prize.Select(v => (double)v).ToArray());
        var prize2 = Vector<double>.Build.DenseOfArray(cm1!.Value.Prize.Select(v => (double)(v + 10_000_000_000_000)).ToArray());

        var matrix = Matrix<double>.Build.DenseOfColumns([a, b]);
        _testOutputHelper.WriteLine(matrix.ToString());
        var result = matrix.Solve(prize1);
        _testOutputHelper.WriteLine(result.ToString());
        result = matrix.Solve(prize2);
        _testOutputHelper.WriteLine(result.ToString());

        var ta = (long)(result![0]);
        var tb = (long)(result![1]);
        _testOutputHelper.WriteLine($"{ta}\t{tb}");
        _testOutputHelper.WriteLine($"{ta * cm1.Value.A[0] + tb * cm1.Value.B[0]}\t{ta * cm1.Value.A[1] + tb * cm1.Value.B[1]}");
        _testOutputHelper.WriteLine($"{prize2[0]}\t{prize2[1]}");
    }
    
    [Fact]
    public async Task TestPart1()
    {
        using var sr = new StringReader(TestInput1);
        var cm1 = await Solver.TryLoadAsync(sr);
        Assert.Equal(280, Solver.SolveClawMachinePart1(cm1!.Value));
        
        var cm2 = await Solver.TryLoadAsync(sr);
        Assert.Equal(0, Solver.SolveClawMachinePart1(cm2!.Value));
        
        var cm3 = await Solver.TryLoadAsync(sr);
        Assert.Equal(200, Solver.SolveClawMachinePart1(cm3!.Value));
        
        var cm4 = await Solver.TryLoadAsync(sr);
        Assert.Equal(0, Solver.SolveClawMachinePart1(cm4!.Value));
    }

    [Fact]
    public async Task TestPart2Alg()
    {
        using var sr = new StringReader(TestInput1);
        var cm1 = await Solver.TryLoadAsync(sr);
        Assert.Equal(280, Solver.SolveClawMachineNoLimits(cm1!.Value));
        
        var cm2 = await Solver.TryLoadAsync(sr);
        Assert.Equal(0, Solver.SolveClawMachineNoLimits(cm2!.Value));
        
        var cm3 = await Solver.TryLoadAsync(sr);
        Assert.Equal(200, Solver.SolveClawMachineNoLimits(cm3!.Value));
        
        var cm4 = await Solver.TryLoadAsync(sr);
        Assert.Equal(0, Solver.SolveClawMachineNoLimits(cm4!.Value));
    }
    
    [Fact]
    public async Task TestParsing()
    {
        using var sr = new StringReader(TestInput1);
        var cm1 = await Solver.TryLoadAsync(sr);
        var cm2 = await Solver.TryLoadAsync(sr);
        var cm3 = await Solver.TryLoadAsync(sr);
        var cm4 = await Solver.TryLoadAsync(sr);
        var cm5 = await Solver.TryLoadAsync(sr);
        
        var cm = Assert.NotNull(cm1);
        Assert.Equal(94, cm.A[0]);
        Assert.Equal(34, cm.A[1]);
        Assert.Equal(22, cm.B[0]);
        Assert.Equal(67, cm.B[1]);
        Assert.Equal(8400, cm.Prize[0]);
        Assert.Equal(5400, cm.Prize[1]);
        
        cm = Assert.NotNull(cm2);
        Assert.Equal(26, cm.A[0]);
        Assert.Equal(66, cm.A[1]);
        Assert.Equal(67, cm.B[0]);
        Assert.Equal(21, cm.B[1]);
        Assert.Equal(12748, cm.Prize[0]);
        Assert.Equal(12176, cm.Prize[1]);
        
        cm = Assert.NotNull(cm3);
        Assert.Equal(17, cm.A[0]);
        Assert.Equal(86, cm.A[1]);
        Assert.Equal(84, cm.B[0]);
        Assert.Equal(37, cm.B[1]);
        Assert.Equal(7870, cm.Prize[0]);
        Assert.Equal(6450, cm.Prize[1]);
        
        cm = Assert.NotNull(cm4);
        Assert.Equal(69, cm.A[0]);
        Assert.Equal(23, cm.A[1]);
        Assert.Equal(27, cm.B[0]);
        Assert.Equal(71, cm.B[1]);
        Assert.Equal(18641, cm.Prize[0]);
        Assert.Equal(10279, cm.Prize[1]);
        
        Assert.Null(cm5);
    }

    public string TestInput1_1 = 
        """
        Button A: X+94, Y+34
        Button B: X+22, Y+67
        Prize: X=8400, Y=5400
        
        
        """;

    public string TestInput1_2 = 
        """
        Button A: X+26, Y+66
        Button B: X+67, Y+21
        Prize: X=12748, Y=12176
        
        
        """;

    public string TestInput1_3 =
        """
        Button A: X+17, Y+86
        Button B: X+84, Y+37
        Prize: X=7870, Y=6450


        """;

    public string TestInput1_4 =
        """
        Button A: X+69, Y+23
        Button B: X+27, Y+71
        Prize: X=18641, Y=10279
        
        
        """;
    
    public string TestInput1 => TestInput1_1 + TestInput1_2 + TestInput1_3 + TestInput1_4;
}