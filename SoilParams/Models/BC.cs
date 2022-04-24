using SoilParams.SoilEnums;
using SoilParams.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoilParams.Models
{
    class BC : BaseModel
    {
        public BC(SoilModelEnum model)
        {
            Name = model.GetDescription();
        }
        public override Dictionary<string, double> CalculateParams(List<double> pressureHeads, List<double> measuredWaterContents, List<double> initialGuess)
        {
            throw new NotImplementedException();
        }

        public override List<double> CalculatePredictedWaterContents(List<double> pressureHeads, List<double> parameters)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, double> GetStats()
        {
            throw new NotImplementedException();
        }
    }
}
