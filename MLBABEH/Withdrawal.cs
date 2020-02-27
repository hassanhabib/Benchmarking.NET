using System;
using Microsoft.ML.Data;

namespace MLBABEH
{
    public class Withdrawal
    {
        [LoadColumn(0)]
        public float Amount { get; set; }

        [LoadColumn(1)]
        public DateTime Date { get; set; }
    }
}
