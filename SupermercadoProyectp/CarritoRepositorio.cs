using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using SupermercadoProyectp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermercadoProyectp
{
    public class CarritoRepositorio
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://proyectogrupo1-default-rtdb.firebaseio.com/");

        public async Task<int> ObtenerID_repartidor()
        {
            var id = (await firebaseClient
            .Child("carrito")
                            .OnceAsync<Bolsa>()).Select(c => {
                                return c.Object;
                            }).ToList().ToArray().Length;

            return id + 1;
        }
        public async Task<bool> Save(Bolsa oBolsa)
        {
            var data = await firebaseClient.Child(nameof(CarritoRepositorio)).PostAsync(JsonConvert.SerializeObject(oBolsa));
            if (!string.IsNullOrEmpty(data.Key))
            {
                return true;
            }
            return false;
        }

        public async Task<int> ObtenerID_PEDIDO()
        {
            var id = (await firebaseClient
            .Child("pedidos")
                            .OnceAsync<Bolsa>()).Select(c => {
                                return c.Object;
                            }).ToList().ToArray().Length;

            return id + 1;
        }

        public async Task SaveDetalle(DetallePedido details)
        {
            await firebaseClient.Child("detallepedidos").PostAsync<DetallePedido>(details);

        }


        public async Task<bool> AgregarPedido(Pedido pedido)
        {
            FirebaseClient firebaseClient = new FirebaseClient("https://proyectogrupo1-default-rtdb.firebaseio.com/");


            try
            {

                //string apiformat = string.Concat(AppSettings.ApiFirebase, "bolsa/{0}.json?auth={1}");



                var response = await firebaseClient.Child("pedidos").PostAsync<Pedido>(pedido);


                return true;
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                return false;
            }

        }


        public async Task<bool> DeleteLista(string id)
        {
            await firebaseClient.Child("carrito" + "/" + id).DeleteAsync();
            return true;
        }





    }
}
