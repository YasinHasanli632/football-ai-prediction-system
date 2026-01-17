using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football.Application.DTOs
{
    // Tarixi oyun statistikası (Football-Data.org)
    public class HistoricalStatsDto
    {
        // Teams
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }

        // Match results
        public int HomeWins { get; set; }
        public int AwayWins { get; set; }
        public int Draws { get; set; }
        public int TotalMatches { get; set; }

        // Goals
        public int TotalGoals { get; set; }
        public int MatchesWithBothTeamsScored { get; set; }

        // =========================
        // DERIVED METRICS
        // =========================

        public double HomeWinRate =>
            TotalMatches == 0 ? 0 : (double)HomeWins / TotalMatches * 100;

        public double AwayWinRate =>
            TotalMatches == 0 ? 0 : (double)AwayWins / TotalMatches * 100;

        public double DrawRate =>
            TotalMatches == 0 ? 0 : (double)Draws / TotalMatches * 100;

        public double AverageGoals =>
            TotalMatches == 0 ? 0 : (double)TotalGoals / TotalMatches;

        public double BttsRate =>
            TotalMatches == 0 ? 0 : (double)MatchesWithBothTeamsScored / TotalMatches * 100;
    }
}
