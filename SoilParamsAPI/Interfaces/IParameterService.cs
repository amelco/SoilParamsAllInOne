﻿using SoilParamsAPI.Models;
using System.Collections.Generic;

namespace SoilParamsAPI.Interfaces
{
    public interface IParameterService
    {
        Dictionary<string, double> GetParameters(int id);
        string                     CalculateParameters(string input);
    }
}