using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TurboRango.Dominio;
using TurboRango.Web.Models;

namespace TurboRango.Web.Controllers
{
    [Authorize]
    public class RestaurantesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Restaurantes
        public ActionResult Index()
        {
            return View(db.Restaurantes.ToList());
        }

        // GET: Restaurantes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurante restaurante = db.Restaurantes
                .Include(_ => _.Localizacao).Include(_ => _.Contato).FirstOrDefault(x => x.Id == id);
            ViewBag.RestauranteLatitude = restaurante.Localizacao.Latitude;
            ViewBag.RestauranteLongitude = restaurante.Localizacao.Longitude;
            ViewBag.UsuarioLogado = User.Identity.Name; //Recupera e-mail(nome) do usuario logado
            ViewBag.DataFeedBack = DateTime.Now.ToString("dd/MM/yyyy");
            if (restaurante == null)
            {
                return HttpNotFound();
            }
            return View(restaurante);
        }

        // GET: Restaurantes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Restaurantes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,Capacidade,Categoria,Contato,Localizacao")] Restaurante restaurante)
        {
            // Solução ruim, mas necessária (não entendo o que está acontecendo):
            //ModelState.Remove("Contato.Id");
            //ModelState.Remove("Localizacao.Id");
            if (ModelState.IsValid)
            {
                db.Restaurantes.Add(restaurante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(restaurante);
        }

        // GET: Restaurantes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurante restaurante = db.Restaurantes.Include(x => x.Localizacao).Include(x => x.Contato).FirstOrDefault(x => x.Id == id);
            if (restaurante == null)
            {
                return HttpNotFound();
            }
            return View(restaurante);
        }

        // POST: Restaurantes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Capacidade,Categoria,Contato,Localizacao")] Restaurante restaurante)
        {
            if (ModelState.IsValid)
            {
                if (restaurante.Contato.Id != null) db.Entry(restaurante.Contato).State = EntityState.Modified;
                else if (restaurante.Contato != null) db.Contatos.Add(restaurante.Contato);
                if (restaurante.Localizacao.Id != null) db.Entry(restaurante.Localizacao).State = EntityState.Modified;
                else if (restaurante.Localizacao != null) db.Localizacoes.Add(restaurante.Localizacao);
                db.Entry(restaurante).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(restaurante);
        }

        // GET: Restaurantes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurante restaurante = db.Restaurantes.Find(id);
            if (restaurante == null)
            {
                return HttpNotFound();
            }
            return View(restaurante);
        }

        // POST: Restaurantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Restaurante restaurante = db.Restaurantes.Find(id);
            if (restaurante.Contato != null) db.Entry(restaurante.Contato).State = EntityState.Deleted;
            if (restaurante.Localizacao != null) db.Entry(restaurante.Localizacao).State = EntityState.Deleted;
            db.Restaurantes.Remove(restaurante);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public JsonResult Restaurantes()
        {
            var todos = db.Restaurantes.Include(_ => _.Localizacao).ToList();

            return Json(new { restaurantes = todos}, JsonRequestBehavior.AllowGet);
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
