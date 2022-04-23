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
    public abstract class BaseModel
    {
        public SoilModelEnum ModelEnum    { get; set; }
        public string        Name         { get; set; }

        public Func<DoubleVector, double, double> WrcFunction;  // delegate

        public abstract Dictionary<string, double> GetParams(List<double> pressureHeads, List<double> measuredWaterContent, List<double> initialGuess);
        public abstract Dictionary<string, double> GetStats();
    }
}
