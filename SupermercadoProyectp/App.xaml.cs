using Delivery.Vistas.Menuprincipal;
using SupermercadoProyectp.Views;
using SupermercadoProyectp.Views.Administrador;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SupermercadoProyectp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new PageLogin());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
