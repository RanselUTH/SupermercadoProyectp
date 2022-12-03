using System;
using System.Collections.Generic;
using System.Text;

namespace SupermercadoProyectp.Models
{
    public class Repartidor
    {
        public int IdRepartidor { get; set; }
        public string Correo { get; set; }
        public string NombreRepartidor { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public float Latitud { get; set; }
        public float Longitud { get; set; }
        public string Foto { get; set; }
    }
}
