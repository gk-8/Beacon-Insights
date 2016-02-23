using BeaconInsightsUWP.Models;
using BeaconInsightsUWP.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using Template10.Mvvm;
using UniversalBeaconLibrary.Beacon;
using Windows.Devices.Bluetooth.Advertisement;

namespace BeaconInsightsUWP.ViewModels
{
    public class CloudPageViewModel : ViewModelBase
    {
        private IBeaconManagementService _beaconManagementService;
        private IAzureEventHubService _azureEventHubService;
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

        public CloudPageViewModel(IBeaconManagementService beaconManagementService, IAzureEventHubService azureEventHubService)
        {
            _beaconManagementService = beaconManagementService;
            _azureEventHubService = azureEventHubService;
            BeaconsList = _beaconManagementService.GetBeaconsList();
            StatusLabel = _beaconManagementService.GetStatusLabel();

            _watcher = _beaconManagementService.GetWatcher();
            _watcher.Received += Watcher_Received;
        }

        private void Watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            try
            {
                foreach (var beacon in BeaconsList)
                {
                    if (beacon.BeaconType != Beacon.BeaconTypeEnum.Unknown)
                    {
                        BeaconMessage bm = new BeaconMessage(beacon);
                        bm.Device = "GorkmaSP3";
                        _azureEventHubService.SendMessage(JsonConvert.SerializeObject(bm));
                    }
                }
            }
            catch (Exception)
            {
                // Sometimes the collection changes while getting through the foreach loop
            }
        }

    }
}