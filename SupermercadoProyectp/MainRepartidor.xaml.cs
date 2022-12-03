using Android.Preferences;
using Firebase.Database;
using SupermercadoProyectp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SupermercadoProyectp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainRepartidor : ContentPage
    {
        FirebaseClient firebaseClient;
        string correo;
        public MainRepartidor()
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
            List<DetalleRepartidor> dr = new List<DetalleRepartidor>();
            foreach (var item in detalle)
            {
                var producto = (await firebaseClient.Child("productos").OnceAsync<Producto>()).Select(p => p.Object).ToList().Find(p => p.IdProducto == item.IdProducto);
                //Debug.WriteLine(producto.NombreProducto);
                dr.Add(new DetalleRepartidor() { NombreProducto = producto.NombreProducto, Cantidad = item.Cantidad, TotalProducto = item.Total });
            }

           await Navigation.PushAsync(new PedidoRepartidor(correo, pedido, dr));
        }

        private async Task cargarListaDisponibles()
        {
            lsvPedidos.BeginRefresh();

            var pedidos = (await firebaseClient.Child("pedidos").OnceAsync<Pedido>()).Select(p => p.Object).ToList().FindAll(p => p.Estado == "PENDIENTE");

            List<Models.PedidoRepartidor> datosLista = new List<Models.PedidoRepartidor>();
            foreach (var pedido in pedidos)
            {
                var cliente = (await firebaseClient.Child("clientes").OnceAsync<Cliente>()).Select(c => c.Object).ToList().Find(c => c.IdCliente == pedido.IdCliente);
                Debug.WriteLine(pedido.IdPedido);
                datosLista.Add(new Models.PedidoRepartidor()
                {
                    IdPedido = pedido.IdPedido,
                    IdCliente = cliente.IdCliente,
                    NombreCliente = cliente.NombreCliente,
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

            var repartidor = (await firebaseClient.Child("repartidores").OnceAsync<Repartidor>()).Select(r => r.Object).ToList().Find(r => r.Correo == correo);
            var pedidos = (await firebaseClient.Child("pedidos").OnceAsync<Pedido>()).Select(p => p.Object).ToList().FindAll(p => p.Estado == "ASIGNADO").FindAll(p => p.IdRepartidor == repartidor.IdRepartidor);

            List<Models.PedidoRepartidor> datosLista = new List<Models.PedidoRepartidor>();
            foreach (var pedido in pedidos)
            {
                var cliente = (await firebaseClient.Child("clientes").OnceAsync<Cliente>()).Select(c => c.Object).ToList().Find(c => c.IdCliente == pedido.IdCliente);

                datosLista.Add(new Models.PedidoRepartidor()
                {
                    IdPedido = pedido.IdPedido,
                    IdCliente = cliente.IdCliente,
                    NombreCliente = cliente.NombreCliente,
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

        private async void tlbcerrar_sesion_Clicked(object sender, EventArgs e)
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
    }
}