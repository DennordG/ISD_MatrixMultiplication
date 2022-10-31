using BenchmarkDotNet.Attributes;

namespace Parallelism;

[MemoryDiagnoser(false)]
[ThreadingDiagnoser]
public class MatrixMultiplication
{
    private int[][] _a;
    private int[][] _b;

    // This is only for benchmark
    public MatrixMultiplication() { }

    public MatrixMultiplication(int[][] a, int[][] b)
    {
        if (a.Length != b.Length || !a.Select(l => l.Length).SequenceEqual(b.Select(l => l.Length)))
        {
            throw new ArgumentException("Both matrices must have the same size.");
        }

        _a = a;
        _b = b;
    }

    [Params(10, 100, 1000)]
    public int N;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _a = Enumerable.Range(1, N).Select(i => Enumerable.Range(1, N).ToArray()).ToArray();
        _b = Enumerable.Range(1, N).Select(i => Enumerable.Range(1, N).ToArray()).ToArray();
    }

    [Benchmark(Baseline = true)]
    public int[][] InSerial()
    {
        var n = _a.Length;
        var result = new int[n][];

        for (var i = 0; i < n; i++)
        {
            result[i] = new int[n];

            for (var j = 0; j < n; j++)
            {
                for (var k = 0; k < n; k++)
                {
                    result[i][j] += _a[i][k] * _b[k][j];
                }
            }
        }

        return result;
    }

    [Benchmark]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(4)]
    public int[][] InParallel(int degreeOfParalellism)
    {
        var n = _a.Length;
        var result = new int[n][];

        var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = degreeOfParalellism };

        Parallel.For(0, n, parallelOptions, i =>
        {
            result[i] = new int[n];

            for (var j = 0; j < n; j++)
            {
                for (var k = 0; k < n; k++)
                {
                    result[i][j] += _a[i][k] * _b[k][j];
                }
            }
        });

        return result;
    }

    [Benchmark]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(4)]
    public int[][] InParallelWithOptimizedMemoryAccess(int degreeOfParalellism)
    {
        var n = _a.Length;
        var result = new int[n][];

        var transposedB = Transpose(_b);

        var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = degreeOfParalellism };

        Parallel.For(0, n, parallelOptions, i =>
        {
            result[i] = new int[n];

            for (var j = 0; j < n; j++)
            {
                for (var k = 0; k < n; k++)
                {
                    result[i][j] += _a[i][k] * transposedB[j][k];
                }
            }
        });

        return result;
    }

    private static int[][] Transpose(int[][] matrix)
    {
        var n = matrix.Length;
        var transpose = new int[n][];

        for (var i = 0; i < n; i++)
        {
            transpose[i] = new int[n];
            for (var j = 0; j < n; j++)
            {
                transpose[i][j] = matrix[j][i];
            }
        }

        return transpose;
    }
}
