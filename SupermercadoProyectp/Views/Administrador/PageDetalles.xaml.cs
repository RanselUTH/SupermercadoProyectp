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
    public partial class PageDetalles : ContentPage
    {
        public PageDetalles(Producto producto)
        {
            InitializeComponent();
            LabelName.Text = producto.NombreProducto;
            LabelPrecio.Text = System.Convert.ToString(producto.Precio);
            LabelDescripcion.Text = System.Convert.ToString(producto.Descripcion);
            ProductoImg.Source = ImageSource.FromStream(() => new MemoryStream(System.Convert.FromBase64String(producto.Foto)));

        }
    }
}