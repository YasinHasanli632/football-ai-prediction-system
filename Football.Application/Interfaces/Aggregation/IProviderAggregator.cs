using Football.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football.Application.Interfaces.Aggregation
{
    public interface IProviderAggregator
    {
        Task<ProviderAggregateDto> AggregateAsync(int matchId);
    }

}
