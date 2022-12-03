using System;
using System.Collections.Generic;
using System.Text;

namespace SupermercadoProyectp.Models
{
    public class Bolsa
    {
        public string key { get; set; }
        public int IdCliente { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
    }
}
