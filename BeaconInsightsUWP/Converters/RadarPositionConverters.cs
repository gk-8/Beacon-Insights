using System;
using System.Collections.ObjectModel;
using System.Linq;
using UniversalBeaconLibrary.Beacon;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace BeaconInsightsUWP.Converters
{
    public class RadarPositionColumnConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var column = 0;
            if (parameter != null)
            {
                var bluetoothAddress = parameter.ToString();
                switch (bluetoothAddress)
                {
                    case "259787379618397":
                        //Mint
                        column = 0;
                        break;
                    case "256098965555991":
                        //Ice
                        column = 1;
                        break;
                    case "243772591070564":
                        //Purple
                        column = 2;
                        break;
                }
            }
            return column;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class RadarPositionRowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ObservableCollection<Beacon> beaconList = value as ObservableCollection<Beacon>;
            string beaconBluetoothAddress = parameter != null ? parameter.ToString() : "";
            Beacon selectedBeacon = beaconList.Where(b => b.BluetoothAddress.ToString() == beaconBluetoothAddress).FirstOrDefault();

            int row = 0;

            if (selectedBeacon != null)
            {
                switch (selectedBeacon.ProximityRange)
                {
                    case Beacon.ProximityRangeEnum.Immediate:
                        row = 1;
                        break;
                    case Beacon.ProximityRangeEnum.Near:
                        row = 2;
                        break;
                    case Beacon.ProximityRangeEnum.Far:
                        row = 3;
                        break;
                    case Beacon.ProximityRangeEnum.Unknown:
                        row = 0;
                        break;
                }
            }

            return row;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class RadarPositionVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ObservableCollection<Beacon> beaconList = value as ObservableCollection<Beacon>;
            string beaconBluetoothAddress = parameter != null ? parameter.ToString() : "";
            Beacon selectedBeacon = beaconList.Where(b => b.BluetoothAddress.ToString() == beaconBluetoothAddress).FirstOrDefault();

            return selectedBeacon != null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
