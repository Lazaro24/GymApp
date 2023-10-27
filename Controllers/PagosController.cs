using GymApp.Models.AppModels;
using GymApp.Models.Entities;
using GymApp.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GymApp.Controllers
{
    [ValidarSesion]
    public class PagosController : Controller
    {
        PagosModel model = new PagosModel();

        private getForma ObtenerForma()
        {
            var _model = new getForma();
            _model.ListaFP = model.FormasPago();
            _model.ListaC = model.Conceptos();
            return _model;
        }

        public ActionResult Index()
        {
            return View(ObtenerForma());
        }

        [HttpGet]
        public ActionResult getCliente(string tipoBusqueda, string valor)
        {
            var _model = model.getCliente(tipoBusqueda, valor);
            return Json(_model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ExtraerCuotas(string id_cliente)
        {
            var _model = model.getCuotas(id_cliente);
            return Json(_model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GuardarPago(string concepto, string id_cliente, string correlativo, string id_forma_pago, string monto, string numero_documento, string autorizacion)
        {
            int vResult = 0;
            vResult = model.GuardarPago(concepto, id_cliente, correlativo, /*Session["usuario"].ToString()*/"ADMIN", id_forma_pago, monto, numero_documento, autorizacion);
            return Json(vResult);
        }
    }
}