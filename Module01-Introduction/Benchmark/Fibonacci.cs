using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;

namespace Dotnetos.AsyncExpert.Homework.Module01.Benchmark
{
    [DisassemblyDiagnoser(exportCombinedDisassemblyReport: true)]
    [NativeMemoryProfiler]
    public class FibonacciCalc
    {
        // HOMEWORK:
        // 1. Write implementations for RecursiveWithMemoization and Iterative solutions
        // 2. Add MemoryDiagnoser to the benchmark
        // 3. Run with release configuration and compare results
        // 4. Open disassembler report and compare machine code
        // 
        // You can use the discussion panel to compare your results with other students

        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(Data))]
        public ulong Recursive(ulong n)
        {
            if (n == 1 || n == 2) return 1;
            return Recursive(n - 1) + Recursive(n - 2);
        }

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public ulong RecursiveWithMemoization(ulong n)
        {
            var lookup = new ulong[n + 1];
            return RecurseWithLookup(n, lookup);
        }

        private ulong RecurseWithLookup(ulong n, ulong[] lookup)
        {
            if (n == 1 || n == 2) return 1;

            if (lookup[n] == default)
                lookup[n] = RecurseWithLookup(n - 1, lookup) + RecurseWithLookup(n - 2, lookup);

            return lookup[n];
        }

        
        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public ulong Iterative(ulong n)
        {
            if (n == 1 || n == 2) return 1;

            ulong number = 1;
            ulong previousNumber = 1;
		
            for(ulong i = 2; i < n; i++) { 
                var temp = number;
                number += previousNumber;
                previousNumber = temp;
            }
            
            return number;
        }

        public IEnumerable<ulong> Data()
        {
            yield return 15;
            yield return 35;
        }
    }
}
