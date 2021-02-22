using System;

namespace NBP_ExchangeRateApp.Library.Models
{
    public class Rate
    {
        public DateTime ReportDate { get; set; }
        public string Currency { get; set; }
        public string Code { get; set; }
        public float Mid { get; set; }
    }

    

}

