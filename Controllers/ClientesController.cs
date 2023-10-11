using GymApp.Models.AppModels;
using GymApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GymApp.Controllers
{
    public class ClientesController : Controller
    {
        ClientesModel pdModel = new ClientesModel();
        // GET: Productos
        private GetClientes ObtenerListado()
        {
            var Model = new GetClientes();
            Model.ListadoClientes = pdModel.ListadoDatos();
            return Model;
        }
        public ActionResult Index()
        {
            var Model = ObtenerListado();
            return View(Model);
        }

        [HttpPost]
        public ActionResult Nuevo(EntClientes cliente)
        {
            int Result = 0;
            cliente.Usuario = Session["usuario"] != null ? Session["usuario"].ToString() : "";
            Result = pdModel.InsertarDatos(cliente);
            return Json(Result);
        }

        [HttpPost]
        public ActionResult Editar(EntClientes cliente)
        {
            int Result = 0;
            cliente.Usuario = Session["usuario"] != null ? Session["usuario"].ToString() : "";
            Result = pdModel.ActualizarDatos(cliente);
            return Json(Result);
        }

        [HttpPost]
        public ActionResult Eliminar(String IdCliente, string estado)
        {
            int Result = 0;
            Result = pdModel.EliminarDatos(IdCliente, estado);
            return Json(Result);
        }

        [HttpGet]
        public ActionResult Listado(EntClientes cliente)
        {
            var _Model = ObtenerListado();
            return Json(_Model, JsonRequestBehavior.AllowGet);
        }
    }
}