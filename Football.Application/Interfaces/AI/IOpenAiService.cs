using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football.Application.Interfaces.AI
{
    // OpenAI yalnız izah mətni yaratmaq üçün istifadə olunur
    public interface IOpenAiService
    {
        Task<string> GetPredictionExplanationAsync(string prompt);
    }
}
