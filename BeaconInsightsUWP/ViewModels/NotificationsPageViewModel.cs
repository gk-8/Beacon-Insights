using BeaconInsightsUWP.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using UniversalBeaconLibrary.Beacon;
using Windows.Devices.Bluetooth.Advertisement;

namespace BeaconInsightsUWP.ViewModels
{
    public class NotificationsPageViewModel : ViewModelBase
    {
        private IBeaconManagementService _beaconManagementService;
        private INotificationsService _notificationsService;
        private BluetoothLEAdvertisementWatcher _watcher;

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
                SetNotificationContent(_selectedBeacon);
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

        private string _notificationTitle;
        public string NotificationTitle
        {
            get
            {
                return _notificationTitle;
            }
            set
            {
                Set(ref _notificationTitle, value);
                base.RaisePropertyChanged();
            }
        }

        private string _notificationBody;
        public string NotificationBody
        {
            get
            {
                return _notificationBody;
            }
            set
            {
                Set(ref _notificationBody, value);
                base.RaisePropertyChanged();
            }
        }

        private object _notificationParameter;
        public object NotificationParameter
        {
            get
            {
                return _notificationParameter;
            }
            set
            {
                Set(ref _notificationParameter, value);
                base.RaisePropertyChanged();
            }
        }

        public NotificationsPageViewModel(IBeaconManagementService beaconManagementService, INotificationsService notificationsService)
        {
            _beaconManagementService = beaconManagementService;
            _notificationsService = notificationsService;
            BeaconsList = _beaconManagementService.GetBeaconsList();
            StatusLabel = _beaconManagementService.GetStatusLabel();

            _watcher = _beaconManagementService.GetWatcher();
            _watcher.Received += Watcher_Received;
        }

        private void Watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            foreach (var beacon in BeaconsList)
            {
                beacon.PropertyChanged -= Beacon_PropertyChanged;
                beacon.PropertyChanged += Beacon_PropertyChanged;
            }
        }

        private void Beacon_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Beacon beacon = (sender as Beacon);
            if (e.PropertyName == "ProximityStatus")
            {
                if (beacon.ProximityRange == Beacon.ProximityRangeEnum.Immediate && beacon.ProximityStatus == Beacon.ProximityStatusEnum.GettingCloser
                    && beacon.BeaconFrames.Count > 0 && beacon.BeaconType == Beacon.BeaconTypeEnum.Eddystone)
                {
                    SelectedBeacon = beacon;
                    SetNotificationContent(beacon);
                    if (beacon.GetUrlEddystoneFrame() != null)
                        _notificationsService.NotifyWithUrl(NotificationTitle, NotificationBody, NotificationParameter.ToString());
                    else if (beacon.GetTlmEddystoneFrame() != null)
                        _notificationsService.NotifyWithTemperature(NotificationTitle, NotificationBody, (float)NotificationParameter);
                }
                else if (beacon.ProximityRange == Beacon.ProximityRangeEnum.Far && beacon.ProximityStatus == Beacon.ProximityStatusEnum.GettingFurther
                    && beacon.BeaconFrames.Count > 0 && beacon.BeaconType == Beacon.BeaconTypeEnum.iBeacon && beacon.GetProximityFrame() != null)
                {
                    SelectedBeacon = beacon;
                    SetNotificationContent(beacon);
                    _notificationsService.NotifyWithDistance(NotificationTitle, NotificationBody, (double)NotificationParameter);
                }
            }
        }

        private void SetNotificationContent(Beacon beacon)
        {
            if (beacon.BeaconType == Beacon.BeaconTypeEnum.Eddystone)
            {
                if (beacon.GetUrlEddystoneFrame() != null)
                {
                    NotificationTitle = "Welcome home!";
                    NotificationBody = "Which TV-show would you wanna watch today?";
                    NotificationParameter = beacon.GetUrlEddystoneFrame().CompleteUrl;
                }
                else if (beacon.GetTlmEddystoneFrame() != null)
                {
                    NotificationTitle = "You have been out for a while!";
                    NotificationBody = "The temperature at the house is {0} ºC";
                    NotificationParameter = beacon.GetTlmEddystoneFrame().TemperatureInC;
                }
            }
            else if (beacon.BeaconType == Beacon.BeaconTypeEnum.iBeacon)
            {
                NotificationTitle = "Have a nice day!";
                NotificationBody = "Hope to see you again :)";
                NotificationParameter = beacon.Distance;
            }
            else {
                NotificationTitle = "N/A";
                NotificationBody = "N/A";
                NotificationParameter = "N/A";
            }
        }

        public override Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            _watcher.Received -= Watcher_Received;
            foreach (var beacon in BeaconsList)
                beacon.PropertyChanged -= Beacon_PropertyChanged;

            return base.OnNavigatingFromAsync(args);
        }
    }
}
