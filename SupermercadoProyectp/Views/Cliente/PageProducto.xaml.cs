using Delivery;
using Firebase.Database;
using SupermercadoProyectp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SupermercadoProyectp.Views.Cliente
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageProductos : ContentPage
    {
        productos _ProductoRepository = new productos();
        public PageProductos()
        {
            InitializeComponent();

        }

        protected override async void OnAppearing()
        {
            var productos = await _ProductoRepository.GetAll();
            cvListaProductos.ItemsSource = null;
            cvListaProductos.ItemsSource = productos;

            // cvListaProductos.IsRefreshing = false;
        }


        private async void cvListaProductos_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            

            Producto oProducto = (Producto)e.CurrentSelection.FirstOrDefault();
            await Navigation.PushAsync(new PageDetalleProducto(oProducto));

        }


     


        private async void Addtolbaritem_Clicked(object sender, EventArgs e)
        {
            bool Msg = await DisplayAlert("AVISO", "Deseas Cerrar Sesión", "Si", "No");
            if (Msg == true)
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

        private async void Addtolbaritem_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageCarrito());

        }

        private async void perfil_usuario_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PagePerfilUser());
        }


        private async void buscarlbl_SearchButtonPressed(object sender, EventArgs e)
        {
            string searchValue = buscarlbl.Text;
            if (!String.IsNullOrEmpty(searchValue))
            {
                var productos = await _ProductoRepository.GetAllBy(searchValue);
                cvListaProductos.ItemsSource = null;
                cvListaProductos.ItemsSource = productos;
            }
            else
            {
                OnAppearing();
            }
        }

        private async void buscarlbl_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchValue = buscarlbl.Text;
            if (!String.IsNullOrEmpty(searchValue))
            {
                var productos = await _ProductoRepository.GetAllBy(searchValue);
                cvListaProductos.ItemsSource = null;
                cvListaProductos.ItemsSource = productos;
            }
            else
            {
                OnAppearing();
            }
        }
    }
}