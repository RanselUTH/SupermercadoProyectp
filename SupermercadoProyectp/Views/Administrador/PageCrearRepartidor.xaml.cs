using SupermercadoProyectp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SupermercadoProyectp.Views.Administrador
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageCrearRepartidor : ContentPage
    {
        UsuarioRepositorio _usuarioRepositorio = new UsuarioRepositorio();
        RepartidorRepositorio _repartidorRepositorio = new RepartidorRepositorio();
        public PageCrearRepartidor()
        {
            InitializeComponent();
        }

        private async void Buttonregistro_Clicked(object sender, EventArgs e)
        {

            try
            {

                string nombre = txtnombree.Text;
                string email = txtemaile.Text;
                string clave = txtclavee.Text;
                string confirclave = txtconfirmclavee.Text;
                string telefono = txttelefono.Text;
                string direccion = txtdireccion.Text;


                if (String.IsNullOrEmpty(nombre))
                {
                    await DisplayAlert("Aviso", "Escribe Un Nombre", "OK");
                    return;
                }
                if (String.IsNullOrEmpty(email))
                {
                    await DisplayAlert("Aviso", "Escribe Un Email", "OK");
                    return;
                }
                if (clave.Length < 6)
                {
                    await DisplayAlert("Aviso", "La contraseña tiene que contener al menos de 6 dígitos", "OK");
                    return;
                }
                if (String.IsNullOrEmpty(clave))
                {
                    await DisplayAlert("Aviso", "Escribe Una Clave", "OK");
                    return;
                }
                if (String.IsNullOrEmpty(telefono))
                {
                    await DisplayAlert("Aviso", "Escribe Un Telefono", "OK");
                    return;
                }
                if (String.IsNullOrEmpty(direccion))
                {
                    await DisplayAlert("Aviso", "Escribe Una Dirección", "OK");
                    return;
                }
                if (String.IsNullOrEmpty(confirclave))
                {
                    await DisplayAlert("Aviso", "Confirma Contraseña", "OK");
                    return;
                }
                if (clave != confirclave)
                {
                    await DisplayAlert("Aviso", "Contraseña No Coincide", "OK");
                    return;
                }



                bool IsSaved = await _usuarioRepositorio.Register(email, nombre, clave);
                if (IsSaved)
                {

                    Models.Usuario user = new Models.Usuario();
                    user.Name = nombre;
                    user.Correo = email;
                    user.Clave = clave;
                    user.Rol = "REPARTIDOR";
                    user.Estado = "ACTIVO";
                    var isSaves = await _usuarioRepositorio.Save(user);

                    Repartidor r = new Repartidor()
                    {
                        Correo = email,
                        IdRepartidor = await _repartidorRepositorio.ObtenerID_repartidor(),
                        NombreRepartidor = nombre,
                        Direccion = direccion,
                        Telefono = telefono
                    };
                    await _repartidorRepositorio.Guardar_Repartidor(r);

                    if (isSaves)

                    {
                        await DisplayAlert("Aviso", "Registro Completo", "OK");
                        await Navigation.PopModalAsync();
                    }
                }
                else
                {
                    await DisplayAlert("Aviso", "Registro Fallo", "OK");
                }



            }
            catch (Exception exception)
            {
                if (exception.Message.Contains("EMAIL_EXISTS"))
                {
                    await DisplayAlert("Aviso", "Correo Ya Existe", "OK");
                }
                else
                {
                   // await DisplayAlert("Aviso", exception.Message, "OK");
                }
            }

        }

        private void lista_productos_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PageListaDeProductosCreados());
        }

        private void crear_repartidor_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PageCrearRepartidor());
        }

        private void lista_pedidos_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PageListaPedidosAdmin());
        }

        private void crear_producto_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PageCrearProducto());
        }

       
    }
}