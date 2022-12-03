using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.ComponentModel;
using Xamarin.Essentials;
using System.Net;
using System.Diagnostics;
using System.Net.Http;
using Plugin.Media.Abstractions;
using Newtonsoft.Json.Linq;
using SupermercadoProyectp.Models;
using Android.Graphics;
//using Android.Graphics;

namespace SupermercadoProyectp.Views.Administrador
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageCrearProducto : ContentPage
    {
        productos productoRepository = new productos();
        public PageCrearProducto()
        {
            InitializeComponent();
        }
        Plugin.Media.Abstractions.MediaFile photo = null;
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
                    string Base64String = System.Convert.ToBase64String(imagenescalada);
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

        private async void Buttonregistro_Clicked(object sender, EventArgs e)
        {
            string nombreproducto = txtnombreproducto.Text;
            int cantidadproducto = System.Convert.ToInt32(txtcantidad.Text);
            decimal precio = System.Convert.ToDecimal(txtprecio.Text);
            string descripcion = txtdescription.Text;

            if (String.IsNullOrEmpty(nombreproducto))
            {
                await DisplayAlert("Aviso", "Escribe Un Nombre Para Tu Producto", "OK");
                return;
            }
            if (String.IsNullOrEmpty(System.Convert.ToString(cantidadproducto)))
            {
                await DisplayAlert("Aviso", "Escribe Una Cantidad Para Tu Producto", "OK");
                return;
            }
            if (String.IsNullOrEmpty(System.Convert.ToString(precio)))
            {
                await DisplayAlert("Aviso", "Escribe Un Precio Para Tu Producto", "OK");
                return;
            }
            if (String.IsNullOrEmpty(descripcion))
            {
                await DisplayAlert("Aviso", "Escribe Una Descripcion Para Tu Producto", "OK");
                return;
            }

            Producto producto = new Producto();
            producto.NombreProducto = nombreproducto;
            producto.Cantidad = cantidadproducto;
            producto.Precio = precio;
            producto.Descripcion = descripcion;
            producto.Foto = traeImagenToBase64();
            producto.IdProducto = await productoRepository.ObtenerID_producto();


            var isSaves = await productoRepository.Save(producto);
            if (isSaves)
            {
                await DisplayAlert("Aviso", "Producto Agregado", "OK");
            }
            else
            {
                await DisplayAlert("Aviso", "Producto NO Agregado", "OK");
            }






        }

        private async void ImagTap_Tapped(object sender, EventArgs e)
        {
            /*
            photo = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "FotosAplicacion",
                Name = "PhotoAlbum.jpg",
                SaveToAlbum = true
            });
            if (photo != null)
            {
                ProductoImg.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
            }
            */

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
                ProductoImg.Source = ImageSource.FromStream(() =>
                {
                    var stream = photo.GetStream();
                    return stream;
                });
            }

        }

        private async void lista_productos_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageListaDeProductosCreados());
        }

        private async void crear_repartidor_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageCrearRepartidor());
        }

        private async void lista_pedidos_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageListaPedidosAdmin());
        }

        private async void crear_producto_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageCrearProducto());
        }

       
    }
}