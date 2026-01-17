using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football.Application.DTOs
{
    // Odds məlumatlarından hesablanmış ehtimal siqnallarını saxlayır.
    // UI-da odds GÖSTƏRİLMİR.
    // Bu DTO yalnız analiz və proqnoz üçün istifadə olunur.
    public class OddsSignalDto
    {
        // Oyun ID-si (provider-lər arasında mapping üçün)
        public int MatchId { get; set; }

        // Ev komandasının qalib gəlmə ehtimalı (0–100)
        public double HomeWinProbability { get; set; }

        // Heç-heçə ehtimalı (0–100)
        public double DrawProbability { get; set; }

        // Səfər komandasının qalib gəlmə ehtimalı (0–100)
        public double AwayWinProbability { get; set; }

        // Over 2.5 qol ehtimalı (0–100)
        public double Over25Probability { get; set; }

        // Under 2.5 qol ehtimalı (0–100)
        public double Under25Probability { get; set; }

        // Bu siqnalın hansı mənbədən gəldiyi (məs: SportMonks)
        public string ProviderName { get; set; } = string.Empty;

        // Məlumatın götürüldüyü zaman (UTC)
        public DateTime RetrievedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
