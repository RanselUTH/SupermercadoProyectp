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
    public partial class PerfilRepositorio
    {
        public string Clientekey { get; set; }
        static string webapikey = "AIzaSyCpxOv5JIchRBnS9Uk9yd-Am61ubABpPUI";
        FirebaseAuthProvider authProvider;
        public static FirebaseClient firebaseperfil = new FirebaseClient("https://proyectogrupo1-default-rtdb.firebaseio.com/");

        public async Task UpdatePerfil(Cliente perfil)
        {
            /*var toUpdatePerfil = (await firebaseperfil
                .Child("clientes")
                .OnceAsync<Cliente>()).Where(a => a.Object.Correo == perfil1.Correo).FirstOrDefault();*/
            if (Clientekey != null)
            {
                await firebaseperfil
                .Child("clientes")
                .Child(Clientekey)
                .PutAsync(perfil);

            }


        }

        public async Task<Cliente> ObtenerCliente(string correo)
        {
            /*var cliente = (await firebaseperfil
                            .Child("clientes")
                            .OnceAsync<Cliente>()).Where(a => a.Object.Correo == correo).FirstOrDefault();

            return cliente.Object;*/

            var clientes = (await firebaseperfil
            .Child("clientes")
                            .OnceAsync<Cliente>()).Select(c => {
                                if (c.Object.Correo == correo)
                                {
                                    Clientekey = c.Key;
                                }
                                return c.Object;
                            }).ToList();

            Cliente cl = null;

            foreach (var cliente in clientes)
            {
                if (cliente.Correo == correo)
                {
                    cl = cliente;
                }
            }
            return cl;
        }

        public async Task Guardar_Cliente(Cliente c)
        {
            await firebaseperfil
                .Child("clientes")
                .PostAsync(c);
        }
        public async Task<int> ObtenerID_cliente()
        {
            var id = (await firebaseperfil
            .Child("clientes")
                            .OnceAsync<Cliente>()).Select(c => {
                                Clientekey = c.Key;
                                return c.Object;
                            }).ToList().ToArray().Length;

            return id + 1;
        }

        public PerfilRepositorio()
        {
            authProvider = new FirebaseAuthProvider(new FirebaseConfig(webapikey));

        }
    }
}
