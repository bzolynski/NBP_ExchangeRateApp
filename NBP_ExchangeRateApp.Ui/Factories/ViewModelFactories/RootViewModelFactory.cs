using NBP_ExchangeRateApp.Ui.States.Navigator;
using NBP_ExchangeRateApp.Ui.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBP_ExchangeRateApp.Ui.Factories.ViewModelFactories
{
    public class RootViewModelFactory : IRootViewModelFactory
    {
        private readonly IViewModelFactory<CurrencyTablesViewModel> _currencyTablesViewModelFactory;

        public RootViewModelFactory(IViewModelFactory<CurrencyTablesViewModel> currencyTablesViewModelFactory)
        {
            _currencyTablesViewModelFactory = currencyTablesViewModelFactory;
        }

        public ViewModelBase CreateViewModel(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Tables:
                    return _currencyTablesViewModelFactory.CreateViewModel();
                case ViewType.Currency:
                    throw new NotImplementedException();
                case ViewType.Gold:
                    throw new NotImplementedException();
                default:
                    throw new Exception();
            }
        }
    }
}
