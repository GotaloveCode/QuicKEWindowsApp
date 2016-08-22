using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace QuicKE.Client.UI
{
    public sealed class CostConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is decimal)
            value = string.Format("KES. {0}", value.ToString());
            return ((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string && value.ToString().StartsWith("KES."))
                value = value.ToString().Replace("KES. ","");
            value = decimal.Parse(value.ToString());
            return ((decimal)value);
        }
    }
}
