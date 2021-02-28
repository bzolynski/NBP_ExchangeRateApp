using NBP_ExchangeRateApp.Ui.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NBP_ExchangeRateApp.Ui.ViewModels
{
    public class DialogViewModel : ViewModelBase
    {
        public event Action OnDialogClose;
        public string Message { get; }
        public ICommand CloseDialogCommand { get; set; }
        public DialogViewModel(string message)
        {
            Message = message;
            CloseDialogCommand = new RelayCommand(CloseDialog);
        }

        private void CloseDialog(object obj)
        {
            OnDialogClose?.Invoke();
        }
    }
}
