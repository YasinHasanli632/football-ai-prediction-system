using Football.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football.Application.Interfaces.Facades
{
    public interface IFootballPredictionFacade
    {
        Task<FinalPredictionDto> PredictMatchAsync(int matchId);

        Task<IReadOnlyList<MatchDto>> GetTodayMatchesAsync();
        Task<IReadOnlyList<MatchDto>> GetLiveMatchesAsync();
        Task<IReadOnlyList<MatchDto>> GetMatchesByLeagueAsync(int leagueId);
    }
}
