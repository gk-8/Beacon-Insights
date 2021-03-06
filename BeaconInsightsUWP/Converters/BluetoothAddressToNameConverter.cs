﻿using System;
using UniversalBeaconLibrary.Beacon;
using Windows.UI.Xaml.Data;

namespace BeaconInsightsUWP.Converters
{
    public class BluetoothAddressToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var name = "Unknown";
            if (value != null)
            {
                var beacon = value as Beacon;
                var bluetoothAddress = beacon.BluetoothAddress.ToString();
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
                    default:
                        if (beacon.BeaconType != Beacon.BeaconTypeEnum.Unknown)
                            name = "Clockwork orange";
                        break;
                }
            }
            return name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
