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
        private OneVariableFunctionFitter<TrustRegionMinimizer> fitter { get; set; }

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
            fitter = new OneVariableFunctionFitter<TrustRegionMinimizer>(WrcFunction);
        }

        public override Dictionary<string, double> CalculateParams(List<double> pressureHeads, List<double> measuredWaterContents, List<double> initialGuess)
        {
            var xValues = new DoubleVector(pressureHeads.ToArray());
            var yValues = new DoubleVector(measuredWaterContents.ToArray());
            var start   = new DoubleVector(initialGuess.ToArray());

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

        public override Statistics GetStats(WRCParams model)
        {
            Statistics stats = new Statistics();

            var stdDevM = NMathFunctions.StandardDeviation(model.MeasuredWaterContents.ToArray());
            var stdErrM = stdDevM / Math.Sqrt(model.MeasuredWaterContents.Count);
            var stdDevP = NMathFunctions.StandardDeviation(model.PredictedWaterContents.ToArray());
            var stdErrP = stdDevP / Math.Sqrt(model.PredictedWaterContents.Count);
            var corr = NMathFunctions.Correlation(new DoubleVector(model.MeasuredWaterContents.ToArray()), new DoubleVector(model.PredictedWaterContents.ToArray()));
            var rsqd = new GoodnessOfFit(fitter, new DoubleVector(model.PressureHeads.ToArray()), new DoubleVector(model.MeasuredWaterContents.ToArray()), new DoubleVector(model.Params.Values.ToArray())).RSquared;

            stats.MeasuredStandardDeviation = stdDevM;
            stats.MeasuredStandardError = stdErrM;
            stats.PredictedStandardDeviation = stdDevP;
            stats.PredictedStandardError = stdErrP;
            stats.PearsonCorrelation = corr;
            stats.Rsquared = rsqd;

            return stats;

        }
    }
}
