using Football.Application.DTOs;
using Football.Application.Interfaces.Providers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Football.Application.Services.Providers
{
    // SportMonks API-dən odds məlumatlarını alır
    // Odds UI-da göstərilmir, yalnız ehtimal siqnallarına çevrilir
    public class SportMonksService : ISportMonksService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public SportMonksService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        // Verilmiş matchId üçün odds əsaslı ehtimal siqnalını qaytarır
        public async Task<OddsSignalDto?> GetOddsSignalAsync(int matchId)
        {
            // SportMonks API key (query param kimi istifadə olunur)
            var apiToken = _config["SportMonks:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiToken))
                return null;

            // Fulltime Result market (1) + Marathonbet (16)
            var url =
                $"/fixtures/{matchId}" +
                $"?api_token={apiToken}" +
                "&include=odds.market;odds.bookmaker" +
                "&filters=markets:1;bookmakers:16";

            var response = await _http.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return ParseOddsSignal(json, matchId);
        }

        // =========================
        // JSON PARSING
        // =========================
        private static OddsSignalDto? ParseOddsSignal(string json, int matchId)
        {
            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("data", out var data))
                return null;

            if (!data.TryGetProperty("odds", out var oddsArray))
                return null;

            double? homeOdds = null;
            double? drawOdds = null;
            double? awayOdds = null;

            foreach (var odd in oddsArray.EnumerateArray())
            {
                try
                {
                    var label = odd.GetProperty("label").GetString();
                    var value = odd.GetProperty("value").GetDouble();

                    // 1X2 mapping
                    if (label == "Home")
                        homeOdds = value;
                    else if (label == "Draw")
                        drawOdds = value;
                    else if (label == "Away")
                        awayOdds = value;
                }
                catch
                {
                    continue;
                }
            }

            // Əgər əsas odds-lardan biri yoxdursa → null
            if (homeOdds == null || drawOdds == null || awayOdds == null)
                return null;

            // Odds → implicit probability (0–100)
            var homeProb = 100 / homeOdds.Value;
            var drawProb = 100 / drawOdds.Value;
            var awayProb = 100 / awayOdds.Value;

            // Normalization (cəmi 100 olsun)
            var total = homeProb + drawProb + awayProb;

            return new OddsSignalDto
            {
                MatchId = matchId,

                HomeWinProbability = System.Math.Round((homeProb / total) * 100, 2),
                DrawProbability = System.Math.Round((drawProb / total) * 100, 2),
                AwayWinProbability = System.Math.Round((awayProb / total) * 100, 2),


                // Over/Under bu mərhələdə hesablanmır
                Over25Probability = 0,
                Under25Probability = 0,

                ProviderName = "SportMonks",
                RetrievedAtUtc = DateTime.UtcNow
            };
        }
    }
}
