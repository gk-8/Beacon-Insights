using Template10.Mvvm;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using UniversalBeaconLibrary.Beacon;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth;
using Windows.ApplicationModel.Resources;
using Windows.UI.Core;
using System;
using System.Diagnostics;
using System.Linq;
using BeaconInsightsUWP.Services.Interfaces;

namespace BeaconInsightsUWP.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private ResourceLoader _resourceLoader;
        private CoreDispatcher _dispatcher;
        private INotificationsService _notificationsService;
        private bool _restartingBeaconWatch;

        private readonly BluetoothLEAdvertisementWatcher _watcher;

        private readonly BeaconManager _beaconManager;

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

        private bool _filteringUnknownBeacons;
        public bool FilteringUnknownBeacons
        {
            get
            {
                return _filteringUnknownBeacons;
            }
            set
            {
                Set(ref _filteringUnknownBeacons, value);
                base.RaisePropertyChanged();
            }
        }

        public MainPageViewModel(INotificationsService notificationsService)
        {
            _notificationsService = notificationsService;
            _dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;

            // Create the Bluetooth LE watcher from the Windows 10 UWP
            _watcher = new BluetoothLEAdvertisementWatcher { ScanningMode = BluetoothLEScanningMode.Active };

            // Construct the Universal Bluetooth Beacon manager
            _beaconManager = new BeaconManager();
            BeaconsList = _beaconManager.BluetoothBeacons;
        }

        private async void WatcherOnReceived(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs eventArgs)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                try
                {
                    _beaconManager.ReceivedAdvertisement(eventArgs);
                    BeaconsList = _beaconManager.BluetoothBeacons;
                    //foreach (var beacon in BeaconsList)
                    //{
                    //    beacon.PropertyChanged -= Beacon_PropertyChanged;
                    //    beacon.PropertyChanged += Beacon_PropertyChanged;
                    //}
                }
                catch (ArgumentException e)
                {
                    // Ignore for real-life scenarios.
                    // In some very rare cases, analyzing the data can result in an
                    // Argument_BufferIndexExceedsCapacity. Ignore the error here,
                    // assuming that the next received frame advertisement will be
                    // correct.
                    Debug.WriteLine(e);
                }
            });
        }

        private void Beacon_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Beacon beacon = (sender as Beacon);
            if (e.PropertyName == "ProximityStatus")
            {
                if (beacon.ProximityStatus == Beacon.ProximityStatusEnum.GettingCloser && beacon.BeaconType == Beacon.BeaconTypeEnum.Eddystone
                    && beacon.BeaconFrames.Count > 0 && beacon.BeaconFrames[0] is UrlEddystoneFrame)
                {
                    _notificationsService.Notify((beacon.BeaconFrames[0] as UrlEddystoneFrame).CompleteUrl);
                }
            }
        }

        private void WatcherOnStopped(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementWatcherStoppedEventArgs args)
        {
            string errorMsg = null;
            if (args != null)
            {
                switch (args.Error)
                {
                    case BluetoothError.Success:
                        errorMsg = "WatchingSuccessfullyStopped";
                        break;
                    case BluetoothError.RadioNotAvailable:
                        errorMsg = "ErrorNoRadioAvailable";
                        break;
                    case BluetoothError.ResourceInUse:
                        errorMsg = "ErrorResourceInUse";
                        break;
                    case BluetoothError.DeviceNotConnected:
                        errorMsg = "ErrorDeviceNotConnected";
                        break;
                    case BluetoothError.DisabledByPolicy:
                        errorMsg = "ErrorDisabledByPolicy";
                        break;
                    case BluetoothError.NotSupported:
                        errorMsg = "ErrorNotSupported";
                        break;
                }
            }
            if (errorMsg == null)
            {
                // All other errors - generic error message
                errorMsg = _restartingBeaconWatch
                    ? "FailedRestartingBluetoothWatch"
                    : "AbortedWatchingBeacons";
            }
            StatusLabel = _resourceLoader.GetString(errorMsg);
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            _resourceLoader = ResourceLoader.GetForCurrentView();
            // Start watching
            _watcher.Received += WatcherOnReceived;
            _watcher.Stopped += WatcherOnStopped;
            _watcher.Start();
            if (_watcher.Status == BluetoothLEAdvertisementWatcherStatus.Started)
            {
                StatusLabel = _resourceLoader.GetString("WatchingForBeacons");
            }
            return Task.CompletedTask;
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            StatusLabel = _resourceLoader.GetString("StoppedWatchingBeacons");
            _watcher.Stop();
            _watcher.Received -= WatcherOnReceived;
            _restartingBeaconWatch = false;
            return Task.CompletedTask;
        }

        public override Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            return Task.CompletedTask;
        }

        //public void GotoDetailsPage() =>
        //    NavigationService.Navigate(typeof(Views.DetailPage), Value);
    }
}

