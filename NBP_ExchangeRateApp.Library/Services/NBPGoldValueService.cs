using NBP_ExchangeRateApp.Library.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NBP_ExchangeRateApp.Library.Services
{
    public class NBPGoldValueService : INBPGoldValueService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string ISO8601DateFormat = "yyyy-MM-dd";
        private readonly List<GoldReport> _goldReports;
        public NBPGoldValueService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _goldReports = new List<GoldReport>();
        }
        public async Task<IEnumerable<GoldReport>> GetCurrent()
        {
            var uri = $"cenyzlota/";

            await LoadGoldPrices(uri);

            return _goldReports;
        }

        public async Task<IEnumerable<GoldReport>> GetBetweenDates(DateTime startDate, DateTime endDate)
        {
            var uri = $"cenyzlota/{ startDate.ToString(ISO8601DateFormat) }/{ endDate.ToString(ISO8601DateFormat) }";

            await LoadGoldPrices(uri);

            return _goldReports;
        }

        public async Task<IEnumerable<GoldReport>> GetOnDate(DateTime date)
        {
            var uri = $"cenyzlota/{ date.ToString(ISO8601DateFormat) }/";

            await LoadGoldPrices(uri);

            return _goldReports;
        }

        private async Task LoadGoldPrices(string uri)
        {
            using (var httpClient = _httpClientFactory.CreateClient("NBP"))
            {
                _goldReports.Clear();

                var responseMessage = await httpClient.GetAsync(uri);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var stringContent = await responseMessage.Content.ReadAsStringAsync();
                    var goldReports = JsonConvert.DeserializeObject<List<GoldReport>>(stringContent);
                    _goldReports.AddRange(goldReports);
                }
                else
                {
                    throw new Exception(responseMessage.StatusCode.ToString());
                }

            }
        }
    }
}
