using NBP_ExchangeRateApp.Library.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NBP_ExchangeRateApp.Library.Services
{
    public interface INBPCurrencyRateService
    {
        Task<IEnumerable<Rate>> GetCurrencyRatesBetweenDates(TableType table, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Rate>> GetCurrencyRatesOnDate(TableType table, DateTime date);
        Task<IEnumerable<Rate>> GetCurrentCurrencyRates(TableType table);
    }
}