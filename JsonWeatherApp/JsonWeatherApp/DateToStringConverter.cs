using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace JsonWeatherApp
{
    public sealed class DateToStringConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //throw new NotImplementedException();
            //DateTime date = (DateTime)value;
            //return String.Format("{0:dddd}  - {0:d}", date);

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var date = epoch.AddSeconds((int)value);
            return String.Format("{0:dddd}  - {0:d}", date);
            //return epoch.AddSeconds((int)value).ToShortDateString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
