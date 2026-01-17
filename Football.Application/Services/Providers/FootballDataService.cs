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
    // Football-Data.org API ilə tarixi nəticələri alan servis
    public class FootballDataService : IFootballDataService
    {
        private readonly HttpClient _http;

        public FootballDataService(HttpClient http)
        {
            _http = http;
        }

        // Ev və səfər komandaları üzrə tarixi nəticələri qaytarır
        public async Task<HistoricalStatsDto?> GetHistoricalStatsAsync(
            int homeTeamId,
            int awayTeamId)
        {
            // Son 10 oyunu götürürük (home team üçün)
            var response = await _http.GetAsync(
                $"/teams/{homeTeamId}/matches?limit=10"
            );

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return ParseHistoricalStats(json, homeTeamId, awayTeamId);
        }

        // =========================
        // JSON PARSING
        // =========================
        private static HistoricalStatsDto? ParseHistoricalStats(
            string json,
            int homeTeamId,
            int awayTeamId)
        {
            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("matches", out var matches))
                return null;

            int homeWins = 0;
            int awayWins = 0;
            int draws = 0;
            int total = 0;

            foreach (var match in matches.EnumerateArray())
            {
                try
                {
                    var score = match.GetProperty("score")
                                     .GetProperty("fullTime");

                    var homeGoals = score.GetProperty("home").GetInt32();
                    var awayGoals = score.GetProperty("away").GetInt32();

                    if (homeGoals > awayGoals)
                        homeWins++;
                    else if (awayGoals > homeGoals)
                        awayWins++;
                    else
                        draws++;

                    total++;
                }
                catch
                {
                    // problemli record-ları ötürürük
                    continue;
                }
            }

            return new HistoricalStatsDto
            {
                HomeTeamId = homeTeamId,
                AwayTeamId = awayTeamId,

                HomeWins = homeWins,
                AwayWins = awayWins,
                Draws = draws,
                TotalMatches = total
            };
        }
    }
    }
