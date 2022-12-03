using SupermercadoProyectp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SupermercadoProyectp.Views.Administrador
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageListaDeProductosCreados : ContentPage
    {
        productos _ProductoRepository = new productos();
        public PageListaDeProductosCreados()
        {
            InitializeComponent();
            ListaProducto.RefreshCommand = new Command(() =>
            {
                OnAppearing();
            });

        }

        protected override async void OnAppearing()
        {
            var productos = await _ProductoRepository.GetAll();
            ListaProducto.ItemsSource = null;
            ListaProducto.ItemsSource = productos;
            ListaProducto.IsRefreshing = false;
        }


        private void Addtolbaritem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new PageCrearProducto());

        }

        private void ListaProducto_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }
            var producto = e.Item as Producto;
            Navigation.PushModalAsync(new PageDetalles(producto));
            ((ListView)sender).SelectedItem = null;


        }

        /*

        private async void DeleteTap_Tapped(object sender, EventArgs e)
        {
          var response = await  DisplayAlert("Aviso", "Deseas Borrar Este Producto", "SI","NO");
            if (response)
            {
                string id = ((TappedEventArgs)e).Parameter.ToString();
               bool is_delete = await _ProductoRepository.Delete(id);
                if (is_delete)
                {
                    await DisplayAlert("Aviso", "Producto Borrado", "OK");
                     OnAppearing();
                }
                else
                {
                    await DisplayAlert("Aviso", "Error Al Eliminar Producto", "OK");
                }
            }
        }
        */
        private async void EditTap_Tapped(object sender, EventArgs e)
        {
            //DisplayAlert("Aviso", "Deseas Editar Este Producto", "OK");
            string id = ((TappedEventArgs)e).Parameter.ToString();
            var product = await _ProductoRepository.GetById(id);
            Debug.WriteLine(id);

            if (product == null)
            {

                await DisplayAlert("Aviso", "Informacion Invalida", "OK");

            }
            product.key = id;

            await Navigation.PushModalAsync(new PageEditarProducto(product));

        }
    }
}