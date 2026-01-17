using Football.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football.Application.Interfaces.Providers
{
    // Football-Data.org API-dən tarixi nəticələri alır
    public interface IFootballDataService
    {
        Task<HistoricalStatsDto?> GetHistoricalStatsAsync(
            int homeTeamId,
            int awayTeamId
        );
    }
}
