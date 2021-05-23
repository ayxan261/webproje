using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webprojesi.Models.Entity;

namespace webprojesi.Controllers
{
    [Authorize(Roles ="A")]
    public class KategorilerController : Controller
    {
        // GET: Kategoriler
        MVCStokTakipEntities db = new MVCStokTakipEntities();
        public ActionResult Index()
        {
            var kategori = db.Kategorilers.ToList();
            return View(kategori);
        }

        public ActionResult YeniKategori()
        {
            return View();
        }
        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult YeniKategori(Kategoriler k)
        {
            if (!ModelState.IsValid) return View("YeniKategori");
            db.Kategorilers.Add(k);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GuncelBilgiGetir(int id)
        {
            var deger = db.Kategorilers.Find(id);
            return View("GuncelBilgiGetir", deger);
        }

        public ActionResult Guncelle(Kategoriler k)
        {
            var kat = db.Kategorilers.Find(k.ID);
            kat.Kategori = k.Kategori;
            kat.Aciklama = k.Aciklama;
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        public ActionResult SilBilgiGetir(Kategoriler k)
        {
            var model = db.Kategorilers.Find(k.ID);
            if (model == null) return HttpNotFound();
            return View(model);

        }
        public ActionResult Sil(Kategoriler k)
        {
            db.Entry(k).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Markalar(int id)
        {
            var model = db.Markalars.Where(x => x.KategoriID==id).ToList();
            var kategor = db.Kategorilers.Where(x => x.ID == id).Select(x => x.Kategori).FirstOrDefault();
            ViewBag.kategor = kategor;
            return View(model);
        }

        public ActionResult Urunler(int id)
        {
            var model = db.Urunlers.Where(x => x.KategoriID == id).ToList();
            var kategor = db.Kategorilers.Where(x => x.ID == id).Select(x => x.Kategori).FirstOrDefault();
            ViewBag.kategor = kategor;
            return View(model);
        }
    }
}

