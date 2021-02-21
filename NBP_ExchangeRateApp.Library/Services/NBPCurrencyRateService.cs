using NBP_ExchangeRateApp.Library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;

namespace NBP_ExchangeRateApp.Library.Services
{
    public class NBPCurrencyRateService : INBPCurrencyRateService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string ISO8601DateFormat = "yyyy-MM-dd";

        public NBPCurrencyRateService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<Rate>> GetCurrentCurrencyRates(TableType table)
        {
            var currencyRates = new List<Rate>();
            var uri = $"exchangerates/tables/{ table }";

            await LoadCurrencyRates(currencyRates, uri);

            return currencyRates;
        }

        public async Task<IEnumerable<Rate>> GetCurrencyRatesBetweenDates(TableType table, DateTime startDate, DateTime endDate)
        {
            var currencyRates = new List<Rate>();

            var uri = $"exchangerates/tables/{ table }/{ startDate.ToString(ISO8601DateFormat) }/{ endDate.ToString(ISO8601DateFormat) }";

            await LoadCurrencyRates(currencyRates, uri);

            return currencyRates;
        }

        public async Task<IEnumerable<Rate>> GetCurrencyRatesOnDate(TableType table, DateTime date)
        {
            var currencyRates = new List<Rate>();

            var uri = $"exchangerates/tables/{ table }/{ date.ToString(ISO8601DateFormat) }/";

            await LoadCurrencyRates(currencyRates, uri);

            return currencyRates;
        }

        private async Task LoadCurrencyRates(List<Rate> currencyRates, string uri)
        {
            using (var httpClient = _httpClientFactory.CreateClient("NBP"))
            {
                var responseMessage = await httpClient.GetAsync(uri);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var stringContent = await responseMessage.Content.ReadAsStringAsync();
                    var rateReports = JsonConvert.DeserializeObject<List<CurrencyRateReport>>(stringContent);
                    rateReports.ForEach(r => currencyRates.AddRange(r.Rates));
                }
                else
                {
                    throw new Exception(responseMessage.StatusCode.ToString());
                }

            }
        }
    }
}
