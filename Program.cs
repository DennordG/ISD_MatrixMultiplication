using BenchmarkDotNet.Running;

namespace Parallelism;

public class Program
{
    public static void Main() => BenchmarkRunner.Run<MatrixMultiplication>();
}