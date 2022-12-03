using Plugin.Media;
using Plugin.Media.Abstractions;
using SupermercadoProyectp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace SupermercadoProyectp.Views.Administrador
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageEditarProducto : ContentPage
    {
        MediaFile file;
        productos productoRepository = new productos();
        Producto p;
        public PageEditarProducto(Producto producto)
        {
            InitializeComponent();
            txtnombreproducto.Text = producto.NombreProducto;
            txtprecio.Text = System.Convert.ToString(producto.Precio);
            txtcantidad.Text = System.Convert.ToString(producto.Cantidad);
            txtdescription.Text = System.Convert.ToString(producto.Descripcion);
            txtid.Text = producto.key;
            p = producto;
            ProductoImg.Source = ImageSource.FromStream(() => new MemoryStream(System.Convert.FromBase64String(producto.Foto)));
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

                    string Base64String = System.Convert.ToBase64String(fotobyte);
                    return Base64String;
                }
            }
            return null;

        }
        private async void ButtonEdit_Clicked(object sender, EventArgs e)
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
            producto.key = txtid.Text;
            producto.NombreProducto = nombreproducto;
            producto.Cantidad = cantidadproducto;
            producto.Precio = precio;
            producto.Descripcion = descripcion;
            producto.Foto = traeImagenToBase64();
            producto.IdProducto = p.IdProducto;
            bool isUpdated = await productoRepository.Update(producto);
            if (isUpdated)
            {
                await Navigation.PopModalAsync();

            }
            else
            {
                await DisplayAlert("Aviso", "Errror Al Actualizar Datos", "OK");
            }

            /*
            var isSaves = await productoRepository.Save(producto);
            if (isSaves)
            {
                await DisplayAlert("Aviso", "Producto Agregado", "OK");
            }
            else
            {

                await DisplayAlert("Aviso", "Producto NO Agregado", "OK");

            }
*/


        }

        private async void ImagTap_Tapped(object sender, EventArgs e)
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
                ProductoImg.Source = ImageSource.FromStream(() =>
                {
                    var stream = photo.GetStream();
                    return stream;
                });
            }


        }
    }
}