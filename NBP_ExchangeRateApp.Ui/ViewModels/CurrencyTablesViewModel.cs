using NBP_ExchangeRateApp.Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBP_ExchangeRateApp.Ui.ViewModels
{
    public class CurrencyTablesViewModel : ViewModelBase
    {
        // Private fields
        #region Private fields
        private TableType _selectedTableType;
        private bool _isDatePeriod;

        private DateTime _selectedDate = DateTime.Today;
        private DateTime _selectedStatDate = DateTime.Today;
        private DateTime _selectedEndDate = DateTime.Today;

        #endregion

        // Properties
        #region Properties
        public bool IsDatePeriod
        {
            get { return _isDatePeriod; }
            set
            {
                _isDatePeriod = value;
                OnPropertyChanged(nameof(IsDatePeriod));
            }
        }

        public List<TableType> AvailableTypes { get; private set; }
        public TableType SelectedTableType { get => _selectedTableType; set => _selectedTableType = value; }
        public DateTime SelectedDate { get => _selectedDate; set => _selectedDate = value; }
        public DateTime SelectedStartDate { get => _selectedStatDate; set => _selectedStatDate = value; }
        public DateTime SelectedEndDate { get => _selectedEndDate; set => _selectedEndDate = value; }


        #endregion

        // Commands
        #region Commands

        #endregion

        // Constructors
        #region Constructors
        public CurrencyTablesViewModel()
        {
            AvailableTypes = new List<TableType> { TableType.a, TableType.b };
        }

        #endregion

        // Methods
        #region Methods

        #endregion

    }
}
