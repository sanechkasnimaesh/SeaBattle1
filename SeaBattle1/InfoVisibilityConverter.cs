using SeaBattle;
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
    /// <summary>
    /// Converts FrameworkElement into Visibility depending on the name of FrameworkElement and GameStage.
    /// </summary>
    public class InfoVisibilityConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility _Result = Visibility.Collapsed;

            FrameworkElement _SP = (FrameworkElement)values[0];

            if (values[1] is GameStage)
            {
                GameStage _stage = (GameStage)values[1];

                if (_SP.Name == "Stage1")
                {
                    if (_stage == GameStage.BoatsArrange)
                    {
                        _Result = Visibility.Visible;
                    }
                }
                else if (_SP.Name == "Stage2")
                {
                    if (_stage == GameStage.Playing)
                    {
                        _Result = Visibility.Visible;
                    }
                }
                else if (_SP.Name == "Stage3")
                {
                    if (_stage == GameStage.Finished)
                    {
                        _Result = Visibility.Visible;
                    }
                }
            }

            return _Result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
