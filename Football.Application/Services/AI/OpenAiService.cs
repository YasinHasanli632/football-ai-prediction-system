using Football.Application.DTOs;
using Football.Application.Interfaces.AI;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Football.Application.Services.AI
{
    public class OpenAiService : IOpenAiService
    {
        private readonly HttpClient _http;
        private readonly OpenAiOptions _options;

        public OpenAiService(
            HttpClient http,
            IOptions<OpenAiOptions> options)
        {
            _http = http;
            _options = options.Value;

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _options.ApiKey);
        }

        public async Task<string> GetPredictionExplanationAsync(string prompt)
        {
            var request = new
            {
                model = _options.Model,
                messages = new[]
                {
                    new { role = "system", content = "You are a football match analyst. Explain predictions clearly and shortly." },
                    new { role = "user", content = prompt }
                },
                temperature = _options.Temperature,
                max_tokens = _options.MaxTokens
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await _http.PostAsync(_options.BaseUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                return "AI explanation is currently unavailable.";
            }

            using var stream = await response.Content.ReadAsStreamAsync();
            using var json = await JsonDocument.ParseAsync(stream);

            return json
                .RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString()
                ?? "No explanation generated.";
        }
    }
}
