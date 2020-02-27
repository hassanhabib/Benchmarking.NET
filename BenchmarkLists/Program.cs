using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace BenchmarkLists
{
    class Program
    {
        static void Main(string[] args)
        {
            Summary summary = BenchmarkRunner.Run<BenchmarkLists>();

            Console.WriteLine(summary);
        }
    }

    public class BenchmarkLists
    {
        private readonly List<int> numbers;

        public BenchmarkLists()
        {
            this.numbers = Enumerable.Range(1, 1000).ToList();
        }

        [Benchmark]
        public bool UsingAny()
        {
            return this.numbers.Any();
        }

        [Benchmark]
        public bool UsingCountProperty()
        {
            return this.numbers.Count > 0;
        }

        [Benchmark]
        public bool UsingCountFunction()
        {
            return this.numbers.Count() > 0;
        }
    }
}
