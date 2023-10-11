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
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string estado { get; set; }
        public bool encontrado { get; set; }
        public string mensaje { get; set; }
    }
}