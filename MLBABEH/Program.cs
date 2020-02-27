using System;
using System.Collections.Generic;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace MLBABEH
{
    class Program
    {
        static void Main(string[] args)
        {
            var mlContext = new MLContext();

            var listOfWithdrawals = new List<Withdrawal>
            {
                new Withdrawal
                {
                    Amount = 300,
                    Date = DateTime.UtcNow
                },
                new Withdrawal
                {
                    Amount = 200,
                    Date = DateTime.UtcNow.AddDays(-1)
                },
                new Withdrawal
                {
                    Amount = 150,
                    Date = DateTime.UtcNow.AddDays(-2)
                },
                new Withdrawal
                {
                    Amount = 3000,
                    Date = DateTime.UtcNow.AddDays(-3)
                },
                new Withdrawal
                {
                    Amount = 75,
                    Date = DateTime.UtcNow.AddDays(-3)
                },
                new Withdrawal
                {
                    Amount = 375,
                    Date = DateTime.UtcNow.AddDays(-3)
                },
                new Withdrawal
                {
                    Amount = 175,
                    Date = DateTime.UtcNow.AddDays(-3)
                },
            };


            var pipeLine = mlContext.Transforms.DetectIidSpike(
                nameof(Prediction.Predication),
                nameof(Withdrawal.Amount),
                confidence: 99,
                pvalueHistoryLength: listOfWithdrawals.Count);

            IDataView dataView = mlContext.Data.LoadFromEnumerable(listOfWithdrawals);

            var transformer = pipeLine.Fit(dataView);
            IDataView transformedData = transformer.Transform(dataView);

            var predictions = mlContext.Data.CreateEnumerable<Prediction>(transformedData, false);


            foreach (var p in predictions)
            {
                Console.WriteLine($"{p.Predication[0]}\t{p.Predication[1]}\t{p.Predication[2]}");
            }
        }

        class Prediction
        {
            [VectorType(3)]
            public double[] Predication { get; set; }
        }
    }
}
