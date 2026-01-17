using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football.Application.DTOs
{
    public class OpenAiOptions
    {
        public string ApiKey { get; set; } = null!;
        public string BaseUrl { get; set; } = "https://api.openai.com/v1/chat/completions";
        public string Model { get; set; } = "gpt-4o-mini";
        public int MaxTokens { get; set; } = 200;
        public double Temperature { get; set; } = 0.6;
    }
}
