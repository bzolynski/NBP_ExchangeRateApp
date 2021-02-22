using NBP_ExchangeRateApp.Library.Models;
using NBP_ExchangeRateApp.Library.Services;
using NBP_ExchangeRateApp.Ui.Commands;
using NBP_ExchangeRateApp.Ui.Factories.ViewModelFactories;
using NBP_ExchangeRateApp.Ui.States.Navigator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NBP_ExchangeRateApp.Ui.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        // Private fields
        #region Private fields
        private readonly INavigator _navigator;

        #endregion

        // Properties
        #region Properties
        public ViewModelBase CurrentViewModel => _navigator.CurrentViewModel;

        #endregion

        // Commands
        #region Commands
        public ICommand ChangeCurrentViewModelCommand{ get; set; }

        #endregion

        // Constructors
        #region Constructors
        public ShellViewModel(INavigator navigator, IRootViewModelFactory rootViewModelFactory)
        {
            _navigator = navigator;
            _navigator.StateChanged += Navigator_StateChanged;

            ChangeCurrentViewModelCommand = new ChangeCurrentViewModelCommand(rootViewModelFactory, _navigator);
            ChangeCurrentViewModelCommand.Execute(ViewType.Tables);
        }

        #endregion

        // Methods
        #region Methods
        private void Navigator_StateChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        #endregion






    }
}
