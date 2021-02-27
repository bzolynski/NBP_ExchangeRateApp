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
        private readonly IViewModelFactory<GoldPricesViewModel> _goldPricesViewModelFactory;

        public RootViewModelFactory(
            IViewModelFactory<CurrencyTablesViewModel> currencyTablesViewModelFactory,
            IViewModelFactory<GoldPricesViewModel> goldPricesViewModelFactory)
        {
            _currencyTablesViewModelFactory = currencyTablesViewModelFactory;
            _goldPricesViewModelFactory = goldPricesViewModelFactory;
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
                    return _goldPricesViewModelFactory.CreateViewModel();
                default:
                    throw new Exception();
            }
        }
    }
}
