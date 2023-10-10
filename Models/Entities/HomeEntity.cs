using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GymApp.Models.Entities
{
    public class HomeEntity
    {
        public int Pagos { get; set; }
        public int Clientes { get; set; }
        public int PagosPendientes { get; set; }
    }

    public class GetDashhboard
    {
        public List<HomeEntity> Dashboard { get; set; }
    }
}