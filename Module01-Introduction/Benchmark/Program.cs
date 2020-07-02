using System.Diagnostics;
using System.Reflection;
using BenchmarkDotNet.Running;

namespace Dotnetos.AsyncExpert.Homework.Module01.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            var fc = new FibonacciCalc();

            foreach (var n in fc.Data())
            {
                var recursive = fc.Recursive(n);
                var recursiveWithMemoization = fc.RecursiveWithMemoization(n);
                var iterative = fc.Iterative(n);

                Debug.Assert(recursive == recursiveWithMemoization);
                Debug.Assert(recursive == iterative);
            }
#endif

            BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly()).Run(args);
        }
    }
}
