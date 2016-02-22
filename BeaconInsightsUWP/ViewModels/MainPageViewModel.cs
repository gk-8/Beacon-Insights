using BeaconInsightsUWP.Services.Interfaces;
using System.Collections.ObjectModel;
using Template10.Mvvm;
using UniversalBeaconLibrary.Beacon;

namespace BeaconInsightsUWP.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private IBeaconManagementService _beaconManagementService;
        
        private ObservableCollection<Beacon> _beaconsList;
        public ObservableCollection<Beacon> BeaconsList
        {
            get { return _beaconsList; }
            set
            {
                Set(ref _beaconsList, value);
                base.RaisePropertyChanged();
            }
        }

        private Beacon _selectedBeacon;
        public Beacon SelectedBeacon
        {
            get { return _selectedBeacon; }
            set
            {
                Set(ref _selectedBeacon, value);
                base.RaisePropertyChanged();
            }
        }


        private string _statusLabel;
        public string StatusLabel
        {
            get
            {
                return _statusLabel;
            }
            set
            {
                Set(ref _statusLabel, value);
                base.RaisePropertyChanged();
            }
        }

        public MainPageViewModel(IBeaconManagementService beaconManagementService)
        {
            _beaconManagementService = beaconManagementService;
            BeaconsList = _beaconManagementService.GetBeaconsList();
            StatusLabel = _beaconManagementService.GetStatusLabel();
        }

        //public void GotoDetailsPage() =>
        //    NavigationService.Navigate(typeof(Views.DetailPage), Value);
    }
}

