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

        public ActionResult Recuperacion()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Recuperacion(string usuario, string clave, string clave_confirm)
        {
            UsuarioModel model = new UsuarioModel();
            string Result = "";

            if (!clave.Equals(clave_confirm))
            {
                string vMensaje = "Las contraseñas ingresadas no coinciden";
                setMessage("ERROR", usuario, clave, clave_confirm, vMensaje);

                return RedirectToAction("Recuperacion", "Login");
            }

            Result = model.RecuperaPassword(usuario, clave);
            string[] vResult = Result.Split('|');

            setMessage(vResult[0], null, null, null, vResult[1]);

            if (vResult[0].Equals("OK"))
            {
                // Salir de la sesion para inicializar una nueva
                Salir();
            }
            return RedirectToAction("Recuperacion", "Login");
        }

        public ActionResult Salir()
        {
            // Limpiar valores temporales
            TempData["vLogin"] = null;
            TempData["vUser"] = null;
            TempData["vClave"] = null;
            // Quitar datos de sesion
            Session["usuario"] = null;

            return RedirectToAction("Ingreso", "Login");
        }

        public void setMessage(string result, string usuario, string clave, string clave_confirm, string vMensaje)
        {
            TempData["vReset"] = result;
            TempData["vUser"] = usuario;
            TempData["vPass"] = clave;
            TempData["vPassC"] = clave_confirm;
            TempData["vMessage"] = vMensaje;
        }
    }
}