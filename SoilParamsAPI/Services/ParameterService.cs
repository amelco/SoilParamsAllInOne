using SoilParamsAPI.Interfaces;
using SoilParamsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoilParamsAPI.Services
{
    public class ParameterService : IParameterService
    {
        public string CalculateParameters(string input)
        {
            return "Yet to be done";
        }

        public Dictionary<string, double> GetParameters(int id)
        {
            return new Dictionary<string, double> { {"parameter_test", 2.34 } };
        }
    }
}
