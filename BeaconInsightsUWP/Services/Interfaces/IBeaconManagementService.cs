using System.Collections.ObjectModel;
using UniversalBeaconLibrary.Beacon;
using Windows.Devices.Bluetooth.Advertisement;

namespace BeaconInsightsUWP.Services.Interfaces
{
    public interface IBeaconManagementService
    {
        void StartScanning();
        void StopScanning();
        ObservableCollection<Beacon> GetBeaconsList();
        string GetStatusLabel();
        BluetoothLEAdvertisementWatcher GetWatcher();
    }
}
