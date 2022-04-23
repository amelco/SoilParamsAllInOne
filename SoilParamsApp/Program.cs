using SoilParams;
using SoilParams.SoilEnums;
using System;
using System.Collections.Generic;

namespace SoilParamsApp
{
    class Program
    {
        static void Main(string[] args)
        {
            List<double> guess = new();
            guess.Add(0.2);
            guess.Add(0.5);
            guess.Add(1.2);
            guess.Add(0.5);

            var sample = new WRCParams(guess);
            Console.WriteLine(sample.GetModelName());


            sample.CalculateParams();
            
            if (sample.Params.Count > 0)
            {
                foreach (var p in sample.Params)
                {
                    Console.WriteLine($"{p.Key}: {p.Value}");
                }
            }

            Console.ReadLine();
        }
    }
}
