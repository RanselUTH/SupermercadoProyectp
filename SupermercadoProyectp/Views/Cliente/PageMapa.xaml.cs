using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SupermercadoProyectp.Views.Cliente
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageMapa : ContentPage
    {
        List<double> latlong;
        public PageMapa(List<double> ll)
        {
            InitializeComponent();
            latlong = ll;
            
        }

        private async void map_MapClicked(object sender, MapClickedEventArgs e)
        {
            var lat = e.Position.Latitude;
            var longg = e.Position.Longitude;
            latlong.Add(lat);
            latlong.Add(longg);
            await Navigation.PopModalAsync();

        }


    }
}