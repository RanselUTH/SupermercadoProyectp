using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Storage;
using Newtonsoft.Json;
using SupermercadoProyectp.Models;

namespace SupermercadoProyectp
{
    public class productos
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://proyectogrupo1-default-rtdb.firebaseio.com/");
        FirebaseStorage firebaseStorage = new FirebaseStorage("gs://supermercado-2fab8.appspot.com");
        public async Task<bool> Save(Producto producto)
        {
            var data = await firebaseClient.Child(nameof(productos)).PostAsync(JsonConvert.SerializeObject(producto));
            if (!string.IsNullOrEmpty(data.Key))
            {
                return true;
            }
            return false;
        }

        public async Task<List<Producto>> GetAll()
        {
            return (await firebaseClient.Child("productos").OnceAsync<Producto>()).Select(item => new Producto
            {
                NombreProducto = item.Object.NombreProducto,
                Descripcion = item.Object.Descripcion,
                Precio = item.Object.Precio,
                Cantidad = item.Object.Cantidad,
                Foto = item.Object.Foto,
                IdProducto = item.Object.IdProducto,
                key = item.Key
            }).ToList();

        }

        public async Task<Producto> GetById(string id)
        {
            return (await firebaseClient.Child(nameof(productos) + "/" + id).OnceSingleAsync<Producto>());
        }

        public async Task<int> ObtenerID_producto()
        {
            var id = (await firebaseClient
            .Child("productos")
                            .OnceAsync<Models.Bolsa>()).Select(c => {
                                return c.Object;
                            }).ToList().ToArray().Length;

            return id + 1;
        }

        public async Task<bool> Update(Producto producto)
        {
            await firebaseClient.Child(nameof(productos) + "/" + producto.key).PutAsync(JsonConvert.SerializeObject(producto));
            return true;
        }

        public async Task<bool> Delete(string id)
        {
            await firebaseClient.Child(nameof(productos) + "/" + id).DeleteAsync();
            return true;
        }

        public async Task<string> Upload(Stream img, string fileName)
        {
            var image = await firebaseStorage.Child("Images").Child(fileName).PutAsync(img);
            return image;
        }

        public async Task<List<Producto>> GetAllBy(String name)
        {
            return (await firebaseClient.Child("productos").OnceAsync<Producto>()).Select(item => new Producto
            {
                NombreProducto = item.Object.NombreProducto,
                Descripcion = item.Object.Descripcion,
                Precio = item.Object.Precio,
                Cantidad = item.Object.Cantidad,
                Foto = item.Object.Foto,
                IdProducto = item.Object.IdProducto,
                key = item.Key
            }).Where(c => c.NombreProducto.ToLower().Contains(name)).ToList();
        }




    }
}
