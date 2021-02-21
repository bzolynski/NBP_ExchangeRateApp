using NBP_ExchangeRateApp.Ui.ViewModels;
using System;

namespace NBP_ExchangeRateApp.Ui.States.Navigator
{
    public interface INavigator
    {
        ViewModelBase CurrentViewModel { get; set; }

        event Action StateChanged;
    }
}