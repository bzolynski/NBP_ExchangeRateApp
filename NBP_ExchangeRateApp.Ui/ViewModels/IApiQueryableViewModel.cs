using System;

namespace NBP_ExchangeRateApp.Ui.ViewModels
{
    public interface IApiQueryableViewModel
    {
        public bool IsDatePeriod { get; set; }
        public bool IsShowLatest { get; set; }
        public DateTime SelectedDate { get; set; }
        public DateTime SelectedStartDate { get; set; }
        public DateTime SelectedEndDate { get; set; }
    }
}