using Football.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football.Application.Interfaces.Math
{
    public interface IMathScoringEngine
    {
        MathScoreDto Calculate(ProviderAggregateDto data);
    }
}
