using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football.Application.DTOs
{
    /// <summary>
    /// Riyazi hesablamaların nəticəsi.
    /// Bu DTO QƏRAR DEYİL, sadəcə SCORE-dur.
    /// </summary>
    public class MathScoreDto
    {
        // 1X2 score-lar (0–100)
        public double HomeWinScore { get; set; }
        public double DrawScore { get; set; }
        public double AwayWinScore { get; set; }

        // Goal-based market-lər
        public double Over25Score { get; set; }
        public double Under25Score { get; set; }
        public double BttsYesScore { get; set; }
        public double BttsNoScore { get; set; }

        /// <summary>
        /// Riyazi hesablamanın nə qədər etibarlı olduğunu göstərir (0–100)
        /// Provider datasının doluluğuna görə hesablanır
        /// </summary>
        public double DataConfidence { get; set; }

        /// <summary>
        /// Debug / analiz üçün
        /// </summary>
        public List<string> Notes { get; set; } = new();
    }
}
