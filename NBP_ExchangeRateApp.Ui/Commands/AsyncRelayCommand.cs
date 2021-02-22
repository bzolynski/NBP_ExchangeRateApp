using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NBP_ExchangeRateApp.Ui.Commands
{
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<object, Task> _function;
        private readonly Predicate<object> _canExecute;
        private readonly Action<Exception> _onException;
        private bool _isExecuting;

        public AsyncRelayCommand(Func<object, Task> function, Predicate<object> canExecute, Action<Exception> onException)
        {
            _function = function;
            _canExecute = canExecute;
            _onException = onException;
        }

        public AsyncRelayCommand(Func<object, Task> function, Action<Exception> onException) : this(function, null, onException) {}

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool IsExecuting
        {
            get { return _isExecuting; }
            set { _isExecuting = value; }
        }


        public bool CanExecute(object parameter)
        {
            if (_canExecute == null ? true : _canExecute.Invoke(parameter) && !IsExecuting)
            {
                return true;
            }

            return false;
        }

        public async void Execute(object parameter)
        {
            IsExecuting = true;

            try
            {
                await _function.Invoke(parameter);
            }
            catch (Exception ex)
            {
                _onException?.Invoke(ex);
            }

            IsExecuting = false;
        }

    }
}
