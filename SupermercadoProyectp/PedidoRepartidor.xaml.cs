using Firebase.Database;
using Firebase.Database.Query;
using SupermercadoProyectp.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SupermercadoProyectp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PedidoRepartidor : ContentPage
    {
        private FirebaseClient firebaseClient;
        string correoRepartidor;
        Models.PedidoRepartidor pedido;
        List<DetalleRepartidor> detalle;

        public PedidoRepartidor(string c, Models.PedidoRepartidor p, List<DetalleRepartidor> d)
        {
            InitializeComponent();
            firebaseClient = new FirebaseClient("https://proyectogrupo1-default-rtdb.firebaseio.com/");
            pedido = p;
            detalle = d;
            correoRepartidor = c;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            lblPedido.Text = pedido.IdPedido.ToString();
            lblCliente.Text = pedido.NombreCliente;
            lblDireccion.Text = pedido.Direccion;
            lblFecha.Text = pedido.Fecha;
            lblTotal.Text = pedido.Total.ToString();
            lsvDetalle.ItemsSource = detalle;

            btnTomar.IsEnabled = pedido.Estado == "PENDIENTE";
            btnEntregar.IsEnabled = pedido.Estado == "ASIGNADO";
        }

        private async void btnMapa_Clicked(object sender, EventArgs e)
        {
            var location = new Location(pedido.Latitud, pedido.Longitud);
            var options = new MapLaunchOptions { Name = pedido.NombreCliente, NavigationMode = NavigationMode.Driving };

            try
            {
                await Map.OpenAsync(location, options);
            }
            catch (Exception)
            {
                await DisplayAlert("ERROR", "Hubo un problema al abrir el mapa", "Ok");
            }
        }

        private async void btnTomar_Clicked(object sender, EventArgs e)
        {
            string key = "unassigned";
            var ped = (await firebaseClient.Child("pedidos").OnceAsync<Pedido>()).Select(p =>
            {
                key = p.Key;
                return p.Object;
            }).ToList().Find(p => p.IdPedido == pedido.IdPedido);

            var repartidor = (await firebaseClient.Child("repartidores").OnceAsync<Repartidor>()).Select(r => r.Object).ToList().Find(r => r.Correo == correoRepartidor);

            ped.IdRepartidor = repartidor.IdRepartidor;
           ped.Estado = "ASIGNADO";
            await firebaseClient.Child("pedidos").Child(key).PutAsync<Pedido>(ped);

            pedido.Estado = ped.Estado;
            await DisplayAlert("ASIGNADO", "El pedido a sido asignado correctamente", "Ok");

            btnEntregar.IsEnabled = true;
            btnTomar.IsEnabled = false;
        }

        private async void btnEntregar_Clicked(object sender, EventArgs e)
        {
            string key = "unassigned";
            var ped = (await firebaseClient.Child("pedidos").OnceAsync<Pedido>()).Select(p =>
            {
                key = p.Key;
                return p.Object;
            }).ToList().Find(p => p.IdPedido == pedido.IdPedido);

            ped.Estado = "ENTREGADO";
            await firebaseClient.Child("pedidos").Child(key).PutAsync<Pedido>(ped);

            pedido.Estado = ped.Estado;
            await DisplayAlert("ENTREGADO", "El pedido a sido marcado como entregado", "Ok");

            btnEntregar.IsEnabled = false;
            btnTomar.IsEnabled = false;
        }
    }
}