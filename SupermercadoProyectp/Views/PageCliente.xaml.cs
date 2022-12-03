using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SupermercadoProyectp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageCliente : ContentPage
    {
        public PageCliente()
        {
            InitializeComponent();
            LblUser.Text = Preferences.Get("userEmail", "default");
        }

        private async void ButtonSesion_Clicked(object sender, EventArgs e)
        {

            bool Msg = await DisplayAlert("AVISO", "Deseas Cerrar Sesión", "Si", "No");
            if (Msg== true)
            {
                await DisplayAlert("AVISO", "Has Cerrado Sesión", "OK");
                bool hasKey = Preferences.ContainsKey("token");
                Preferences.Remove("token");
                Preferences.Clear();
                await Navigation.PopAsync();


            }
            else
            {
                await DisplayAlert("AVISO", "Cancelado", "OK");
            }




        }


    


    }
}