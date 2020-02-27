using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace IterationProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            var mlConext = new MLContext();

            List<string> texts = new List<string> { "Hassan", "Hussein", "John" };

            IDataView dataView = mlConext.Data.LoadFromEnumerable(texts);

            DetectSpike(mlConext, 3, dataView);
        }

        static void DetectSpike(MLContext mlContext, int docSize, IDataView productSales)
        {
            Console.WriteLine("Detect temporary changes in pattern");

            // STEP 2: Set the training algorithm   
            // <SnippetAddSpikeTrainer> 
            var iidSpikeEstimator = mlContext.Transforms.DetectIidSpike(outputColumnName: "Prediction", inputColumnName: "ColumnName", confidence: 95, pvalueHistoryLength: docSize / 4);
            // </SnippetAddSpikeTrainer> 

            // STEP 3: Create the transform
            // Create the spike detection transform
            Console.WriteLine("=============== Training the model ===============");
            // <SnippetTrainModel1>
            ITransformer iidSpikeTransform = iidSpikeEstimator.Fit(CreateEmptyDataView(mlContext));
            // </SnippetTrainModel1>

            Console.WriteLine("=============== End of training process ===============");
            //Apply data transformation to create predictions.
            // <SnippetTransformData1>
            IDataView transformedData = iidSpikeTransform.Transform(productSales);
            // </SnippetTransformData1>

            // <SnippetCreateEnumerable1>
            var predictions = mlContext.Data.CreateEnumerable<PersonPrediction>(transformedData, reuseRowObject: false);
            // </SnippetCreateEnumerable1>

            // <SnippetDisplayHeader1>
            Console.WriteLine("Alert\tScore\tP-Value");
            // </SnippetDisplayHeader1>

            // <SnippetDisplayResults1>
            foreach (var p in predictions)
            {
                var results = $"{p.Prediction[0]}\t{p.Prediction[1]:f2}\t{p.Prediction[2]:F2}";

                if (p.Prediction[0] == 1)
                {
                    results += " <-- Spike detected";
                }

                Console.WriteLine(results);
            }
            Console.WriteLine("");
            // </SnippetDisplayResults1>
        }

        static void DetectChangepoint(MLContext mlContext, int docSize, IDataView productSales)
        {
            Console.WriteLine("Detect Persistent changes in pattern");

            //STEP 2: Set the training algorithm 
            // <SnippetAddChangePointTrainer> 
            var iidChangePointEstimator = mlContext.Transforms.DetectIidChangePoint(outputColumnName: "Prediction", inputColumnName: "ColumnName", confidence: 95, changeHistoryLength: docSize / 4);
            // </SnippetAddChangePointTrainer> 

            //STEP 3: Create the transform
            Console.WriteLine("=============== Training the model Using Change Point Detection Algorithm===============");
            // <SnippetTrainModel2>
            var iidChangePointTransform = iidChangePointEstimator.Fit(CreateEmptyDataView(mlContext));
            // </SnippetTrainModel2>
            Console.WriteLine("=============== End of training process ===============");

            //Apply data transformation to create predictions.
            // <SnippetTransformData2>
            IDataView transformedData = iidChangePointTransform.Transform(productSales);
            // </SnippetTransformData2>

            // <SnippetCreateEnumerable2>
            var predictions = mlContext.Data.CreateEnumerable<PersonPrediction>(transformedData, reuseRowObject: false);
            // </SnippetCreateEnumerable2>

            // <SnippetDisplayHeader2>
            Console.WriteLine("Alert\tScore\tP-Value\tMartingale value");
            // </SnippetDisplayHeader2>

            // <SnippetDisplayResults2>
            foreach (var p in predictions)
            {
                var results = $"{p.Prediction[0]}\t{p.Prediction[1]:f2}\t{p.Prediction[2]:F2}\t{p.Prediction[3]:F2}";

                if (p.Prediction[0] == 1)
                {
                    results += " <-- alert is on, predicted changepoint";
                }
                Console.WriteLine(results);
            }
            Console.WriteLine("");
            // </SnippetDisplayResults2>
        }

        static IDataView CreateEmptyDataView(MLContext mlContext)
        {
            // Create empty DataView. We just need the schema to call Fit() for the time series transforms
            IEnumerable<string> enumerableData = new List<string>();
            return mlContext.Data.LoadFromEnumerable(enumerableData);
        }

        public class PersonPrediction
        {
            [VectorType(3)]
            public double[] Prediction { get; set; }
        }

    }
}