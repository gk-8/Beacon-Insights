using BeaconInsightsUWP.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Template10.Mvvm;
using UniversalBeaconLibrary.Beacon;
using Windows.ApplicationModel.Resources;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Storage.Streams;
using Windows.UI.Core;

namespace BeaconInsightsUWP.Services
{
    public class BeaconManagementService : BindableBase, IBeaconManagementService
    {
        private ResourceLoader _resourceLoader;
        private CoreDispatcher _dispatcher;
        private bool _restartingBeaconWatch;

        private readonly BluetoothLEAdvertisementWatcher _watcher;
        private readonly BluetoothLEAdvertisementPublisher _publisher;

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

        public BeaconManagementService()
        {
            _dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;

            // Create the Bluetooth LE watcher from the Windows 10 UWP
            _watcher = new BluetoothLEAdvertisementWatcher { ScanningMode = BluetoothLEScanningMode.Active };

            // Create the Bluetooth LE publisher from the Windows 10 UWP
            _publisher = new BluetoothLEAdvertisementPublisher();

            // Construct the Universal Bluetooth Beacon manager
            _beaconManager = new BeaconManager();
            BeaconsList = _beaconManager.BluetoothBeacons;

            _resourceLoader = ResourceLoader.GetForCurrentView();
            StartScanning();
        }

        public void StartScanning()
        {
            // Start watching
            _watcher.Received += WatcherOnReceived;
            _watcher.Stopped += WatcherOnStopped;
            _watcher.Start();
            if (_watcher.Status == BluetoothLEAdvertisementWatcherStatus.Started)
            {
                StatusLabel = _resourceLoader.GetString("WatchingForBeacons");
            }
        }

        public void StopScanning()
        {
            StatusLabel = _resourceLoader.GetString("StoppedWatchingBeacons");
            _watcher.Stop();
            _watcher.Received -= WatcherOnReceived;
            _restartingBeaconWatch = false;
        }

        private async void WatcherOnReceived(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs eventArgs)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                try
                {
                    _beaconManager.ReceivedAdvertisement(eventArgs);
                    BeaconsList = _beaconManager.BluetoothBeacons;
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

        public ObservableCollection<Beacon> GetBeaconsList()
        {
            return BeaconsList;
        }

        public string GetStatusLabel()
        {
            return StatusLabel;
        }

        public BluetoothLEAdvertisementWatcher GetWatcher()
        {
            return _watcher;
        }

        public void StartAdvertising()
        {
            _publisher.Start();
        }

        public void StopAdvertising()
        {
            _publisher.Stop();
        }

        public void SetAdvertisingPayload(Beacon.BeaconTypeEnum protocol)
        {
            var writer = new DataWriter();
            UInt16 uuidData = 0x1234;
            writer.WriteUInt16(uuidData);

            var manufacturerData = new BluetoothLEManufacturerData();
            //manufacturerData.CompanyId = 0x0006; // Microsoft
            manufacturerData.CompanyId = 0x4C00; // iBeacon
            manufacturerData.Data = writer.DetachBuffer();

            _publisher.Advertisement.ManufacturerData.Clear();
            _publisher.Advertisement.ManufacturerData.Add(manufacturerData);
        }
    }
}
