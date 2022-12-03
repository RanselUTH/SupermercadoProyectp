using System;
using System.Collections.Generic;
using System.Text;

namespace SupermercadoProyectp.Models
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public int IdCliente { get; set; }
        public int IdRepartidor { get; set; }
        public string Fecha { get; set; }
        public string Direccion { get; set; }
       
        public float Latitud { get; set; }
        public float Longitud { get; set; }
        public float Subtotal { get; set; }
        public float ISV { get; set; }
        public float Total { get; set; }
        public string Estado { get; set; }
    }
}
