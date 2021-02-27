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
    public class GoldPricesViewModel : ViewModelBase
    {
        // Private fields
        #region Private fields
        private bool _isDatePeriod;
        private bool _isShowLatest = true;

        private List<GoldReport> _goldReports;
        private DateTime _selectedDate = DateTime.Today;
        private DateTime _selectedStatDate = DateTime.Today;
        private DateTime _selectedEndDate = DateTime.Today;
        private readonly INBPGoldValueService _goldValueService;

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

        public DateTime SelectedDate { get => _selectedDate; set => _selectedDate = value; }
        public DateTime SelectedStartDate { get => _selectedStatDate; set => _selectedStatDate = value; }
        public DateTime SelectedEndDate { get => _selectedEndDate; set => _selectedEndDate = value; }
        public ICollectionView GoldPriceCollectionView { get; set; }
        #endregion

        // Commands
        #region Commands
        public ICommand LoadGoldPricesCommand { get; set; }
        #endregion

        // Constructors
        #region Constructors
        public GoldPricesViewModel(INBPGoldValueService goldValueService)
        {
            _goldValueService = goldValueService;

            LoadGoldPricesCommand = new AsyncRelayCommand(LoadGoldPrices, (ex) => throw ex);

            _goldReports = new();
            GoldPriceCollectionView = CollectionViewSource.GetDefaultView(_goldReports);
            GoldPriceCollectionView.SortDescriptions.Add(new SortDescription(nameof(GoldReport.ReportDate), ListSortDirection.Ascending));

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
                _goldReports.AddRange(await _goldValueService.GetBetweenDates(_selectedStatDate, _selectedEndDate));
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
