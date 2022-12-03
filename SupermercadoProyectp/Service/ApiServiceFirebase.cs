using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using SupermercadoProyectp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SupermercadoProyectp.Service
{
    public class ApiServiceFirebase
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://supermercado-2fab8-default-rtdb.firebaseio.com");




        public static async Task<bool> AgregarenBolsa(Bolsa oBolsa)
        {
            FirebaseClient firebaseClient = new FirebaseClient("https://proyectogrupo1-default-rtdb.firebaseio.com/");


            try
            {

                //string apiformat = string.Concat(AppSettings.ApiFirebase, "bolsa/{0}.json?auth={1}");
                Bolsa carrito = new Bolsa()
                {

                    IdProducto = oBolsa.IdProducto,
                    IdCliente = oBolsa.IdCliente,
                    Cantidad = oBolsa.Cantidad,
                    Total = oBolsa.Total
                };


                var response = await firebaseClient.Child("carrito").PostAsync<Bolsa>(carrito);


                return true;
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                return false;
            }

        }

        public static async Task<int> ObtenerIdCliente(String C)
        {
            FirebaseClient firebaseClient = new FirebaseClient("https://proyectogrupo1-default-rtdb.firebaseio.com/");

            var usuario = (await firebaseClient.Child("clientes").OnceAsync<Models.Cliente>()).Select(u => u.Object).ToList().Find(u => u.Correo == C);
            return usuario.IdCliente;


        }






    }
}