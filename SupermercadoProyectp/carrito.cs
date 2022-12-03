using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SupermercadoProyectp
{
    public class carrito
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://proyectogrupo1-default-rtdb.firebaseio.com/");

        public async Task<bool> Delete(string id)
        {
            await firebaseClient.Child(nameof(carrito) + "/" + id).DeleteAsync();
            return true;
        }


    }
}
