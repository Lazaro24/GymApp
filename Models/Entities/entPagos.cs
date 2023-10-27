using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GymApp.Models.Entities
{
    public class entPagos
    {
        public Int64 correlativo { get; set; }
        public string id_cliente { get; set; }
        public string nombre_completo { get; set; }
        public Int64 dpi { get; set; }
        public Int64 nit { get; set; }
        public Int64 telefono { get; set; }
        public string fecha_emision { get; set; }
        public string id_concepto { get; set; }
        public string concepto { get; set; }
        public decimal monto { get; set; }
        public string fecha_pago { get; set; }
        public string usuario { get; set; }
        public string fecha_anulacion { get; set; }
        public Int64 id_estado_pago { get; set; }
    }

    public class entFormasPago
    {
        public string id_forma_pago { get; set; }
        public string descripcion { get; set; }
    }

    public class entConcepto
    {
        public string id_concepto { get; set; }
        public string descripcion { get; set; }
        public decimal monto { get; set; }
    }

    // retorno listado de datos para ventana de pagos
    public class getForma
    {
        public List<entPagos> listadoPP { get; set; }
        public List<entFormasPago> ListaFP { get; set; }
        public List<entConcepto> ListaC { get; set; }
    }
}