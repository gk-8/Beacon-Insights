using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalBeaconLibrary.Beacon;

namespace BeaconInsightsUWP.Services.Interfaces
{
    public interface IBeaconManagementService
    {
        void StartScanning();
        void StopScanning();
        ObservableCollection<Beacon> GetBeaconsList();
        string GetStatusLabel();
    }
}
