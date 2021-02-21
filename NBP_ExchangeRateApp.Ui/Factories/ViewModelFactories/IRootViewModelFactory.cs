using NBP_ExchangeRateApp.Ui.States.Navigator;
using NBP_ExchangeRateApp.Ui.ViewModels;

namespace NBP_ExchangeRateApp.Ui.Factories.ViewModelFactories
{
    public interface IRootViewModelFactory
    {
        public ViewModelBase CreateViewModel(ViewType viewType);
    }
}