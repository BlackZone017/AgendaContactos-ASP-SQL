using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using AgendaWeb.Models;

namespace AgendaWeb.Controllers
{
    public class AgendaController : Controller
    {
        private AgendaEntities db = new AgendaEntities();

        public ActionResult enviarMensaje(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agenda agenda = db.Agenda.Find(id);
            if (agenda == null)
            {
                return HttpNotFound();
            }
            return View(agenda);
        }

        public ActionResult Correo()
        {
            ViewBag.enviado="Se ha enviado el correo exitosamente";
            var agenda = db.Agenda.Include(a => a.Usuario);
            int uid = (int)Session["UserID"];

            agenda = agenda.Where(a => a.idUsuario == uid);

            return View("Index",agenda.ToList());
        }

        // GET: Agenda
        public ActionResult Index()
        {
            var agenda = db.Agenda.Include(a => a.Usuario);
            return View(agenda.ToList());
        }

        [HttpPost]
        public ActionResult Index(string nombreContacto)
        {
            var contactos = from tabla in db.Agenda select tabla;
            int uid = (int)Session["UserID"];
            if (!String.IsNullOrEmpty(nombreContacto))
            {
                contactos = contactos.Where(tabla => tabla.nombreContacto.Contains(nombreContacto) && tabla.idUsuario==uid);
            }
            else
            {
                return RedirectToAction("verAgenda");
            }

            return View(contactos.ToList());
        }

        public ActionResult verAgenda(/*int? mid*/)
        {
            if (Session["UserID"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            int uid = (int)Session["UserID"];
            var agenda = db.Agenda.Where(a => a.idUsuario == uid /*&& a.Materia_ID == mid*/).ToList();
            return View("Index", agenda);
        }

        // GET: Agenda/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agenda agenda = db.Agenda.Find(id);
            if (agenda == null)
            {
                return HttpNotFound();
            }
            return View(agenda);
        }

        // GET: Agenda/Create
        public ActionResult Create()
        {
            ViewBag.idUsuario = new SelectList(db.Usuario, "id", "usuario1");
            return View();
        }

        // POST: Agenda/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idUsuario,nombreContacto,telefono,email,direccion")] Agenda agenda)
        {
            if (ModelState.IsValid)
            {
                db.Agenda.Add(agenda);
                db.SaveChanges();
                return RedirectToAction("verAgenda");
            }

            ViewBag.idUsuario = new SelectList(db.Usuario, "id", "usuario1", agenda.idUsuario);
            return View(agenda);
        }

        // GET: Agenda/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserID"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            int uid = (int)Session["UserID"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agenda agenda = db.Agenda.Find(id);
            if (agenda == null)
            {
                return HttpNotFound();
            }

            List<SelectListItem> lst = new List<SelectListItem>();

            List<Agenda> m = db.Agenda.Where(d => d.idUsuario == uid).ToList();

            foreach (var s in m)
            {
                lst.Add(new SelectListItem() { Text = s.nombreContacto, Value = s.id.ToString() });
            }
            ViewBag.materias = lst;

            ViewBag.idUsuario = new SelectList(db.Usuario, "id", "usuario1", agenda.idUsuario);
            return View(agenda);
        }

        // POST: Agenda/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idUsuario,nombreContacto,telefono,email,direccion")] Agenda agenda)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agenda).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("verAgenda");
            }
            ViewBag.idUsuario = new SelectList(db.Usuario, "id", "usuario1", agenda.idUsuario);
            return View(agenda);
        }

        // GET: Agenda/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agenda agenda = db.Agenda.Find(id);
            if (agenda == null)
            {
                return HttpNotFound();
            }
            return View(agenda);
        }

        // POST: Agenda/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Agenda agenda = db.Agenda.Find(id);
            db.Agenda.Remove(agenda);
            db.SaveChanges();
            return RedirectToAction("verAgenda");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
