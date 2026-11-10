using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
            if (value is string s && double.TryParse(s, culture, out double result))
            {
                SetCurrentValue(ErrorTextProperty, null);
                return result;
            }
            SetCurrentValue(ErrorTextProperty, 
                new Dictionary<string, string>() { [parameter?.ToString() ?? ""] = $"Значение '{parameter?.ToString()}' не является числом!" });
            return DependencyProperty.UnsetValue;
        }
        protected override Freezable CreateInstanceCore() => new StringToDoubleConverter();

        public static readonly DependencyProperty ErrorTextProperty =
        DependencyProperty.Register(
            nameof(ErrorText),
            typeof(Dictionary<string, string>),
            typeof(StringToDoubleConverter));

        public Dictionary<string, string>? ErrorText
        {
            get => (Dictionary<string, string>?)GetValue(ErrorTextProperty);
            set => SetValue(ErrorTextProperty, value);
        }
    }
}
