using Delivery;
using SupermercadoProyectp.Models;
using SupermercadoProyectp.Views.Cliente;
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
    public partial class PageClienteMenu : ContentPage
    {
        productos _ProductoRepository = new productos();
        public PageClienteMenu()
        {
            InitializeComponent();
        }

       




        private void inicio_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PageClienteMenu());
        }

        

        private void bolsa_de_compra_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Cliente.PageProductos());
        }

        private void pedidos_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PagePedidosUser());
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
                await Navigation.PopToRootAsync();


            }
            else
            {
                await DisplayAlert("AVISO", "Cancelado", "OK");
            }

        }

        private void perfil_usuario_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new PagePerfilUser());
        }
    }
}