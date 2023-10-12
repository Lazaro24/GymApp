using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GymApp.Models.Entities
{
    public class entUsuarios
    {
        public string usuario { get; set; }
        public string password { get; set; }
        public string fecha_creacion { get; set; }
        public Int64 id_empleado { get; set; }
        public string nombre_completo { get; set; }
        public Int64 dpi { get; set; }
        public string estado { get; set; }
        public bool encontrado { get; set; }
        public string mensaje { get; set; }
    }

    public class GetUsuarios
    {
        public List<entUsuarios> ListadoUsuarios { get; set; }
    }
}