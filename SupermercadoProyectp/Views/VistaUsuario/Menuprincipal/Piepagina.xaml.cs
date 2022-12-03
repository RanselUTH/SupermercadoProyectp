using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Diagnostics;
using Xamarin.Essentials;
using System.Threading.Tasks;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
//using Android.Views;
//using Android.App;

namespace Delivery.Vistas.Menuprincipal
  {
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class Piepagina : ContentView
    {
    public Piepagina()
      {
            InitializeComponent();
      }

       
        private void inicio_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Menuprincipal.Principal());
        }

        private  void pedidos_Tapped(object sender, EventArgs e)
        {
          Navigation.PushModalAsync(new PagePedidosUser());
        }

        private void bolsa_de_compra_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new PageBolsaCompra());
        }

        private async void salir_Tapped(object sender, EventArgs e)
        {
        }

    }
  }