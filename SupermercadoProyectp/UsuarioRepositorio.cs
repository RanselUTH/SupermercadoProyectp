using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Newtonsoft.Json;
using Firebase.Storage;
using System.IO;
using System.Linq;

namespace SupermercadoProyectp
{
    public class UsuarioRepositorio
    {
       static string webapikey = "AIzaSyCpxOv5JIchRBnS9Uk9yd-Am61ubABpPUI"; 
        FirebaseAuthProvider authProvider;



        FirebaseClient firebaseClient = new FirebaseClient("https://proyectogrupo1-default-rtdb.firebaseio.com/");
        FirebaseStorage firebaseStorage = new FirebaseStorage("gs://supermercado-2fab8.appspot.com");

        public async Task<bool> Save(Models.Usuario user)
        {
            var data = await firebaseClient.Child("usuarios").PostAsync(JsonConvert.SerializeObject(user));
            if (!string.IsNullOrEmpty(data.Key))
            {
                return true;
            }
            return false;
        }


        public UsuarioRepositorio()
        {
            authProvider = new FirebaseAuthProvider(new FirebaseConfig(webapikey));

        }

        public async Task<bool> Register(string email, string nombre,string clave)
        {
            var token = await authProvider.CreateUserWithEmailAndPasswordAsync(email, clave, nombre);
       if(!string.IsNullOrEmpty(token.FirebaseToken))
            {
                return true;
            }
        return false;
        
        }

        public async Task<string> SignIn(string email, string clave)
        {
            var token = await authProvider.SignInWithEmailAndPasswordAsync(email, clave);
            if (!string.IsNullOrEmpty(token.FirebaseToken))
            {
                return token.FirebaseToken;
            }

            return "";

        }

        public async Task<string> Getrole(string c)
        {
            var usuario = (await firebaseClient.Child("usuarios").OnceAsync<Models.Usuario>()).Select(u => u.Object).ToList().Find(u => u.Correo==c);
            return usuario.Rol;
        }
    
    public async Task<bool>Resetpassword(string email)
        {
            await authProvider.SendPasswordResetEmailAsync(email);
            return true;
        }


    
    
    
    }
}
