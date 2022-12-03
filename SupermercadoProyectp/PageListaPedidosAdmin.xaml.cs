
using Firebase.Database;
using Firebase.Database.Query;
using SupermercadoProyectp.Models;
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
    public partial class PageListaPedidosAdmin : ContentPage
    {
        FirebaseClient firebaseClient;
        public PageListaPedidosAdmin()
        {
            InitializeComponent();
            firebaseClient = new FirebaseClient("https://proyectogrupo1-default-rtdb.firebaseio.com/");
        }

        protected async override void OnAppearing()
        {
            await cargarLista("PENDIENTE");
           
        }

        private async void btnEntregados_Clicked(object sender, EventArgs e)
        {
            await cargarLista("ENTREGADO");
            btnPendientes.BackgroundColor = Color.Gray;
            btnEntregados.BackgroundColor = Color.Orange;
        }

        private async void btnPendientes_Clicked(object sender, EventArgs e)
        {
            await cargarLista("PENDIENTE");
            btnEntregados.BackgroundColor = Color.Gray;
            btnPendientes.BackgroundColor = Color.Orange;
        }

        private async void lsvPedidos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private async Task cargarLista(string estado)
        {
            lsvPedidos.BeginRefresh();

            var pedidos = (await firebaseClient.Child("pedidos").OnceAsync<Pedido>()).Select(p => p.Object).ToList();

            List<AdminListItem> datosLista = new List<AdminListItem>();
            foreach (var pedido in pedidos)
            {
                if (pedido.Estado == estado)
                {
    
                   var cliente = (await firebaseClient.Child("clientes").OnceAsync<Cliente>()).Select(c => c.Object).ToList().Find(c => c.IdCliente == pedido.IdCliente);
                    
                    var repartidor = (await firebaseClient.Child("repartidores").OnceAsync<Repartidor>()).Select(r => r.Object).ToList().Find(r => r.IdRepartidor == pedido.IdRepartidor);

                   
                    datosLista.Add(new AdminListItem() {
                        IdPedido = pedido.IdPedido, 
                        NombreCliente = cliente.NombreCliente,
                        NombreRepartidor = (estado != "PENDIENTE")? repartidor.NombreRepartidor : "",
                        FechaPedido = pedido.Fecha });
                    
                }

            }

            lsvPedidos.ItemsSource = datosLista;

            lsvPedidos.EndRefresh();
        }

        private void lista_productos_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Administrador.PageListaDeProductosCreados());
        }

        private void crear_repartidor_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Administrador.PageCrearRepartidor());
        }

        private void lista_pedidos_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PageListaPedidosAdmin());
        }

        private void crear_producto_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Administrador.PageCrearProducto());
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
