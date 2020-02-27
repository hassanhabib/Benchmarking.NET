using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using MoreLinq;

namespace DistinctListExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Summary summary = BenchmarkRunner.Run<MyBechmarkProcess>();

            Console.WriteLine(summary);
        }
    }

    public class MyBechmarkProcess
    {
        private List<int> numbers;

        public MyBechmarkProcess()
        {
            this.numbers = Enumerable.Range(1, 1000).ToList();
        }

        [Benchmark]
        public bool UsinAny()
        {
            return this.numbers.Any();
        }

        [Benchmark]
        public bool UsingCount()
        {
            return this.numbers.Count > 0;
        }
    }
}
