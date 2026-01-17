using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football.Application.DTOs
{
    public class ProviderAggregateDto
    {
        /// <summary>
        /// Əsas oyun məlumatı (komandalar, tarix, liga və s.)
        /// BU OLMASA → prediction mümkün deyil
        /// </summary>
        public MatchDto Match { get; set; } = null!;

        /// <summary>
        /// Odds əsasında ehtimal siqnalları (SportMonks)
        /// Ola bilər null (coverage yoxdursa)
        /// </summary>
        public OddsSignalDto? OddsSignal { get; set; }

        /// <summary>
        /// Tarixi qarşılaşmalar və forma statistikası
        /// Ola bilər null (data tapılmadıqda)
        /// </summary>
        public HistoricalStatsDto? HistoricalStats { get; set; }

        /// <summary>
        /// Aggregation zamanı problemlər olduqda (opsional)
        /// Math / Final engine-lər bunu nəzərə ala bilər
        /// </summary>
        public List<string> Warnings { get; set; } = new();
    }
}
