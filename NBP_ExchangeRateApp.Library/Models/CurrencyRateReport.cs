﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NBP_ExchangeRateApp.Library.Models
{
    public class CurrencyRateReport
    {
        [JsonProperty("effectiveDate")]
        private DateTime ReportDate { get; set; }
        public List<Rate> Rates { get; set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext streamingContext)
        {
            foreach (var rate in Rates)
            {
                rate.ReportDate = ReportDate;
            }
        }
    }

    public class Rate
    {
        public DateTime ReportDate { get; set; }
        public string Currency { get; set; }
        public string Code { get; set; }
        public float Mid { get; set; }
    }



}

