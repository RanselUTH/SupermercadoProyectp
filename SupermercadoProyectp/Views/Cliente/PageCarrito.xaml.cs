
using Delivery;
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
    public partial class PageCarrito : ContentPage
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://proyectogrupo1-default-rtdb.firebaseio.com/");
        carrito car = new carrito();
        CarritoRepositorio carritoRepositorio = new CarritoRepositorio();
        //int IdCliente;
        string correo;
        List<double> latlong;
        decimal subtotal = 0;
        float totalpagar = 0;
        List<Bolsa> carrito;
        public PageCarrito()
        {
            InitializeComponent();

            correo = Preferences.Get("userEmail", "");
            latlong = new List<double>();
        }


        protected override async void OnAppearing()
        {
            await cargarLista();
            if (latlong.Count != 0)
            {
                lblLAT.Text = latlong.First().ToString();
                lblLONG.Text = latlong.Last().ToString();
            }
        }


        private async Task cargarLista()
        {


            lvsCarrito.BeginRefresh();

            var idcliente = (await firebaseClient.Child("clientes").OnceAsync<Models.Cliente>()).Select(p => p.Object).ToList().Find(c => c.Correo == correo).IdCliente;



            carrito = (await firebaseClient.Child("carrito").OnceAsync<Bolsa>()).Select(p =>
            {
                p.Object.key = p.Key.ToString();
                return p.Object;
            }).ToList().FindAll(c => c.IdCliente == idcliente);


            List<ItemBolsa> datosLista = new List<ItemBolsa>();


            subtotal = 0;
            totalpagar = 0;

            foreach (var car in carrito)
            {
                var cliente = (await firebaseClient.Child("productos").OnceAsync<Producto>()).Select(c => c.Object).ToList().Find(c => c.IdProducto == car.IdProducto);



                datosLista.Add(new ItemBolsa()
                {
                    key = car.key.ToString(),
                    nombre = cliente.NombreProducto,
                    cantidad = car.Cantidad,
                    Total = car.Total

                });

                subtotal += car.Total;

            }


            totalpagar = ((float)subtotal) * 1.15f;
            lblSUB.Text = "SUB-TOTAL: " + subtotal.ToString();
            lblTotal.Text = "TOTAL A PAGAR: " + totalpagar.ToString();






            lvsCarrito.ItemsSource = datosLista;

            lvsCarrito.EndRefresh();
        }

        private void lvsCarrito_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private async void lblUbicacion_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PageMapa(latlong));
        }

        private async void DeleteTap_Tapped(object sender, EventArgs e)
        {


            var response = await DisplayAlert("Aviso", "Deseas Borrar Este Producto", "SI", "NO");
            if (response)
            {
                string id = ((TappedEventArgs)e).Parameter.ToString();
                bool is_delete = await car.Delete(id);
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

        private async void lblpagar_Clicked(object sender, EventArgs e)
        {
            if (subtotal == 0 || totalpagar == 0 || string.IsNullOrEmpty(txtDireccion.Text) || string.IsNullOrEmpty(lblLAT.Text) || string.IsNullOrEmpty(lblLONG.Text))
            {
                await DisplayAlert("AVISO", "INFORMACION INVALIDA", "OK");
                return;
            }


            var idcliente = (await firebaseClient.Child("clientes").OnceAsync<Models.Cliente>()).Select(p => p.Object).ToList().Find(c => c.Correo == correo).IdCliente;



            var id = await carritoRepositorio.ObtenerID_PEDIDO();
            Pedido pedido = new Pedido()
            {
                IdPedido = id,
                IdCliente = idcliente,
                Fecha = DateTime.Now.ToString(),
                Direccion = txtDireccion.Text,
                Latitud = (float)System.Convert.ToDouble(lblLAT.Text),
                Longitud = (float)System.Convert.ToDouble(lblLONG.Text),
                Subtotal = (float)System.Convert.ToDouble(subtotal),
                ISV = 0.15f,
                Total = totalpagar,
                Estado = "PENDIENTE"

            };

            if (await carritoRepositorio.AgregarPedido(pedido))
            {
                //  await DisplayAlert("AVISO", "INFORMACION AGREGADA CORRECTAMENTE", "OK");
                List<DetallePedido> lista = new List<DetallePedido>();
                foreach (var item in carrito)
                {
                    var details = new DetallePedido
                    {

                        IdPedido = id,
                        IdProducto = item.IdProducto,
                        Cantidad = item.Cantidad,
                        Total = (float)System.Convert.ToDouble(item.Total)
                    };

                    await carritoRepositorio.SaveDetalle(details);
                    await carritoRepositorio.DeleteLista(item.key);

                }

                await DisplayAlert("AVISO", "INFORMACION AGREGADA CORRECTAMENTE", "OK");
                await cargarLista();


            }









        }
    }
}