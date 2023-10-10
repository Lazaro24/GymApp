using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GymApp.Models.Entities
{
    public class EntClientes
    {
        public string IdCliente { get; set; }
        public string Nombre1 { get; set; }
        public string Nombre2 { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string Apellido3 { get; set; }
        public int Nit { get; set; }
        public int Telefono { get; set; }
        public Int64 Dpi { get; set; }
        public string FechaCreacion { get; set; }
        public string Direccion { get; set; }
        public string Genero { get; set; }
        public string Estado { get; set; }
        public string Usuario { get; set; }
        public string Email { get; set; }
    }

    public class GetClientes
    {
        public List<EntClientes> ListadoClientes { get; set; }
    }
}