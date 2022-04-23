using SoilParams.SoilEnums;
using SoilParams.Extensions;
using SoilParams.Models;
using System;
using System.Collections.Generic;

namespace SoilParams
{
    public class WRCParams
    {
        public BaseModel                  WRCModel               { get; set; }
        public List<double>               PressureHeads          { get; private set; }
        public List<double>               MeasuredWaterContents  { get; private set; } = new();
        public List<double>               PredictedWaterContents { get; private set; } = new();
        public Dictionary<string, double> Params                 { get; private set; } = new();
        public double                     StandardDeviation      { get; private set; }
        public double                     StandardError          { get; private set; }
        public double                     Rsquared               { get; private set; }
        public List<double>               InitialGuess           { get; private set; }


        // TODO: Read input string in JSON format and add as a parameter in the constructor.
        //       Call a private method to read this JSON file to give values to
        //       PressureHeads, MeasuredWaterContents and InitialGuess
        public WRCParams(List<double> initialGuess)
        {
            WRCModel = ModelSimpleFactory.CreateModel(SoilModelEnum.VG);
            InitialGuess = initialGuess;
        }

        public WRCParams(SoilModelEnum model, List<double> initialGuess)
        {
            WRCModel = ModelSimpleFactory.CreateModel(model);
            InitialGuess = initialGuess;
        }
        
        public string GetModelName()
        {
            return WRCModel.Name;
        }

        public void CalculateParams()
        {
            if (WRCModel == null)
            {
                throw new NullReferenceException("A model must be selected first");
            }

            Params = WRCModel.GetParams(PressureHeads, MeasuredWaterContents, InitialGuess);
        }

        public void CalculateStatistics()
        {

        }
    }
}
