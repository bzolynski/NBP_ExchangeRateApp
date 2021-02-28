using NBP_ExchangeRateApp.Library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using NBP_ExchangeRateApp.Library.Enums;
using System.Net;

namespace NBP_ExchangeRateApp.Library.Services
{
    public class NBPCurrencyRateService : INBPCurrencyRateService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string ISO8601DateFormat = "yyyy-MM-dd";
        private readonly List<Rate> _currencyRates;
        public NBPCurrencyRateService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _currencyRates = new List<Rate>();
        }

        public async Task<IEnumerable<Rate>> GetCurrent(TableType table)
        {
            var uri = $"exchangerates/tables/{ table }";

            await LoadCurrencyRates(uri);

            return _currencyRates;
        }

        public async Task<IEnumerable<Rate>> GetBetweenDates(TableType table, DateTime startDate, DateTime endDate)
        {
            var uri = $"exchangerates/tables/{ table }/{ startDate.ToString(ISO8601DateFormat) }/{ endDate.ToString(ISO8601DateFormat) }";

            await LoadCurrencyRates(uri);

            return _currencyRates;
        }

        public async Task<IEnumerable<Rate>> GetOnDate(TableType table, DateTime date)
        {
            var uri = $"exchangerates/tables/{ table }/{ date.ToString(ISO8601DateFormat) }/";

            await LoadCurrencyRates(uri);

            return _currencyRates;
        }

        private async Task LoadCurrencyRates(string uri)
        {
            using (var httpClient = _httpClientFactory.CreateClient("NBP"))
            {
                _currencyRates.Clear();

                var responseMessage = await httpClient.GetAsync(uri);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var stringContent = await responseMessage.Content.ReadAsStringAsync();
                    var rateReports = JsonConvert.DeserializeObject<List<CurrencyRateReport>>(stringContent);
                    rateReports.ForEach(r => _currencyRates.AddRange(r.Rates));
                }
                else
                {
                    throw new Exception("Can't load data:\n" + responseMessage.ReasonPhrase);
                }
            }
        }
    }
}
