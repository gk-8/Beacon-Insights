using BeaconInsightsUWP.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using Template10.Mvvm;
using UniversalBeaconLibrary.Beacon;
using Windows.UI.Xaml;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using BeaconInsightsUWP.Models;

namespace BeaconInsightsUWP.ViewModels
{
    public class RadarPageViewModel : ViewModelBase
    {
        private IBeaconManagementService _beaconManagementService;
        private DispatcherTimer _dispatcherTimer;

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
        public ObservableCollection<Beacon> RadarBeaconsList
        {
            get { return _beaconsList; }
            set
            {
                Set(ref _beaconsList, value);
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

        public string ImmediateDistanceLabel { get { return AppConfig.ImmediateDistance + " m."; } }
        public string NearDistanceLabel { get { return AppConfig.NearDistance + " m."; } }

        public RadarPageViewModel(IBeaconManagementService beaconManagementService)
        {
            _beaconManagementService = beaconManagementService;
            BeaconsList = _beaconManagementService.GetBeaconsList();
            StatusLabel = _beaconManagementService.GetStatusLabel();

            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            _dispatcherTimer.Tick += UpdateRadarList;
            _dispatcherTimer.Start();
        }

        private void UpdateRadarList(object sender, object e)
        {
            RadarBeaconsList = BeaconsList;
        }

        public override Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            _dispatcherTimer.Stop();
            return Task.CompletedTask;
        }
    }
}
