using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football.Application.DTOs
{
    public class MatchDto
    {
        public int MatchId { get; set; }

        // League
        public int LeagueId { get; set; }
        public string LeagueName { get; set; } = null!;
        public string LeagueLogoUrl { get; set; } = null!;

        // Teams
        public int HomeTeamId { get; set; }
        public string HomeTeam { get; set; } = null!;
        public string HomeLogoUrl { get; set; } = null!;

        public int AwayTeamId { get; set; }
        public string AwayTeam { get; set; } = null!;
        public string AwayLogoUrl { get; set; } = null!;

        public DateTime MatchDate { get; set; }

        // Prediction Preview (Index üçün)
        public bool HasPrediction { get; set; }
        public string? MainPickLabel { get; set; }            // "BTTS YES"
        public double? MainPickProbability { get; set; }      // 68.0

        // UI əlavə rahatlıq (istəsən)
        public string? MainPickText => HasPrediction && MainPickLabel != null && MainPickProbability != null
            ? $"{MainPickLabel} • {MainPickProbability:0}%"
            : null;
    }

}
