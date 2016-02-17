using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace BeaconInsightsUWP.Converters
{
    public class BluetoothAddressToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var colorSelected = Colors.LightSlateGray;
            if (value != null)
            {
                var bluetoothAddress = value.ToString();
                switch (bluetoothAddress)
                {
                    case "259787379618397":
                        //Mint
                        colorSelected = Colors.Aquamarine;
                        break;
                    case "256098965555991":
                        //Ice
                        colorSelected = Colors.LightSkyBlue;
                        break;
                    case "243772591070564":
                        //Purple
                        colorSelected = Colors.Indigo;
                        break;
                }
            }
            return new SolidColorBrush(colorSelected);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
