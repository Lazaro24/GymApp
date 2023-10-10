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
            Model.ListadoClientes = pdModel.CargarClientes();
            return Model;
        }
        public ActionResult Index()
        {
            var Model = ObtenerListado();
            return View(Model);
        }

        [HttpPost]
        public ActionResult GuardarNuevoCliente(String Nombre, String Apellido, String Telefono, String Nit, String Sexo, String Email)
        {
            int Result = 0;
            Result = pdModel.CrearCliente(Nombre, Apellido, Telefono, Nit, Sexo, Email);
            return Json(Result);
        }

        [HttpGet]
        public ActionResult ActualizarListadoClientes()
        {
            var _Model = ObtenerListado();
            return Json(_Model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EliminarCliente(String Codigo)
        {
            int Result = 0;
            Result = pdModel.EliminarCliente(Codigo);
            return Json(Result);
        }

        [HttpPost]
        public ActionResult EditarCliente(String Codigo, String Nombre, String Apellido, int Telefono, String Nit, String Sexo, String Email)
        {
            int Result = 0;
            Result = pdModel.ActualizarCliente(Codigo, Nombre, Apellido, Telefono, Nit, Sexo, Email);
            return Json(Result);
        }
    }
}