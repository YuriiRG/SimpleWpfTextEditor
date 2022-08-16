using System;
using System.Windows;
using System.Windows.Data;

namespace SimpleWpfTextEditor.Helpers
{
    public class BoolWrapEnumConverter : IValueConverter
    {
        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              System.Globalization.CultureInfo culture)
        {
            switch ((bool)value)
            {
                case true:
                    return TextWrapping.Wrap;
                case false:
                    return TextWrapping.NoWrap;
            }
        }
        public object ConvertBack(object value,
                              Type targetType,
                              object parameter,
                              System.Globalization.CultureInfo culture)
        {
            switch ((TextWrapping)value)
            {
                case TextWrapping.Wrap:
                    return true;
                case TextWrapping.NoWrap:
                    return false;
                default:
                    throw new Exception();
            }
        }
    }
}
