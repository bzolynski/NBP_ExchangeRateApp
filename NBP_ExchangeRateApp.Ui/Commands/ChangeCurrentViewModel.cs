using NBP_ExchangeRateApp.Ui.Factories.ViewModelFactories;
using NBP_ExchangeRateApp.Ui.States.Navigator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NBP_ExchangeRateApp.Ui.Commands
{
    public class ChangeCurrentViewModel : ICommand
    {
        private readonly IRootViewModelFactory _rootViewModelFactory;
        private readonly INavigator _navigator;


        public ChangeCurrentViewModel(IRootViewModelFactory rootViewModelFactory, INavigator navigator)
        {
            _rootViewModelFactory = rootViewModelFactory;
            _navigator = navigator;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (parameter is ViewType viewType)
            {
                _navigator.CurrentViewModel = _rootViewModelFactory.CreateViewModel(viewType);
            }
        }
    }
}
