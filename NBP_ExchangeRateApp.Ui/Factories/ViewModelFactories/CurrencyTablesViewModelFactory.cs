using NBP_ExchangeRateApp.Library.Services;
using NBP_ExchangeRateApp.Ui.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBP_ExchangeRateApp.Ui.Factories.ViewModelFactories
{
    public class CurrencyTablesViewModelFactory : IViewModelFactory<CurrencyTablesViewModel>
    {
        private readonly INBPCurrencyRateService _currencyRateService;

        public CurrencyTablesViewModelFactory(INBPCurrencyRateService currencyRateService)
        {
            _currencyRateService = currencyRateService;
        }
        public CurrencyTablesViewModel CreateViewModel()
        {
            return new CurrencyTablesViewModel(_currencyRateService);
        }
    }
}
