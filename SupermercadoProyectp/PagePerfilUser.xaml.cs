using Android.Graphics;
using Plugin.Media;
using SupermercadoProyectp;
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

namespace Delivery
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PagePerfilUser : ContentPage
    {
        public Cliente perfil = new Cliente();
        PerfilRepositorio _perfilRepositorio = new PerfilRepositorio();
        Plugin.Media.Abstractions.MediaFile photo = null;
        public PagePerfilUser()
        {
            InitializeComponent();
            txtemail.Text = Preferences.Get("userEmail", "default");
            setData();

        }

        public async void setData()
        {
            perfil = await _perfilRepositorio.ObtenerCliente(txtemail.Text);
            if (perfil != null)
            {
                Foto.Source = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(perfil.Foto)));
                txtnombre.Text = perfil.NombreCliente;
                txtdireccion.Text = perfil.Direccion;
                txttelefono.Text = perfil.Telefono;
               
            }
        }


        private String traeImagenToBase64()
        {
            if (photo != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    Stream stream = photo.GetStream();
                    stream.CopyTo(memory);
                    byte[] fotobyte = memory.ToArray();

                    byte[] imagenescalada = obtener_imagen_escalada(fotobyte, 50);

                    // string Base64String = Convert.ToBase64String(fotobyte);
                    string Base64String = Convert.ToBase64String(imagenescalada);
                    return Base64String;
                }
            }
            return null;

        }

        private byte[] obtener_imagen_escalada(byte[] imagen, int compresion)
        {
            Bitmap original = BitmapFactory.DecodeByteArray(imagen, 0, imagen.Length);
            using (MemoryStream memory = new MemoryStream())
            {
                original.Compress(Bitmap.CompressFormat.Jpeg, compresion, memory);
                return memory.ToArray();
            }

        }

        private async void btnFoto_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                "Elige Una Opcion",
                "Cancelar",
                null,
                "Galeria",
                "Camara");

            if (source == "Cancelar")
            {
                photo = null;
                return;
            }
            if (source == "Camara")
            {
                photo = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "FotosAplicacion",
                    Name = "PhotoAlbum.jpg",
                    SaveToAlbum = true
                });
            }
            else
            {
                photo = await CrossMedia.Current.PickPhotoAsync();
            }

            if (photo != null)
            {
                Foto.Source = ImageSource.FromStream(() =>
                {
                    var stream = photo.GetStream();
                    return stream;
                });
            }
        }

        public async void UpdateMethod()
        {
            Cliente p;
            if (perfil != null)
            {
                p = new Cliente
                {
                    NombreCliente = txtnombre.Text,
                    Telefono = txttelefono.Text,
                    Direccion = txtdireccion.Text,
                    Correo = perfil.Correo,
                    IdCliente = perfil.IdCliente,
                    Foto = traeImagenToBase64()
                };

                await _perfilRepositorio.UpdatePerfil(p);
                await DisplayAlert("Info", "Datos actualizados con exito", "Ok");
                return;
            }
            p = new Cliente
            {
                NombreCliente = txtnombre.Text,
                Telefono = txttelefono.Text,
                Direccion = txtdireccion.Text,
                Correo = txtemail.Text,
                Foto = traeImagenToBase64(),
                IdCliente = await _perfilRepositorio.ObtenerID_cliente()
            };

            await _perfilRepositorio.Guardar_Cliente(p);
            await DisplayAlert("Info", "Datos guardados con exito", "Ok");

        }
        private async void Buttoactualizar_Clicked(object sender, EventArgs e)
        {
            if (txtdireccion.Text.Equals(""))
            {
                await DisplayAlert("Info", "Porfavor llenar el campo direccion", "Ok");
            }
            else if (txtemail.Text.Equals(""))
            {
                await DisplayAlert("Info", "Porfavor llenar el campo email", "Ok");
            }
            else if (txtnombre.Text.Equals(""))
            {
                await DisplayAlert("Info", "Porfavor llenar el campo nombre", "Ok");
            }
            else if (txttelefono.Text.Equals(""))
            {
                await DisplayAlert("Info", "Porfavor llenar el campo telefono", "Ok");
            }
            else if (traeImagenToBase64() == null)
            {
                await DisplayAlert("Info", "Porfavor tomar la fotografia", "Ok");
            }
            else
            {
                UpdateMethod();
            }
        }
    }
}