using System;
using System.Globalization;
using LiveInNEU.Models;
using Xamarin.Forms;

namespace LiveInNEU.Converters
{
    public class ItemTappedEventArgsToScheduleConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture) =>
            (value as ItemTappedEventArgs)?.Item as Schedule;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}