using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace MalifoApp.Converters
{
    public class BoolToImage : IValueConverter
    {
        public ImageSource TrueImage { get; set; }
        public ImageSource FalseImage { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is bool))
            {
                return null;
            }

            bool b = (bool)value;
            if (b)
            {
                return this.TrueImage;
            }
            else
            {
                return this.FalseImage;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
