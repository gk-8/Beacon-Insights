using System;
using Windows.UI.Xaml.Data;

namespace BeaconInsightsUWP.Converters
{
    public class TimeSincePowerUpConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = value;
            if (parameter != null)
            {
                var unities = parameter.ToString().ToLower();
                switch (unities)
                {
                    case "seconds":
                        result = (uint)value * 10;
                        break;
                    case "minutes":
                        result = ((uint)value * 10) / 60;
                        break;
                    case "hours":
                        result = ((uint)value * 10) / 60 / 60;
                        break;
                    case "days":
                        result = ((uint)value * 10) / 60 / 60 / 24;
                        break;
                    case "years":
                        result = ((uint)value * 10) / 60 / 60 / 24 / 365;
                        break;
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
