using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace SupermercadoProyectp.Convert
{
    public class ByteArrayToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ImageSource retSource = null; //Se valida que el objeto  a convertir no vallan a ser nulos
            if (value != null)
            {

                byte[] imageAsBytes = System.Convert.FromBase64String((string)value);
                retSource = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));

            }
            Debug.WriteLine((string)value);
            return retSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

