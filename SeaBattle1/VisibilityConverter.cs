using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SeaBattle
{
    public class VisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts value of Boolean into WPF-visibility.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        { //преобразует значение Boolean в Visibility
            Visibility _Result = Visibility.Collapsed;
            bool _value = (bool)value;

            if (_value)
            {
                _Result = Visibility.Visible;
            }

            else
            {
                _Result = Visibility.Collapsed;
            }

            return _Result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
