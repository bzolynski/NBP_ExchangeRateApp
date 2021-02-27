using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBP_ExchangeRateApp.Library.Models
{
    public class GoldReport
    {
        [JsonProperty("data")]
        public DateTime ReportDate { get; set; }
        [JsonProperty("cena")]
        public float Price { get; set; }
    }

}
