using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBP_ExchangeRateApp.Ui.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event Action<string> OnOpenDialog;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private protected void OpenDialog(string message)
        {
            OnOpenDialog?.Invoke(message);
        }
    }
}
