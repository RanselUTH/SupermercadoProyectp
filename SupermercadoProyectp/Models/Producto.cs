using System;
using System.Collections.Generic;
using System.Text;

namespace SupermercadoProyectp.Models
{
    public class Producto
    {
        public string key { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }

        public decimal Precio { get; set; }

        public int Cantidad { get; set; }

        public string Descripcion { get; set; }

        public string Foto { get; set; }

    }
}