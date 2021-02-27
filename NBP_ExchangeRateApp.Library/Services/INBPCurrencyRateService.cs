using NBP_ExchangeRateApp.Library.Enums;
using NBP_ExchangeRateApp.Library.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NBP_ExchangeRateApp.Library.Services
{
    public interface INBPCurrencyRateService
    {
        Task<IEnumerable<Rate>> GetBetweenDates(TableType table, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Rate>> GetOnDate(TableType table, DateTime date);
        Task<IEnumerable<Rate>> GetCurrent(TableType table);
    }
}