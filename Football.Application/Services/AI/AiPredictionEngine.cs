using Football.Application.DTOs;
using Football.Application.Interfaces.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football.Application.Services.AI
{
    public class AiPredictionEngine : IAiPredictionEngine
    {
        private readonly IOpenAiService _openAi;

        public AiPredictionEngine(IOpenAiService openAi)
        {
            _openAi = openAi;
        }

        public async Task<AiPredictionDto> GenerateAsync(
            ProviderAggregateDto providerData,
            MathScoreDto mathScore)
        {
            // =========================
            // 1️⃣ PROMPT KONTEKSTİ YIĞ
            // =========================

            var match = providerData.Match;

            var prompt =
                $"Match: {match.HomeTeam} vs {match.AwayTeam}\n" +
                $"Date: {match.MatchDate:yyyy-MM-dd}\n\n" +

                $"Probabilities (Math Model):\n" +
                $"- Home Win: {mathScore.HomeWinScore}%\n" +
                $"- Draw: {mathScore.DrawScore}%\n" +
                $"- Away Win: {mathScore.AwayWinScore}%\n" +
                $"- Over 2.5 Goals: {mathScore.Over25Score}%\n" +
                $"- Under 2.5 Goals: {mathScore.Under25Score}%\n" +
                $"- BTTS Yes: {mathScore.BttsYesScore}%\n\n" +

                $"Data availability:\n" +
                $"- Odds: {(providerData.OddsSignal != null ? "Yes" : "No")}\n" +
                $"- Historical Stats: {(providerData.HistoricalStats != null ? "Yes" : "No")}\n\n" +

                $"Explain the likely match outcome briefly.\n" +
                $"Do NOT give betting advice.\n" +
                $"Do NOT say what to bet.\n" +
                $"Only explain tendencies and risks.";

            // =========================
            // 2️⃣ OPENAI ÇAĞIRIŞI
            // =========================

            var explanation = await _openAi.GetPredictionExplanationAsync(prompt);

            // =========================
            // 3️⃣ FACTOR ANALİZİ
            // =========================

            var factors = new List<string>();

            if (providerData.OddsSignal != null)
                factors.Add("Odds probabilities");

            if (providerData.HistoricalStats != null)
                factors.Add("Historical performance");

            if (mathScore.DataConfidence >= 80)
                factors.Add("High data confidence");
            else if (mathScore.DataConfidence >= 50)
                factors.Add("Medium data confidence");
            else
                factors.Add("Low data confidence");

            // =========================
            // 4️⃣ EXPLANATION CONFIDENCE
            // =========================
            // Bu AI keyfiyyəti deyil
            // Sadəcə izahın arxasında nə qədər data olduğunu göstərir

            var explanationConfidence =
               System.Math.Min(100, mathScore.DataConfidence + 10);

            // =========================
            // 5️⃣ NƏTİCƏ
            // =========================

            return new AiPredictionDto
            {
                Explanation = explanation,
                ExplanationConfidence = explanationConfidence,
                UsedFactors = factors,
                PromptContext = $"1X2 + Goals + BTTS | DataConfidence={mathScore.DataConfidence}"
            };
        }
    }

}
