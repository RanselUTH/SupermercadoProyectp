using Firebase.Database;
using SupermercadoProyectp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SupermercadoProyectp.Views.Cliente
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageClientePedidos : ContentPage
    {
        FirebaseClient firebaseClient;
        string correo;
        public PageClientePedidos()
        {
            InitializeComponent();
            correo = Preferences.Get("userEmail", "");
            firebaseClient = new FirebaseClient("https://proyectogrupo1-default-rtdb.firebaseio.com/");
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            lblTitulo.Text = correo;
            await cargarListaDisponibles();
            btnTomados.BackgroundColor = Color.Gray;
            btnDisponibles.BackgroundColor = Color.Orange;
        }

        private async void btnTomados_Clicked(object sender, EventArgs e)
        {
            await cargarListaAsignados();
            btnDisponibles.BackgroundColor = Color.Gray;
            btnTomados.BackgroundColor = Color.Orange;
        }

        private async void btnDisponibles_Clicked(object sender, EventArgs e)
        {
            await cargarListaDisponibles();
            btnTomados.BackgroundColor = Color.Gray;
            btnDisponibles.BackgroundColor = Color.Orange;
        }


        private async void lsvPedidos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Models.PedidoRepartidor pedido = e.SelectedItem as Models.PedidoRepartidor;

            var detalle = (await firebaseClient.Child("detallepedidos").OnceAsync<DetallePedido>()).Select(d => d.Object).ToList().FindAll(d => d.IdPedido == pedido.IdPedido);
            
            if(pedido.Estado != "ENVIADO")
            {
                await Navigation.PushAsync(new Views.Cliente.PageRatingBar());
            }
       
        }

        private async Task cargarListaDisponibles()
        {
            lsvPedidos.BeginRefresh();

            var pedidos = (await firebaseClient.Child("pedidos").OnceAsync<Pedido>()).Select(p => p.Object).ToList().FindAll(p => p.Estado == "ENVIADO");

            List<Models.PedidoRepartidor> datosLista = new List<Models.PedidoRepartidor>();
            foreach (var pedido in pedidos)
            {
                var cliente = (await firebaseClient.Child("repartidores").OnceAsync<Repartidor>()).Select(c => c.Object).ToList().Find(c => c.IdRepartidor == pedido.IdRepartidor);
                
                datosLista.Add(new Models.PedidoRepartidor()
                {
                    IdPedido = pedido.IdPedido,
                    IdCliente = cliente.IdRepartidor,
                    NombreCliente = cliente.NombreRepartidor,
                    Fecha = pedido.Fecha,
                    Direccion = pedido.Direccion,
                    Latitud = pedido.Latitud,
                    Longitud = pedido.Longitud,
                    Total = pedido.Total,
                    Estado = pedido.Estado
                });
            }

            lsvPedidos.ItemsSource = datosLista;
            lsvPedidos.EndRefresh();
        }



        private async Task cargarListaAsignados()
        {
            lsvPedidos.BeginRefresh();

            
            var pedidos = (await firebaseClient.Child("pedidos").OnceAsync<Pedido>()).Select(p => p.Object).ToList().FindAll(p => p.Estado == "ASIGNADO");

            List<Models.PedidoRepartidor> datosLista = new List<Models.PedidoRepartidor>();
            foreach (var pedido in pedidos)
            {
                var cliente = (await firebaseClient.Child("repartidores").OnceAsync<Repartidor>()).Select(c => c.Object).ToList().Find(c => c.IdRepartidor == pedido.IdRepartidor);

                datosLista.Add(new Models.PedidoRepartidor()
                {
                    IdPedido = pedido.IdPedido,
                    IdCliente = cliente.IdRepartidor,
                    NombreCliente = cliente.NombreRepartidor,
                    Fecha = pedido.Fecha,
                    Direccion = pedido.Direccion,
                    Latitud = pedido.Latitud,
                    Longitud = pedido.Longitud,
                    Total = pedido.Total,
                    Estado = pedido.Estado
                });
            }

            lsvPedidos.ItemsSource = datosLista;
            lsvPedidos.EndRefresh();
        }
    }
}