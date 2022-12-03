using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Delivery
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PagePedidosUser : ContentPage
    {
        public PagePedidosUser()
        {
            InitializeComponent();
        }

        private void inicio_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Vistas.Menuprincipal.Principal());
        }

       

        private void bolsa_de_compra_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new PageBolsaCompra());
        }

        private void pedidos_Tapped(object sender, EventArgs e)
        {
           Navigation.PushModalAsync(new PagePedidosUser());
        }

        private void salir_Tapped(object sender, EventArgs e)
        {

        }
    }
}