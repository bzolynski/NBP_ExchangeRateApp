using NBP_ExchangeRateApp.Library.Services;
using NBP_ExchangeRateApp.Ui.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBP_ExchangeRateApp.Ui.Factories.ViewModelFactories
{
    public class GoldPricesViewModelFactory : IViewModelFactory<GoldPricesViewModel>
    {
        private readonly INBPGoldValueService _goldValueService;

        public GoldPricesViewModelFactory(INBPGoldValueService goldValueService)
        {
            _goldValueService = goldValueService;
        }
        public GoldPricesViewModel CreateViewModel()
        {
            return new GoldPricesViewModel(_goldValueService);
        }
    }
}
