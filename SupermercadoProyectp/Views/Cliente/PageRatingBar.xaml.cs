using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SupermercadoProyectp.Views.Cliente
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageRatingBar : ContentPage
    {
        public PageRatingBar()
        {
            InitializeComponent();
        }

        private void ShowButton_Clicked(object sender, EventArgs e)
        {
            int rating = calificacion.SelectedStarValue;
            DisplayAlert("Tu Calificacion", rating.ToString(), "OK");

        }

    }
}