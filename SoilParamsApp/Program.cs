using SoilParams;
using SoilParams.SoilEnums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoilParamsApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = @"{
                ""Title"": ""Soil from location X"",
                ""Description"": ""Samples collected in 02/03/2022"",
                ""PressureHeads"": [
                    0, 5, 30, 100, 300, 500, 1000, 5000, 10000, 15000
                ],
                ""MeasuredWaterContents"": [
                    0.543, 0.474, 0.402, 0.390, 0.327, 0.309, 0.290, 0.287, 0.286, 0.280
                ],
                ""Models"": [
                    ""VG""
                ],
                ""InitialGuess"": [
                    0.1, 0.5, 0.5, 1.1
                ]
            }";

            List<double> guess = new();
            guess.Add(0.2);
            guess.Add(0.5);
            guess.Add(1.2);
            guess.Add(0.5);

            var sample = new WRCParams(input);
            Console.WriteLine(sample.GetModelName());

            sample.CalculateParams();
            if (sample.Params.Count > 0)
            {
                foreach (var p in sample.Params)
                {
                    Console.WriteLine($"{p.Key,10}: {p.Value.ToString("#0.0000"),7}");
                }
            }

            sample.CalculateWaterContents();
            if (sample.PredictedWaterContents.Count > 0)
            {
                Console.WriteLine("\nMeasured, Predicted");
                foreach (var (p,i) in sample.PredictedWaterContents.Select((p,i) => (p,i)))
                {
                    Console.WriteLine($"{sample.MeasuredWaterContents[i].ToString("0.0000"),8}, {p.ToString("0.0000"),9}");
                }
            }

        }
    }
}
