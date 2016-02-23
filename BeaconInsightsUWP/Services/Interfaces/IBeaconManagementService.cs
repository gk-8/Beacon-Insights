using System.Collections.ObjectModel;
using UniversalBeaconLibrary.Beacon;
using Windows.Devices.Bluetooth.Advertisement;

namespace BeaconInsightsUWP.Services.Interfaces
{
    public interface IBeaconManagementService
    {
        void StartScanning();
        void StopScanning();
        void StartAdvertising();
        void StopAdvertising();
        void SetAdvertisingPayload(Beacon.BeaconTypeEnum protocol);
        ObservableCollection<Beacon> GetBeaconsList();
        string GetStatusLabel();
        BluetoothLEAdvertisementWatcher GetWatcher();
    }
}
