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
        public CurrencyTablesViewModel CreateViewModel()
        {
            return new CurrencyTablesViewModel();
        }
    }
}
