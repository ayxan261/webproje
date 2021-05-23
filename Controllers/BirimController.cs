using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webprojesi.Models.Entity;

namespace webprojesi.Controllers
{
    [Authorize(Roles ="A")]
    public class BirimController : Controller
    {
        // GET: Birim
        MVCStokTakipEntities db = new MVCStokTakipEntities();
        public ActionResult Index()
        {
            var deger = db.Birimlers.ToList();
            return View(deger);
        }

        public ActionResult YeniBirim()
        {
            return View();
        }
        [HttpPost]
        public ActionResult YeniBirim(Birimler b)
        {
            if (!ModelState.IsValid) return View("YeniBirim");
            db.Birimlers.Add(b);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GuncelBilgiGetir(int id)
        {
            var deger = db.Birimlers.Find(id);
            return View("GuncelBilgiGetir", deger);
        }

        public ActionResult Guncelle(Birimler b)
        {
            var birim = db.Birimlers.Find(b.ID);
            birim.Birim = b.Birim;
            birim.Aciklama = b.Aciklama;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SilBilgiGetir(Birimler b)
        {
            var model = db.Birimlers.Find(b.ID);
            if (model == null) return HttpNotFound();
            return View(model);

        }
        public ActionResult Sil(Birimler b)
        {
            db.Entry(b).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}