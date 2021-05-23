using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webprojesi.Models.Entity;

namespace webprojesi.Controllers
{
   [Authorize(Roles ="A")]
    public class MarkalarController : Controller
    {
        // GET: Markalar
        MVCStokTakipEntities db = new MVCStokTakipEntities();
        public ActionResult Index()
        {
            var deger = db.Markalars.ToList();
            return View(deger);
        }

        public void SelecBilgiGetir()
        {
            List<SelectListItem> list = (from x in db.Kategorilers
                                         select new SelectListItem
                                         {
                                             Value = x.ID.ToString(),
                                             Text = x.Kategori
                                         }).ToList();
            ViewBag.l = list;

        }

        public ActionResult YeniMarka()
        {
            SelecBilgiGetir();
            return View();
        }
   
        [HttpPost]
        public ActionResult YeniMarka(Markalar m)
        {
            if (!ModelState.IsValid) {
                ViewBag.KategoriID = new SelectList(db.Kategorilers, "ID", "Kategori", m.KategoriID);
                return View();
            } 
            db.Markalars.Add(m);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GuncelBilgiGetir(int id)
        {
            SelecBilgiGetir();
            var ara = db.Markalars.Find(id);
            return View(ara);
        }

        public ActionResult Guncelle(Markalar m)
        {
            if (!ModelState.IsValid)
            {
                SelecBilgiGetir();
                return View("GuncelBilgiGetir");
            }
            db.Entry(m).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SilBilgiGetir(int id)
        {
            var getir = db.Markalars.Find(id);
            return View(getir);
        }

        public ActionResult Sil(Markalar m)
        {
            db.Entry(m).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}