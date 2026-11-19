using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WPFPlateModel.Converters
{
    class StringToDoubleConverter : Freezable, IValueConverter
    {
        // ViewModel -> View
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is null
                ? DependencyProperty.UnsetValue
                : double.Parse(value.ToString() ?? "0");
        }
        // View -> ViewModel
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (double.TryParse((string)value, NumberFormatInfo.InvariantInfo, out double result))
            {
                SetCurrentValue(DoubleErrorTextProperty, null);
                return result;
            }
            else if (double.TryParse((string)value, culture, out double res))
            {
                SetCurrentValue(DoubleErrorTextProperty, null);
                return res;
            }
            SetCurrentValue(DoubleErrorTextProperty,
                new Dictionary<string, string>() { [parameter?.ToString() ?? ""] = $"Значение '{parameter?.ToString()}' не является числом!" });
            return DependencyProperty.UnsetValue;
        }
        protected override Freezable CreateInstanceCore() => new StringToDoubleConverter();

        public static readonly DependencyProperty DoubleErrorTextProperty =
        DependencyProperty.Register(
            nameof(DoubleErrorText),
            typeof(Dictionary<string, string>),
            typeof(StringToDoubleConverter));

        public Dictionary<string, string>? DoubleErrorText
        {
            get => (Dictionary<string, string>?)GetValue(DoubleErrorTextProperty);
            set => SetValue(DoubleErrorTextProperty, value);
        }
    }
}
