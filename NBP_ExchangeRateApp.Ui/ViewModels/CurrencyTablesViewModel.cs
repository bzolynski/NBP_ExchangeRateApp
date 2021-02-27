using NBP_ExchangeRateApp.Library.Enums;
using NBP_ExchangeRateApp.Library.Models;
using NBP_ExchangeRateApp.Library.Services;
using NBP_ExchangeRateApp.Ui.Commands;
using NBP_ExchangeRateApp.Ui.Validators;
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
    public class CurrencyTablesViewModel : ViewModelBase, IApiQueryableViewModel, IDataErrorInfo
    {

        // Validation
        #region Validation
        public string Error => null;

        public string this[string propertyName] 
        {
            get
            {
                var errors = _validationRules.Validate(this).Errors;

                _canSubmit = errors.Count() > 0 ? false : true;

                var error = errors.FirstOrDefault(e => e.PropertyName == propertyName);

                if (ErrorCollection.ContainsKey(propertyName) && error != null)
                {
                    ErrorCollection[propertyName] = error.ErrorMessage;
                }
                else if (error != null)
                {
                    ErrorCollection.Add(propertyName, error.ErrorMessage);
                }
                else
                {
                    ErrorCollection.Remove(propertyName);
                }

                OnPropertyChanged(nameof(ErrorCollection));

                if (ErrorCollectionView != null)
                    ErrorCollectionView.Refresh();

                return error != null ? error.ErrorMessage : null;
            }
        }

        #endregion

        // Private fields
        #region Private fields
        private bool _canSubmit;
        private bool _isDatePeriod;
        private bool _isShowLatest = true;
        private List<Rate> _rates;
        private Dictionary<string, string> _errorCollection;
        private string _selectedCode;
        private TableType _selectedTableType;
        private DateTime _selectedDate = DateTime.Today;
        private DateTime _selectedStartDate = DateTime.Today;
        private DateTime _selectedEndDate = DateTime.Today;

        private readonly INBPCurrencyRateService _currencyRateService;
        private readonly ApiQueryableValidator _validationRules;

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
                OnPropertyChanged(nameof(SelectedDate));
                OnPropertyChanged(nameof(SelectedStartDate));
                OnPropertyChanged(nameof(SelectedEndDate));
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
                OnPropertyChanged(nameof(SelectedDate));
                OnPropertyChanged(nameof(SelectedStartDate));
                OnPropertyChanged(nameof(SelectedEndDate));
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
        public TableType SelectedTableType { get => _selectedTableType; set => _selectedTableType = value; }
        public DateTime SelectedDate { get => _selectedDate; set => _selectedDate = value; }
        public DateTime SelectedStartDate
        {
            get { return _selectedStartDate; }
            set
            {
                _selectedStartDate = value;
                OnPropertyChanged(nameof(SelectedStartDate));
                OnPropertyChanged(nameof(SelectedEndDate));
            }
        }
        public DateTime SelectedEndDate
        {
            get { return _selectedEndDate; }
            set
            {
                _selectedEndDate = value;
                OnPropertyChanged(nameof(SelectedStartDate));
                OnPropertyChanged(nameof(SelectedEndDate));
            }
        }
         
        public List<TableType> AvailableTypes { get; private set; }
        public Dictionary<string, string> ErrorCollection
        {
            get { return _errorCollection; }
            set
            {
                _errorCollection = value;
                OnPropertyChanged(nameof(ErrorCollection));
            }
        }
        public ICollectionView RateCollectionView { get; set; }
        public ICollectionView ErrorCollectionView { get; set; }
        public ObservableCollection<Rate> RatesCollection { get; set; }
        #endregion

        // Commands
        #region Commands
        public ICommand LoadCurrencyRatesCommand { get; set; }
        
        #endregion

        // Constructors
        #region Constructors
        public CurrencyTablesViewModel(INBPCurrencyRateService currencyRateService, ApiQueryableValidator validationRules)
        {
            _currencyRateService = currencyRateService;
            _validationRules = validationRules;
            ErrorCollection = new();
            _rates = new();
            AvailableTypes = new List<TableType> { TableType.a, TableType.b };

            ErrorCollectionView = CollectionViewSource.GetDefaultView(_errorCollection.Values);

            RateCollectionView = CollectionViewSource.GetDefaultView(_rates);
            RateCollectionView.SortDescriptions.Add(new SortDescription(nameof(Rate.ReportDate), ListSortDirection.Ascending));
            RateCollectionView.SortDescriptions.Add(new SortDescription(nameof(Rate.Code), ListSortDirection.Ascending));
            RateCollectionView.Filter = FilterRates;

            // TODO: Custom exception 
            LoadCurrencyRatesCommand = new AsyncRelayCommand(LoadCurrencyRates, (obj) => _canSubmit, (ex) => throw ex);
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
                _rates.AddRange(await _currencyRateService.GetBetweenDates(_selectedTableType, _selectedStartDate, _selectedEndDate));
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
