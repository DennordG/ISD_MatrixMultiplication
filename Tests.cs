using Xunit;

namespace Parallelism;

public class Tests
{
    private readonly int[][] _a;
    private readonly int[][] _b;
    private readonly int[][] _expectedResult;

    public Tests()
    {
        _a = new int[][]
        {
            new int[] {  1, -2,  3 },
            new int[] { -4, -5,  6 },
            new int[] {  7,  8, -9 }
        };

        _b = new int[][]
        {
            new int[] {  5, -1, -3 },
            new int[] {  2, -9, -4 },
            new int[] { -7,  6,  8 }
        };

        _expectedResult = new int[][]
        {
            new int[] { -20,   35,   29 },
            new int[] { -72,   85,   80 },
            new int[] { 114, -133, -125 }
        };
    }

    [Fact]
    public void TestInSerial()
    {
        var m = new MatrixMultiplication(_a, _b);

        var actualResult = m.InSerial();

        Assert.Equal(_expectedResult, actualResult);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(4)]
    public void TestInParallel(int degreeOfParallelism)
    {
        var m = new MatrixMultiplication(_a, _b);

        var actualResult = m.InParallel(degreeOfParallelism);

        Assert.Equal(_expectedResult, actualResult);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(4)]
    public void TestInParallelWithOptimizedMemoryAccess(int degreeOfParallelism)
    {
        var m = new MatrixMultiplication(_a, _b);

        var actualResult = m.InParallelWithOptimizedMemoryAccess(degreeOfParallelism);

        Assert.Equal(_expectedResult, actualResult);
    }
}
