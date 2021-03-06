﻿using NBP_ExchangeRateApp.Library.Services;
using NBP_ExchangeRateApp.Ui.Validators;
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
        private readonly ApiQueryableValidator _validationRules;

        public GoldPricesViewModelFactory(INBPGoldValueService goldValueService, ApiQueryableValidator validationRules)
        {
            _goldValueService = goldValueService;
            _validationRules = validationRules;
        }
        public GoldPricesViewModel CreateViewModel()
        {
            return new GoldPricesViewModel(_goldValueService, _validationRules);
        }
    }
}
