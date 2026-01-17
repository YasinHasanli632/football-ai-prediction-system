using Football.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football.Application.Interfaces.AI
{
    public interface IAiPredictionEngine
    {
        Task<AiPredictionDto> GenerateAsync(
            ProviderAggregateDto providerData,
            MathScoreDto mathScore);
    }

}
