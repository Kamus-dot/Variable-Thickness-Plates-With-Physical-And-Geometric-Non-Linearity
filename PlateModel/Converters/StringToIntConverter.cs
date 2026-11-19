using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace WPFPlateModel.Converters
{
    class StringToIntConverter : Freezable, IValueConverter
    {
        // ModelView -> View
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is null
                ? DependencyProperty.UnsetValue
                : int.Parse(value.ToString() ?? "0");
        }
        // View -> ModelView
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s && int.TryParse(s, out int number))
            {
                SetCurrentValue(IntErrorTextProperty, null);
                return number;
            }
            SetCurrentValue(IntErrorTextProperty,
                new Dictionary<string, string>() { [parameter?.ToString() ?? ""] = $"Значение '{parameter?.ToString()}' не форматируется" });
            return DependencyProperty.UnsetValue;
        }
        protected override Freezable CreateInstanceCore() => new StringToIntConverter();

        public static readonly DependencyProperty IntErrorTextProperty =
        DependencyProperty.Register(
            nameof(IntErrorText),
            typeof(Dictionary<string, string>),
            typeof(StringToIntConverter));

        public Dictionary<string, string>? IntErrorText
        {
            get => (Dictionary<string, string>?)GetValue(IntErrorTextProperty);
            set => SetValue(IntErrorTextProperty, value);
        }
    }
}
