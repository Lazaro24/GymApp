using GymApp.Models.AppModels;
using GymApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GymApp.Controllers
{
    public class UsuariosController : Controller
    {
        UsuarioModel pdModel = new UsuarioModel();
        // GET: Productos
        private GetUsuarios ObtenerListado()
        {
            var Model = new GetUsuarios();
            Model.ListadoUsuarios = pdModel.ListadoDatos();
            return Model;
        }
        public ActionResult Index()
        {
            var Model = ObtenerListado();
            return View(Model);
        }

        [HttpPost]
        public ActionResult Nuevo(entUsuarios user)
        {
            int Result = 0;            
            Result = pdModel.InsertarDatos(user);
            return Json(Result);
        }

        [HttpPost]
        public ActionResult Editar(entUsuarios user)
        {
            int Result = 0;
            Result = pdModel.ActualizarDatos(user);
            return Json(Result);
        }

        [HttpPost]
        public ActionResult Eliminar(String usuario)
        {
            int Result = 0;
            Result = pdModel.EliminarDatos(usuario);
            return Json(Result);
        }

        [HttpGet]
        public ActionResult Listado(entUsuarios user)
        {
            var _Model = ObtenerListado();
            return Json(_Model, JsonRequestBehavior.AllowGet);
        }
    }
}