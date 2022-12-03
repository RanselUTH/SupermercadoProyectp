using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using SupermercadoProyectp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermercadoProyectp
{
    public partial class RepartidorRepositorio
    {
        public string Repartidorkey { get; set; }
        static string webapikey = "AIzaSyCpxOv5JIchRBnS9Uk9yd-Am61ubABpPUI";
        FirebaseAuthProvider authProvider;
        public static FirebaseClient firebaseperfil = new FirebaseClient("https://proyectogrupo1-default-rtdb.firebaseio.com/");

        public async Task UpdatePerfil(Repartidor perfil)
        {
            /*var toUpdatePerfil = (await firebaseperfil
                .Child("clientes")
                .OnceAsync<Cliente>()).Where(a => a.Object.Correo == perfil1.Correo).FirstOrDefault();*/
            if (Repartidorkey != null)
            {
                await firebaseperfil
                .Child("repartidores")
                .Child(Repartidorkey)
                .PutAsync(perfil);

            }


        }

       

        public async Task Guardar_Repartidor(Repartidor c)
        {
            await firebaseperfil
                .Child("repartidores")
                .PostAsync(c);
        }
        public async Task<int> ObtenerID_repartidor()
        {
            var id = (await firebaseperfil
            .Child("repartidores")
                            .OnceAsync<Repartidor>()).Select(c => {
                                Repartidorkey = c.Key;
                                return c.Object;
                            }).ToList().ToArray().Length;

            return id + 1;
        }

        public RepartidorRepositorio()
        {
            authProvider = new FirebaseAuthProvider(new FirebaseConfig(webapikey));

        }
    }
}
