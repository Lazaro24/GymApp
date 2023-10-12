using GymApp.Models.AppModels;
using GymApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GymApp.Controllers
{
    public class EmpleadosController : Controller
    {
        EmpleadosModel pdModel = new EmpleadosModel();
        // GET: Productos
        private GetEmpleados ObtenerListado()
        {
            var Model = new GetEmpleados();
            Model.ListadoEmpleados = pdModel.ListadoDatos();
            return Model;
        }
        public ActionResult Index()
        {
            var Model = ObtenerListado();
            return View(Model);
        }

        [HttpPost]
        public ActionResult Nuevo(entEmpleados empleado)
        {
            int Result = 0;
            Result = pdModel.InsertarDatos(empleado);
            return Json(Result);
        }

        [HttpPost]
        public ActionResult Editar(entEmpleados empleado)
        {
            int Result = 0;
            Result = pdModel.ActualizarDatos(empleado);
            return Json(Result);
        }

        [HttpPost]
        public ActionResult Eliminar(String IdEmpleado, string estado)
        {
            int Result = 0;
            Result = pdModel.EliminarDatos(IdEmpleado, estado);
            return Json(Result);
        }

        [HttpGet]
        public ActionResult Listado(entEmpleados empleado)
        {
            var _Model = ObtenerListado();
            return Json(_Model, JsonRequestBehavior.AllowGet);
        }
    }
}