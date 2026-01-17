using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football.Application.DTOs
{
    public class FinalPredictionDto
    {
        // Ən uyğun seçim
        // Məsələn: "Home Win", "Draw", "Away Win", "BTTS Yes", "Under 2.5"
        public string BestPick { get; set; } = string.Empty;

        // Seçilən variantın confidence dərəcəsi (0–100)
        public double Confidence { get; set; }

        // Qısa insan oxunaqlı izah
        // AI-dan gələn explanation burada istifadə olunur
        public string Explanation { get; set; } = string.Empty;

        // Alternativ seçimlər (UI üçün)
        public List<string> Alternatives { get; set; } = new();

        // Hansı faktorlar bu qərara təsir edib
        public List<string> DecisionFactors { get; set; } = new();

        // Data nə qədər dolu idi (provider coverage)
        public double DataConfidence { get; set; }
    }
}
