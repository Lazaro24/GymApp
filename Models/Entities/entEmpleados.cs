using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GymApp.Models.Entities
{
    public class entEmpleados
    {
        public Int64 IdEmpleado { get; set; }
        public string Nombre1 { get; set; }
        public string Nombre2 { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string Apellido3 { get; set; }
        public Int64 Dpi { get; set; }
        public Int64 Nit { get; set; }
        public Int64 Telefono { get; set; }                
        public string Direccion { get; set; }
        public string FechaCreacion { get; set; }
        public string FechaBaja { get; set; }
        public string Genero { get; set; }
        public string Correo { get; set; }
        public string Estado { get; set; }
    }

    public class GetEmpleados
    {
        public List<entEmpleados> ListadoEmpleados { get; set; }
    }
}