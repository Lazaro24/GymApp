using GymApp.Models.AppModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GymApp.Controllers
{
    public class ReportesController : Controller
    {
        ReportesModel model = new ReportesModel();

        public ActionResult Reporte()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ReportePagos(string id_cliente, string concepto, string fecha_inicio, string fecha_final, string tipo_reporte)
        {
            var _model = model.Reporte(id_cliente, concepto, fecha_inicio, fecha_final, tipo_reporte);
            return Json(_model, JsonRequestBehavior.AllowGet);
        }
    }
}