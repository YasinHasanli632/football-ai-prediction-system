using Football.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football.Application.Interfaces.Providers
{
    // API-Football ilə əlaqəni saxlayan servis
    public interface IApiFootballService
    {
        Task<IReadOnlyList<MatchDto>> GetTodayMatchesAsync();
        Task<IReadOnlyList<MatchDto>> GetLiveMatchesAsync();
        Task<IReadOnlyList<MatchDto>> GetMatchesByLeagueAsync(int leagueId);
        // Business logic üçün (VACİB)
        Task<MatchDto?> GetMatchAsync(int matchId);
    }
}
