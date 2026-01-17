using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football.Application.DTOs
{
    public class AiPromptContextDto
    {
        // Oyun haqqında qısa info
        public string MatchInfo { get; set; } = string.Empty;

        // Riyazi score-ların xülasəsi
        public string MathSummary { get; set; } = string.Empty;

        // Hansı datalar mövcuddur
        public bool HasOdds { get; set; }
        public bool HasHistoricalStats { get; set; }

        // AI-dan gözlənilən cavabın tipi
        // məsələn: "low scoring match", "balanced game" və s.
        public string ExpectedTone { get; set; } = "neutral";
    }
}
