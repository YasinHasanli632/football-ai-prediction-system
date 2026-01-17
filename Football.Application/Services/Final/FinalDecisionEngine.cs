using Football.Application.DTOs;
using Football.Application.Interfaces.Final;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football.Application.Services.Final
{
    public class FinalDecisionEngine : IFinalDecisionEngine
    {
        public FinalPredictionDto Decide(
            ProviderAggregateDto providerData,
            MathScoreDto mathScore,
            AiPredictionDto aiPrediction)
        {
            var result = new FinalPredictionDto();

            // =========================
            // 1️⃣ ƏSAS 1X2 QƏRAR
            // =========================

            var max1X2 = System.Math.Max(
                mathScore.HomeWinScore,
                System.Math.Max(mathScore.DrawScore, mathScore.AwayWinScore));

            if (max1X2 == mathScore.HomeWinScore)
            {
                result.BestPick = "Home Win";
                result.Confidence = mathScore.HomeWinScore;
            }
            else if (max1X2 == mathScore.AwayWinScore)
            {
                result.BestPick = "Away Win";
                result.Confidence = mathScore.AwayWinScore;
            }
            else
            {
                result.BestPick = "Draw";
                result.Confidence = mathScore.DrawScore;
            }

            result.DecisionFactors.Add("1X2 probability comparison");

            // =========================
            // 2️⃣ GOAL MARKET PRIORITY
            // =========================
            // Əgər goal market-lər daha güclüdürsə, 1X2-ni üstələyə bilər

            if (mathScore.Over25Score >= 65)
            {
                result.BestPick = "Over 2.5 Goals";
                result.Confidence = mathScore.Over25Score;
                result.DecisionFactors.Add("High Over 2.5 probability");
            }
            else if (mathScore.Under25Score >= 65)
            {
                result.BestPick = "Under 2.5 Goals";
                result.Confidence = mathScore.Under25Score;
                result.DecisionFactors.Add("High Under 2.5 probability");
            }

            // =========================
            // 3️⃣ BTTS YOXLANIŞI
            // =========================
            if (mathScore.BttsYesScore >= 70)
            {
                result.BestPick = "BTTS Yes";
                result.Confidence = mathScore.BttsYesScore;
                result.DecisionFactors.Add("Strong BTTS Yes signal");
            }
            else if (mathScore.BttsNoScore >= 70)
            {
                result.BestPick = "BTTS No";
                result.Confidence = mathScore.BttsNoScore;
                result.DecisionFactors.Add("Strong BTTS No signal");
            }

            // =========================
            // 4️⃣ CONFIDENCE ADJUSTMENT
            // =========================
            // Data zəifdirsə confidence azaldılır

            result.DataConfidence = mathScore.DataConfidence;

            if (mathScore.DataConfidence < 60)
            {
                result.Confidence *= 0.85;
                result.DecisionFactors.Add("Low data confidence adjustment");
            }

            result.Confidence =System.Math.Round(result.Confidence, 2);

            // =========================
            // 5️⃣ AI İZAHI
            // =========================
            result.Explanation = aiPrediction.Explanation;

            // =========================
            // 6️⃣ ALTERNATİVLƏR
            // =========================
            if (result.BestPick != "Home Win")
                result.Alternatives.Add("Home Win");

            if (result.BestPick != "Draw")
                result.Alternatives.Add("Draw");

            if (result.BestPick != "Away Win")
                result.Alternatives.Add("Away Win");

            return result;
        }
    }
}
