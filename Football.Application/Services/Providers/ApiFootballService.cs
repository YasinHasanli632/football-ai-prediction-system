using Football.Application.DTOs;
using Football.Application.Interfaces.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Football.Application.Services.Providers
{
    // API-Football ilə real HTTP əlaqəni həyata keçirən servis
    public class ApiFootballService : IApiFootballService
    {
        private readonly HttpClient _http;

        public ApiFootballService(HttpClient http)
        {
            _http = http;
        }
        public async Task<MatchDto?> GetMatchAsync(int matchId)
        {
            // Bu sadəcə nümunədir (mock / real API-yə görə dəyişir)
            var matches = await GetTodayMatchesAsync();

            return matches.FirstOrDefault(m => m.MatchId == matchId);
        }

        // Bugünkü oyunları qaytarır
        public async Task<IReadOnlyList<MatchDto>> GetTodayMatchesAsync()
        {
            var today = DateTime.UtcNow.ToString("yyyy-MM-dd");

            var response = await _http.GetAsync($"/fixtures?date={today}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return ParseMatches(json);
        }

        // Canlı oyunları qaytarır
        public async Task<IReadOnlyList<MatchDto>> GetLiveMatchesAsync()
        {
            var response = await _http.GetAsync("/fixtures?live=all");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return ParseMatches(json);
        }

        // Verilmiş liqa üzrə oyunları qaytarır
        public async Task<IReadOnlyList<MatchDto>> GetMatchesByLeagueAsync(int leagueId)
        {
            var response = await _http.GetAsync($"/fixtures?league={leagueId}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return ParseMatches(json);
        }

        // =========================
        // JSON PARSING (sadə və təhlükəsiz)
        // =========================
        private static IReadOnlyList<MatchDto> ParseMatches(string json)
        {
            var result = new List<MatchDto>();

            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("response", out var responseArray))
                return result;

            foreach (var item in responseArray.EnumerateArray())
            {
                try
                {
                    var fixture = item.GetProperty("fixture");
                    var teams = item.GetProperty("teams");

                    var match = new MatchDto
                    {
                        MatchId = fixture.GetProperty("id").GetInt32(),
                        MatchDate = fixture.GetProperty("date").GetDateTime(),

                        HomeTeam = teams.GetProperty("home").GetProperty("name").GetString() ?? "",
                        AwayTeam = teams.GetProperty("away").GetProperty("name").GetString() ?? ""
                    };

                    result.Add(match);
                }
                catch
                {
                    // problemli record-ları ötürürük (fail-safe)
                    continue;
                }
            }

            return result;
        }
    }
}
