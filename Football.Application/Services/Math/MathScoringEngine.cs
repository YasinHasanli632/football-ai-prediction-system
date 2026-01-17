using Football.Application.DTOs;
using Football.Application.Interfaces.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SysMath = System.Math;
namespace Football.Application.Services.Math
{
    /// <summary>
    /// Provider datasını riyazi score-lara çevirir.
    /// QƏRAR VERMİR.
    /// </summary>
    public class MathScoringEngine : IMathScoringEngine
    {
        public MathScoreDto Calculate(ProviderAggregateDto data)
        {
            var result = new MathScoreDto();

            // =========================
            // 1️⃣ ODDS BASED SCORE
            // =========================
            if (data.OddsSignal != null)
            {
                result.HomeWinScore = data.OddsSignal.HomeWinProbability;
                result.DrawScore = data.OddsSignal.DrawProbability;
                result.AwayWinScore = data.OddsSignal.AwayWinProbability;

                result.Over25Score = data.OddsSignal.Over25Probability;
                result.Under25Score = data.OddsSignal.Under25Probability;

                result.Notes.Add("Odds-based probabilities applied.");
            }
            else
            {
                // Odds yoxdursa neytral paylama
                result.HomeWinScore = 33;
                result.DrawScore = 34;
                result.AwayWinScore = 33;

                result.Notes.Add("Odds not available. Neutral distribution used.");
            }

            // =========================
            // 2️⃣ HISTORICAL ADJUSTMENT
            // =========================
            if (data.HistoricalStats != null)
            {
                // Sadə nümunə (genişləndirilə bilər)
                if (data.HistoricalStats.HomeWinRate > data.HistoricalStats.AwayWinRate)
                {
                    result.HomeWinScore += 5;
                    result.AwayWinScore -= 5;
                }

                if (data.HistoricalStats.AverageGoals > 2.5)
                {
                    result.Over25Score += 10;
                    result.Under25Score -= 10;
                }

                result.BttsYesScore = data.HistoricalStats.BttsRate;
                result.BttsNoScore = 100 - result.BttsYesScore;

                result.Notes.Add("Historical statistics adjustment applied.");
            }
            else
            {
                result.BttsYesScore = 50;
                result.BttsNoScore = 50;

                result.Notes.Add("No historical stats. Default BTTS scores used.");
            }

            // =========================
            // 3️⃣ NORMALIZE (0–100)
            // =========================
            Normalize1X2(result);

            // =========================
            // 4️⃣ DATA CONFIDENCE
            // =========================
            result.DataConfidence = CalculateConfidence(data);

            return result;
        }

        // -------------------------
        // HELPERS
        // -------------------------

        private static void Normalize1X2(MathScoreDto score)
        {
            var total =
                score.HomeWinScore +
                score.DrawScore +
                score.AwayWinScore;

            if (total <= 0)
                return;

            score.HomeWinScore = SysMath.Round(score.HomeWinScore / total * 100, 2);
            score.DrawScore = SysMath.Round(score.DrawScore / total * 100, 2);
            score.AwayWinScore = SysMath.Round(score.AwayWinScore / total * 100, 2);
        }

        private static double CalculateConfidence(ProviderAggregateDto data)
        {
            double confidence = 0;

            if (data.Match != null)
                confidence += 40;

            if (data.OddsSignal != null)
                confidence += 40;

            if (data.HistoricalStats != null)
                confidence += 20;

            return confidence;
        }
    }
}
