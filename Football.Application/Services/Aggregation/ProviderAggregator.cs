using Football.Application.DTOs;
using Football.Application.Interfaces.Aggregation;
using Football.Application.Interfaces.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football.Application.Services.Aggregation
{
    /// <summary>
    /// Müxtəlif provider-lərdən (API Football, SportMonks, Football-Data)
    /// gələn məlumatları vahid obyektə yığır.
    /// 
    /// ⚠️ Bu class:
    /// - HESABLAMA ETMİR
    /// - QƏRAR VERMİR
    /// - SADƏCƏ DATA TOPLAYIR
    /// </summary>
    public class ProviderAggregator : IProviderAggregator
    {
        private readonly IApiFootballService _apiFootball;
        private readonly ISportMonksService _sportMonks;
        private readonly IFootballDataService _footballData;

        public ProviderAggregator(
            IApiFootballService apiFootball,
            ISportMonksService sportMonks,
            IFootballDataService footballData)
        {
            _apiFootball = apiFootball;
            _sportMonks = sportMonks;
            _footballData = footballData;
        }

        public async Task<ProviderAggregateDto> AggregateAsync(int matchId)
        {
            var result = new ProviderAggregateDto();

            // =========================
            // 1️⃣ ƏSAS MATCH DATA
            // =========================
            // Bu məlumat OLMALIDIR, əks halda prediction yoxdur
            var match = await _apiFootball.GetMatchAsync(matchId);

            if (match == null)
            {
                throw new InvalidOperationException(
                    $"Match with id {matchId} not found.");
            }

            result.Match = match;

            // =========================
            // 2️⃣ PARALEL PROVIDER ÇAĞIRIŞLARI
            // =========================
            // Odds və Historical data bir-birindən asılı deyil
            // ona görə paralel çağırırıq (performance üçün)
            var oddsTask = _sportMonks.GetOddsSignalAsync(matchId);
            var historyTask = _footballData.GetHistoricalStatsAsync(
                match.HomeTeamId,
                match.AwayTeamId);

            await Task.WhenAll(oddsTask, historyTask);

            // =========================
            // 3️⃣ ODDS SİQNALI
            // =========================
            result.OddsSignal = oddsTask.Result;

            if (result.OddsSignal == null)
            {
                result.Warnings.Add("Odds data not available for this match.");
            }

            // =========================
            // 4️⃣ TARİXİ STATİSTİKA
            // =========================
            result.HistoricalStats = historyTask.Result;

            if (result.HistoricalStats == null)
            {
                result.Warnings.Add("Historical statistics not found.");
            }

            // =========================
            // 5️⃣ GƏLƏCƏK GENİŞLƏNMƏ NÖQTƏLƏRİ
            // =========================
            // Buraya sabah rahatlıqla əlavə edə bilərsən:
            //
            // - Injury / Suspended players
            // - Team form (last 5 matches)
            // - Home/Away performance
            // - Head-to-Head advanced metrics
            //
            // Hər biri ayrıca provider olacaq və
            // burada sadəcə yığılacaq

            return result;
        }
    }
    }
