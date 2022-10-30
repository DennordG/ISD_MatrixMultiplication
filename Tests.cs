using Xunit;

namespace Parallelism;

public class Tests
{
    [Fact]
    public void TestInSerial()
    {
        var a = new int[][]
        {
            new int[] { 1, 0, 0 },
            new int[] { 0, 1, 0 },
            new int[] { 0, 0, 1 }
        };

        var b = new int[][]
        {
            new int[] { 5, 1, 3 },
            new int[] { 2, 9, 4 },
            new int[] { 7, 6, 8 }
        };

        var m = new MatrixMultiplication(a, b);

        var res = m.InSerial();

        Assert.Equal(b, res);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(4)]
    public void TestInParallel(int degreeOfParallelism)
    {
        var a = new int[][]
        {
            new int[] { 1, 0, 0 },
            new int[] { 0, 1, 0 },
            new int[] { 0, 0, 1 }
        };

        var b = new int[][]
        {
            new int[] { 5, 1, 3 },
            new int[] { 2, 9, 4 },
            new int[] { 7, 6, 8 }
        };

        var m = new MatrixMultiplication(a, b);

        var res = m.InParallel(degreeOfParallelism);

        Assert.Equal(b, res);
    }
}
