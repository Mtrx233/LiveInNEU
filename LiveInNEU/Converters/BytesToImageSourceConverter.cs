using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace LiveInNEU.Converters {
    public class BytesToImageSourceConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture) =>
            !(value is byte[] bytes)
                ? null
                : ImageSource.FromStream(() => new MemoryStream(bytes));

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}