using GymApp.Models.Entities;
using GymApp.Models.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GymApp.Controllers
{
    //[ValidarSesion] Se descomenta cuando halla login
    public class HomeController : Controller
    {
        HomeProc home = new HomeProc();

        private GetDashhboard ListadoHomes()
        {
            var Model = new GetDashhboard();
            Model.Dashboard = home.CargarDashboard();
            return Model;
        }

        public ActionResult Index()
        {
            var Model = ListadoHomes();
            return View(Model);
        }

        /*
        public ActionResult Logout()
        {
            Session["Usuario"] = null;
            Session["Email"] = null;
            Session["TipoUsuario"] = null;

            return RedirectToAction("Acceso", "Login");
        }
        */
    }
}