using Football.Application.DTOs;
using Football.Application.Interfaces.Aggregation;
using Football.Application.Interfaces.AI;
using Football.Application.Interfaces.Facades;
using Football.Application.Interfaces.Final;
using Football.Application.Interfaces.Math;
using Football.Application.Interfaces.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football.Application.Services.Facades
{
    public class FootballPredictionFacade : IFootballPredictionFacade
    {
        private readonly IProviderAggregator _providerAggregator;
        private readonly IMathScoringEngine _mathEngine;
        private readonly IAiPredictionEngine _aiEngine;
        private readonly IFinalDecisionEngine _finalEngine;

        // Match data üçün birbaşa provider
        private readonly IApiFootballService _apiFootball;

        public FootballPredictionFacade(
            IProviderAggregator providerAggregator,
            IMathScoringEngine mathEngine,
            IAiPredictionEngine aiEngine,
            IFinalDecisionEngine finalEngine,
            IApiFootballService apiFootball)
        {
            _providerAggregator = providerAggregator;
            _mathEngine = mathEngine;
            _aiEngine = aiEngine;
            _finalEngine = finalEngine;
            _apiFootball = apiFootball;
        }

        // =========================
        // PREDICTION FLOW
        // =========================
        public async Task<FinalPredictionDto> PredictMatchAsync(int matchId)
        {
            // 1️⃣ Provider-lərdən bütün data yığ
            var providerData = await _providerAggregator.AggregateAsync(matchId);

            // 2️⃣ Riyazi scoring
            var mathScore = _mathEngine.Calculate(providerData);

            // 3️⃣ AI izahı (yalnız explanation)
            var aiPrediction = await _aiEngine.GenerateAsync(providerData, mathScore);

            // 4️⃣ Final qərar
            return _finalEngine.Decide(providerData, mathScore, aiPrediction);
        }

        // =========================
        // MATCH LIST (UI üçün)
        // =========================

        public async Task<IReadOnlyList<MatchDto>> GetTodayMatchesAsync()
            => await _apiFootball.GetTodayMatchesAsync();

        public async Task<IReadOnlyList<MatchDto>> GetLiveMatchesAsync()
            => await _apiFootball.GetLiveMatchesAsync();

        public async Task<IReadOnlyList<MatchDto>> GetMatchesByLeagueAsync(int leagueId)
            => await _apiFootball.GetMatchesByLeagueAsync(leagueId);
    }
}
