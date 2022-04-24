using SoilParams.SoilEnums;
using SoilParams.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CenterSpace.NMath.Core;

namespace SoilParams.Models
{
    class VG : BaseModel
    {
        public VG(SoilModelEnum model)
        {
            Name = model.GetDescription();

            WrcFunction = delegate( DoubleVector parameters, double h )
            {
                if ( parameters.Length != 4 )
                {
                    throw new InvalidArgumentException( "Incorrect number of function parameters: " + parameters.Length );
                }
                var thetaR = parameters[0];
                var thetaS = parameters[1];
                var alpha  = parameters[2];
                var n      = parameters[3];
                return thetaR + (thetaS - thetaR) / Math.Pow(1 + Math.Pow(alpha * h, n), 1-1/n);
            };
        }

        public override Dictionary<string, double> CalculateParams(List<double> pressureHeads, List<double> measuredWaterContents, List<double> initialGuess)
        {
            var xValues = new DoubleVector(pressureHeads.ToArray());
            var yValues = new DoubleVector(measuredWaterContents.ToArray());
            var start   = new DoubleVector(initialGuess.ToArray());

            var fitter = new OneVariableFunctionFitter<TrustRegionMinimizer>(WrcFunction);

            DoubleVector parameters = fitter.Fit( xValues, yValues, start );

            var soilParameters = new Dictionary<string, double>
            {
                { "ThetaR", parameters[0] },
                { "ThetaS", parameters[1] },
                { "alpha",  parameters[2] },
                { "n",      parameters[3] }
            };

            return soilParameters;
        }

        public override List<double> CalculatePredictedWaterContents(List<double> pressureHeads, List<double> parameters)
        {
            var predictedWaterContents = new List<double>();
            foreach (var pressureHead in pressureHeads)
            {
                predictedWaterContents.Add(WrcFunction(new DoubleVector(parameters.ToArray()), (double)pressureHead));
            }
            return predictedWaterContents;
        }

        public override Dictionary<string, double> GetStats()
        {
            Dictionary<string, double> stats = new();

            // MeasuredStandardDeviation = NMathFunctions.StandardDeviation(yValues);
            // MeasuredStandardError     = MeasuredStandardDeviation / Math.Sqrt(sample.MeasuredWaterContents.Count);
            // SoilParameters.Add("Standard deviation (measured values)", MeasuredStandardDeviation);
            // SoilParameters.Add("Standard error (measured values)",     MeasuredStandardError);

            // var predictedWaterContents = new DoubleVector(sample.PredictedWaterContents.Select(x => (double)x).ToArray());
            // PredictedStandardDeviation = NMathFunctions.StandardDeviation(predictedWaterContents);
            // PredictedStandardError     = PredictedStandardDeviation / Math.Sqrt(sample.PredictedWaterContents.Count);
            // SoilParameters.Add("Standard deviation (predicted values)", PredictedStandardDeviation);
            // SoilParameters.Add("Standard error (predicted values)",     PredictedStandardError);

            // Correlation = NMathFunctions.Correlation(yValues, predictedWaterContents);
            // Rsquared = new GoodnessOfFit(fitter, xValues, yValues, parameters).RSquared;

            return stats;

        }
    }
}
