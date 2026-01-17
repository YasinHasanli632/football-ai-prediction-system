using Football.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football.Application.Interfaces.Final
{
    public interface IFinalDecisionEngine
    {
        FinalPredictionDto Decide(
            ProviderAggregateDto providerData,
            MathScoreDto mathScore,
            AiPredictionDto aiPrediction);
    }

}
