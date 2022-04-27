using SoilParamsAPI.Interfaces;
using SoilParamsAPI.Models;
using SoilParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using SoilParamsAPI.Extensions;

namespace SoilParamsAPI.Services
{
    public class ParameterService : IParameterService
    {
        public OutputModel CalculateParameters(InputQueryParameters input)
        {
            var res = this.Calculate(JsonSerializer.Serialize(input));
            return res;
        }

        public Dictionary<string, double> GetParameters(int id)
        {
            return new Dictionary<string, double> { {"parameter_test", 2.34 } };
        }

        private OutputModel Calculate(string input)
        {
            var sample = new WRCParams(input);
            sample.CalculateParams();
            sample.CalculateWaterContents();
            sample.CalculateStatistics();

            return sample.ToOuputModel();
        }
    }
}
