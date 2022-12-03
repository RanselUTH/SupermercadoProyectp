using SupermercadoProyectp.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SupermercadoProyectp.Service;
using Xamarin.Essentials;
using SupermercadoProyectp.Models;

namespace SupermercadoProyectp.Views.Cliente
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageDetalleProducto : ContentPage
    {

        public Producto oGlobalProducto;
        CarritoRepositorio CarritoRepositorio = new CarritoRepositorio();
        public PageDetalleProducto(Producto producto)
        {
            InitializeComponent();
            txtNombre.Text = producto.NombreProducto;
            txtPrecio.Text = System.Convert.ToString("Precio: " + producto.Precio + "LPS.");
            txtDescripcion.Text = System.Convert.ToString(producto.Descripcion);
            //txtCantidad.Text = System.Convert.ToString("Cantidad: "+producto.Cantidad);
            ImagenProducto.Source = ImageSource.FromStream(() => new MemoryStream(System.Convert.FromBase64String(producto.Foto)));
            oGlobalProducto = producto;


        }


        private void TapMenos_Tapped(object sender, EventArgs e)
        {
            int cantidad = System.Convert.ToInt32(lblCantidad.Text);
            if (cantidad > 1)
            {
                cantidad -= 1;
            }
            lblCantidad.Text = cantidad.ToString();
        }

        private void TapMas_Tapped(object sender, EventArgs e)
        {
            int cantidad = System.Convert.ToInt32(lblCantidad.Text);
            cantidad += 1;
            lblCantidad.Text = cantidad.ToString();
        }

        private async void btnAgregarBolsa_Clicked(object sender, EventArgs e)
        {
            /*
            bool encontrado = await validarProductoEnBolsa();

            if (encontrado)
            {
                await DisplayAlert("Mensaje", "El producto ya se encuentra en la bolsa", "Ok");
                return;
            }*/

            string email = Preferences.Get("userEmail", "default");
            Bolsa oBolsa = new Bolsa()
            {
                Cantidad = System.Convert.ToInt32(lblCantidad.Text),
                IdCliente = await ApiServiceFirebase.ObtenerIdCliente(email),
                IdProducto = oGlobalProducto.IdProducto,
                Total = System.Convert.ToInt32(lblCantidad.Text) * oGlobalProducto.Precio,
                key = ""

            };
            var isSaves = await ApiServiceFirebase.AgregarenBolsa(oBolsa);


            if (isSaves)
            {
                var DisplayResultado = await DisplayAlert("Mensaje", "Producto agregado a la bolsa", "Ir al carrito", "Seguir comprando");
                if (DisplayResultado)
                {
                    await Navigation.PushAsync(new PageCarrito());
                }

            }
            else
            {
                await DisplayAlert("Mensaje", "No se pudo agregar a la bolsa", "Ok");
            }


        }

        /*
        private async Task<bool> validarProductoEnBolsa()
        {
            bool encontrado = false;
            Dictionary<string, Bolsa> oObjecto = await ApiServiceFirebase.ObtenerBolsa();
            if (oObjecto != null)
            {
                foreach (KeyValuePair<string, Bolsa> item in oObjecto)
                {
                    if (item.Value.producto.Id == oGlobalProducto.Id)
                    {
                        encontrado = true;
                        break;
                    }
                }
            }

            return encontrado;

        }
        */

    }
}