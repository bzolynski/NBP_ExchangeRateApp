using NBP_ExchangeRateApp.Library.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NBP_ExchangeRateApp.Library.Services
{
    public interface INBPGoldValueService
    {
        Task<IEnumerable<GoldReport>> GetBetweenDates(DateTime startDate, DateTime endDate);
        Task<IEnumerable<GoldReport>> GetCurrent();
        Task<IEnumerable<GoldReport>> GetOnDate(DateTime date);
    }
}