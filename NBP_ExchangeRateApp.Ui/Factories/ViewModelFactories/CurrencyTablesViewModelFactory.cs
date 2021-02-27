using NBP_ExchangeRateApp.Library.Services;
using NBP_ExchangeRateApp.Ui.Validators;
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
        private readonly ApiQueryableValidator _validationRules;

        public CurrencyTablesViewModelFactory(INBPCurrencyRateService currencyRateService, ApiQueryableValidator validationRules)
        {
            _currencyRateService = currencyRateService;
            _validationRules = validationRules;
        }
        public CurrencyTablesViewModel CreateViewModel()
        {
            return new CurrencyTablesViewModel(_currencyRateService, _validationRules);
        }
    }
}
