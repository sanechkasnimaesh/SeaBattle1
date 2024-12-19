using SeaBattle;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SeaBattle
{
    /// <summary>
    /// Содержит конвертер для преобразования значения типа CellStyle в WPF-стиль.
    /// </summary>
    class CellStyleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {//преобразует значение CellStyle и IsInvisible в соответствующий WPF-стиль
            Style _Result = null;

            bool _isInvisible = (bool)values[0];

            if (_isInvisible)
            {
                _Result = Application.Current.FindResource("Fog") as Style;
            }
            else if (values[1] is CellStyle)
            {
                CellStyle _CellStyle = (CellStyle)values[1];

                if (_CellStyle == CellStyle.Empty)
                {
                    _Result = Application.Current.FindResource("Empty") as Style;
                }
                else if (_CellStyle == CellStyle.Shooted)
                {
                    _Result = Application.Current.FindResource("Shooted") as Style;
                }
                else if (_CellStyle == CellStyle.HealthyCell)
                {
                    _Result = Application.Current.FindResource("HealthyCell") as Style;
                }
                else if (_CellStyle == CellStyle.WoundedCell)
                {
                    _Result = Application.Current.FindResource("WoundedCell") as Style;
                }
                else if (_CellStyle == CellStyle.DeadCell)
                {
                    _Result = Application.Current.FindResource("DeadCell") as Style;
                }
            }

            return _Result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
