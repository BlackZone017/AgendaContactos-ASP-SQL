using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgendaWeb.Models;

namespace AgendaWeb.Controllers
{
    public class HomeController : Controller
    {
        private AgendaEntities db = new AgendaEntities();
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                int uid = (int)Session["UserID"];
                return View(db.Agenda.Where(m => m.idUsuario == uid).ToList());
            }
            return View();
        }

        [HttpPost]
        public ActionResult login(Usuario login)
        {
            if (db.Usuario.Where(n => n.usuario1 == login.usuario1 && n.password == login.password).ToList().Count > 0)
            {
                Usuario s = db.Usuario.Where(n => n.usuario1 == login.usuario1).First();
                Session["UserEmail"] = s.usuario1;
                Session["UserID"] = s.id;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "Usuario y/o Contraseña Incorrecta";
            }
            return View();
        }



        public ActionResult login()
        {
            return View();
        }

        public ActionResult logout()
        {

            Session.Clear();
            Session.Abandon();
            return RedirectToAction("login", "Home");
        }

        public ActionResult registrarse()
        {
            return View();
        }

        [HttpPost]
        public ActionResult registrarse([Bind(Include = "usuario1, password")] Usuario reg)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Usuario.Add(reg);
                    db.SaveChanges();

                    ViewBag.notificacion = "Usuario Registrado";
                    return View("login");
                }
            }
            catch (Exception ex)
            {
                ViewBag.notificacion = "El usuario ya existe";
            }
            //todo

            return View();
        }
    }
}