using System;
using Template10.Mvvm;
using UniversalBeaconLibrary.Beacon;

namespace BeaconInsightsUWP.Models
{
    public class BeaconMessage : BindableBase
    {
        private string _device;
        private string _beaconName;
        private double _distance;
        private string _proximityRange;
        private float _temperature;
        private DateTime _time;

        public string Device
        {
            get { return _device; }
            set
            {
                Set(ref _device, value);
                base.RaisePropertyChanged();
            }
        }
        public string BeaconName
        {
            get { return _beaconName; }
            set
            {
                Set(ref _beaconName, value);
                base.RaisePropertyChanged();
            }
        }
        public double Distance
        {
            get { return _distance; }
            set
            {
                Set(ref _distance, value);
                base.RaisePropertyChanged();
            }
        }
        public string ProximityRange
        {
            get { return _proximityRange; }
            set
            {
                Set(ref _proximityRange, value);
                base.RaisePropertyChanged();
            }
        }
        public float Temperature
        {
            get { return _temperature; }
            set
            {
                Set(ref _temperature, value);
                base.RaisePropertyChanged();
            }
        }
        public DateTime Time
        {
            get { return _time; }
            set
            {
                Set(ref _time, value);
                base.RaisePropertyChanged();
            }
        }

        public BeaconMessage(Beacon beacon)
        {
            BeaconName = GetBeaconName(beacon.BluetoothAddress.ToString());
            Distance = beacon.Distance;
            ProximityRange = beacon.ProximityRange.ToString();
            Temperature = beacon.GetTemperature();
            Time = DateTime.Now;
        }

        private string GetBeaconName(string bluetoothAddress)
        {
            var name = "Unknown";
            switch (bluetoothAddress)
            {
                case "259787379618397":
                    //Mint
                    name = "Mint cocktail";
                    break;
                case "256098965555991":
                    //Ice
                    name = "Icy marshmallow";
                    break;
                case "243772591070564":
                    //Purple
                    name = "Blueberry pie";
                    break;
            }
            return name;
        }
    }
}
