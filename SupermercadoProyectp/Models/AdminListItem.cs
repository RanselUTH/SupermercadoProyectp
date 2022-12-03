using System;
using System.Collections.Generic;
using System.Text;

namespace SupermercadoProyectp.Models
{
    public  class AdminListItem
    {
        public int IdPedido { get; set; }
        public string NombreCliente { get; set; }
        public string NombreRepartidor { get; set; }
        public string FechaPedido { get; set; }
    }
}
