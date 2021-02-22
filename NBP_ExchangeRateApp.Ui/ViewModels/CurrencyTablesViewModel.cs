using NBP_ExchangeRateApp.Library.Enums;
using NBP_ExchangeRateApp.Library.Models;
using NBP_ExchangeRateApp.Library.Services;
using NBP_ExchangeRateApp.Ui.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace NBP_ExchangeRateApp.Ui.ViewModels
{
    public class CurrencyTablesViewModel : ViewModelBase
    {
        // Private fields
        #region Private fields
        private TableType _selectedTableType;
        private bool _isDatePeriod;
        private bool _isShowLatest;

        private List<Rate> _rates;
        private DateTime _selectedDate = DateTime.Today;
        private DateTime _selectedStatDate = DateTime.Today;
        private DateTime _selectedEndDate = DateTime.Today;
        private readonly INBPCurrencyRateService _currencyRateService;

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
        public bool IsShowLatest
        {
            get { return _isShowLatest; }
            set
            {
                _isShowLatest = value;
                if (value)
                {
                    IsDatePeriod = false;
                    OnPropertyChanged(nameof(IsDatePeriod));
                }

                OnPropertyChanged(nameof(IsShowLatest));
            }
        }

        public List<TableType> AvailableTypes { get; private set; }
        public TableType SelectedTableType { get => _selectedTableType; set => _selectedTableType = value; }
        public DateTime SelectedDate { get => _selectedDate; set => _selectedDate = value; }
        public DateTime SelectedStartDate { get => _selectedStatDate; set => _selectedStatDate = value; }
        public DateTime SelectedEndDate { get => _selectedEndDate; set => _selectedEndDate = value; }
        public ICollectionView RateCollectionView { get; set; }
        #endregion

        // Commands
        #region Commands
        public ICommand LoadCurrencyRatesCommand { get; set; }
        #endregion

        // Constructors
        #region Constructors
        public CurrencyTablesViewModel(INBPCurrencyRateService currencyRateService)
        {
            _currencyRateService = currencyRateService;
            _rates = new List<Rate>();
            AvailableTypes = new List<TableType> { TableType.a, TableType.b };

            RateCollectionView = CollectionViewSource.GetDefaultView(_rates);
            RateCollectionView.SortDescriptions.Add(new SortDescription(nameof(Rate.ReportDate), ListSortDirection.Ascending));
            RateCollectionView.SortDescriptions.Add(new SortDescription(nameof(Rate.Code), ListSortDirection.Ascending));

            // TODO: Custom exception 
            LoadCurrencyRatesCommand = new AsyncRelayCommand(LoadCurrencyRates, (ex) => throw ex);
        }




        #endregion

        // Methods
        #region Methods
        private async Task LoadCurrencyRates(object arg)
        {
            _rates.Clear();

            if (_isShowLatest)
            {
                _rates.AddRange(await _currencyRateService.GetCurrentCurrencyRates(_selectedTableType));
            }            
            else if (_isDatePeriod)
            {
                _rates.AddRange(await _currencyRateService.GetCurrencyRatesBetweenDates(_selectedTableType, _selectedStatDate, _selectedEndDate));
            }
            else
            {
                _rates.AddRange(await _currencyRateService.GetCurrencyRatesOnDate(_selectedTableType, _selectedDate));
            }

            RateCollectionView.Refresh();
        }
        #endregion

    }
}
