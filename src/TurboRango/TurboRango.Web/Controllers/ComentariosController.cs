using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TurboRango.Dominio;
using TurboRango.Web.Models;

namespace TurboRango.Web.Controllers
{
    public class ComentariosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comentarios
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Comentarios()
        {
            var idRestaurante = Request.Form["IdRestaurante"];
            var nota = Request.Form["Nota"];
            var comentario = Request.Form["Comentario"];
            var dataFeedBack = Request.Form["DataFeedBack"];
            var usuario = Request.Form["Usuario"];
            nota = nota.Replace(",", ".");
            FeedBack feedBack = new FeedBack
            {
                IdRestaurante = Convert.ToInt32(idRestaurante),
                Nota = Convert.ToDouble(nota),
                DataFeedBack = DateTime.Now,
                Comentario = comentario,
                Usuario = usuario
            };
            db.FeedBack.Add(feedBack);
            db.SaveChanges();
            return Json (new { mensagem = "ok", feed = feedBack});
        }
    }
}