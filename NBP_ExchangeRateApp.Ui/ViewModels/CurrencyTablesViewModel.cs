using NBP_ExchangeRateApp.Library.Enums;
using NBP_ExchangeRateApp.Library.Models;
using NBP_ExchangeRateApp.Library.Services;
using NBP_ExchangeRateApp.Ui.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private bool _isShowLatest = true;
        private string _selectedCode;

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
        public string SelectedCode
        {
            get { return _selectedCode; }
            set
            {
                if(value != _selectedCode)
                {
                    _selectedCode = value;
                    RateCollectionView.Refresh();
                }
            }
        }


        public List<TableType> AvailableTypes { get; private set; }
        public TableType SelectedTableType { get => _selectedTableType; set => _selectedTableType = value; }
        public DateTime SelectedDate { get => _selectedDate; set => _selectedDate = value; }
        public DateTime SelectedStartDate { get => _selectedStatDate; set => _selectedStatDate = value; }
        public DateTime SelectedEndDate { get => _selectedEndDate; set => _selectedEndDate = value; }
        public ICollectionView RateCollectionView { get; set; }
        public ObservableCollection<Rate> RatesCollection { get; set; }
        #endregion

        // Commands
        #region Commands
        public ICommand LoadCurrencyRatesCommand { get; set; }
        public ICommand ClearSelectedCodeCommand { get; set; }
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
            RateCollectionView.Filter = FilterRates;

            // TODO: Custom exception 
            LoadCurrencyRatesCommand = new AsyncRelayCommand(LoadCurrencyRates, (ex) => throw ex);
            ClearSelectedCodeCommand = new RelayCommand(ClearSelectedCode);
        }

        






        #endregion

        // Methods
        #region Methods
        private async Task LoadCurrencyRates(object arg)
        {
            _rates.Clear();

            if (_isShowLatest)
            {
                _rates.AddRange(await _currencyRateService.GetCurrent(_selectedTableType));
            }            
            else if (_isDatePeriod)
            {
                _rates.AddRange(await _currencyRateService.GetBetweenDates(_selectedTableType, _selectedStatDate, _selectedEndDate));
            }
            else
            {
                _rates.AddRange(await _currencyRateService.GetOnDate(_selectedTableType, _selectedDate));
            }

            RateCollectionView.Refresh();
            RatesCollection = new ObservableCollection<Rate>(_rates.GroupBy(x => x.Code)
                .Select(y => y.First())
                .OrderBy(z => z.Code));

            OnPropertyChanged(nameof(RatesCollection));
        }

        private void ClearSelectedCode(object obj)
        {
            SelectedCode = null;
        }

        private bool FilterRates(object obj)
        {
            if (_selectedCode == null)
            {
                return true;
            }
            if(obj is Rate rate)
            {
                return rate.Code.Contains(_selectedCode);
            }

            return false;
        }
        #endregion

    }
}
