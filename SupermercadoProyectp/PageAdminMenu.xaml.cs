using SupermercadoProyectp.Views.Administrador;
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
    public partial class PageAdminMenu : ContentPage
    {
        public PageAdminMenu()
        {
            InitializeComponent();
        }

      

        private void crear_producto_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PageCrearProducto());
        }

        private void crear_repartidor_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PageCrearRepartidor());
        }

        private void lista_productos_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PageListaDeProductosCreados());
        }

        private void lista_pedidos_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PageListaPedidosAdmin());
        }

        private async void tlbcerrar_sesion_Clicked(object sender, EventArgs e)
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
    }
}