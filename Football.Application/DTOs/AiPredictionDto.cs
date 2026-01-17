using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football.Application.DTOs
{
    public class AiPredictionDto
    {
        // AI tərəfindən generasiya olunan izah mətni
        // UI-da "Niyə bu seçim?" bölməsində göstəriləcək
        public string Explanation { get; set; } = string.Empty;

        // AI izahının nə qədər güvənli olduğunu hiss etdirməsi üçün (0–100)
        // QƏRAR deyil, sadəcə izah keyfiyyəti
        public double ExplanationConfidence { get; set; }

        // AI izahının hansı məntiqə əsaslandığını göstərmək üçün
        // (odds, stats, form və s.)
        public List<string> UsedFactors { get; set; } = new();

        // Debug və analiz üçün
        // AI-ya göndərilən prompt-un qısa xülasəsi
        public string PromptContext { get; set; } = string.Empty;
    }

}
