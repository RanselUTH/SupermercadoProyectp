using System;
using System.Collections.Generic;
using System.Text;

namespace SupermercadoProyectp.Models
{
    public class PedidoRepartidor
    {
        public int IdPedido { get; set; }
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public string Direccion { get; set; }
        public float Latitud { get; set; }
        public float Longitud { get; set; }
        public string Fecha { get; set; }
        public float Total { get; set; }
        public string Estado { get; set; }
    }
}
