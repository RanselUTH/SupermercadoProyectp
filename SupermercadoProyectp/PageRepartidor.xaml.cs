using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SupermercadoProyectp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageRepartidor : ContentPage
    {
        public PageRepartidor()
        {
            InitializeComponent();
        }

        private void ver_pedido_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new MainRepartidor());
        }

        private void perfil_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new PagePerfilRepartidor());
        }

        private async void salir_Tapped(object sender, EventArgs e)
        {
            bool Msg = await DisplayAlert("AVISO", "Deseas Cerrar Sesión", "Si", "No");
            if (Msg == true)
            {
                await DisplayAlert("AVISO", "Has Cerrado Sesión", "OK");
                bool hasKey = Preferences.ContainsKey("token");
                Preferences.Remove("token");
                Preferences.Clear();
                await Navigation.PushModalAsync(new Views.PageLogin());


            }
            else
            {
                await DisplayAlert("AVISO", "Cancelado", "OK");
            }
        }
    }
}