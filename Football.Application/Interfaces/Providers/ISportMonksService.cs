using Football.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football.Application.Interfaces.Providers
{
    // SportMonks API-dən odds əsaslı ehtimal siqnalları alır
    public interface ISportMonksService
    {
        Task<OddsSignalDto?> GetOddsSignalAsync(int matchId);
    }
}
