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
    public class GoldPricesViewModel : ViewModelBase, IApiQueryableViewModel, IDataErrorInfo
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
        private Dictionary<string, string> _errorCollection;
        private List<GoldReport> _goldReports;
        private DateTime _selectedDate = DateTime.Today;
        private DateTime _selectedStartDate = DateTime.Today;
        private DateTime _selectedEndDate = DateTime.Today;

        private readonly INBPGoldValueService _goldValueService;
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
        public Dictionary<string, string> ErrorCollection
        {
            get { return _errorCollection; }
            set
            {
                _errorCollection = value;
                OnPropertyChanged(nameof(ErrorCollection));
            }
        }
        public ICollectionView GoldPriceCollectionView { get; set; }
        public ICollectionView ErrorCollectionView { get; set; }
        #endregion

        // Commands
        #region Commands
        public ICommand LoadGoldPricesCommand { get; set; }
        #endregion

        // Constructors
        #region Constructors
        public GoldPricesViewModel(INBPGoldValueService goldValueService, ApiQueryableValidator validationRules)
        {
            _goldValueService = goldValueService;
            _validationRules = validationRules;
            ErrorCollection = new();
            _goldReports = new();

            ErrorCollectionView = CollectionViewSource.GetDefaultView(_errorCollection.Values);

            GoldPriceCollectionView = CollectionViewSource.GetDefaultView(_goldReports);
            GoldPriceCollectionView.SortDescriptions.Add(new SortDescription(nameof(GoldReport.ReportDate), ListSortDirection.Ascending));

            LoadGoldPricesCommand = new AsyncRelayCommand(LoadGoldPrices, (obj) => _canSubmit, (ex) => OpenDialog(ex.Message));
        }


        #endregion

        // Methods
        #region Methods
        private async Task LoadGoldPrices(object arg)
        {
            _goldReports.Clear();

            if (_isShowLatest)
            {
                _goldReports.AddRange(await _goldValueService.GetCurrent());
            }
            else if (_isDatePeriod)
            {
                _goldReports.AddRange(await _goldValueService.GetBetweenDates(_selectedStartDate, _selectedEndDate));
            }
            else
            {
                _goldReports.AddRange(await _goldValueService.GetOnDate(_selectedDate));
            }

            GoldPriceCollectionView.Refresh();
        }
        #endregion

    }
}
