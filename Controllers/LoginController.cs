using GymApp.Models;
using GymApp.Models.AppModels;
using GymApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GymApp.Controllers
{
    public class LoginController : Controller
    {        
        Log log = new Log();
        // GET: Login
        public ActionResult Ingreso()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Ingreso(entUsuarios datos)
        {
            try
            {
                entUsuarios datosLogin = new entUsuarios();
                UsuarioModel login = new UsuarioModel();

                var user = string.IsNullOrEmpty(datos.usuario);
                var pwd = string.IsNullOrEmpty(datos.password);

                datosLogin = login.Loggeo(datos);

                TempData["Loggeo"] = datosLogin.mensaje;
                TempData["valUser"] = datos.usuario;
                TempData["valPass"] = datos.password;
                if (!datosLogin.encontrado)
                {
                    return RedirectToAction("Ingreso", "Login");
                }

                Session["Usuario"] = datosLogin.usuario;

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                log.AddToLog(ex);
                return View();
            }            
        }


    }
}