using System.Threading.Tasks;
using BeaconInsightsUWP.Services.Interfaces;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using UniversalBeaconLibrary.Beacon;

namespace BeaconInsightsUWP.ViewModels
{
    public class AdvertisingPageViewModel : ViewModelBase
    {
        private IBeaconManagementService _beaconManagementService;

        private Beacon.BeaconTypeEnum _selectedProtocol;
        public Beacon.BeaconTypeEnum SelectedProtocol
        {
            get { return _selectedProtocol; }
            set
            {
                Set(ref _selectedProtocol, value);
                base.RaisePropertyChanged();
            }
        }

        private string _rawPayloadBroadcasted;
        public string RawPayloadBroadcasted
        {
            get { return _rawPayloadBroadcasted; }
            set
            {
                Set(ref _rawPayloadBroadcasted, value);
                base.RaisePropertyChanged();
            }
        }

        public AdvertisingPageViewModel(IBeaconManagementService beaconManagementService)
        {
            _beaconManagementService = beaconManagementService;

            _beaconManagementService.SetAdvertisingPayload(SelectedProtocol);
            _beaconManagementService.StartAdvertising();
        }

        public override Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            _beaconManagementService.StopAdvertising();
            return base.OnNavigatingFromAsync(args);
        }
    }
}
