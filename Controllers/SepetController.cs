using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webprojesi.Models.Entity;

namespace webprojesi.Controllers
{
    public class SepetController : Controller
    {
        // GET: Sepet
        MVCStokTakipEntities db = new MVCStokTakipEntities();
        public ActionResult Index(decimal? Tutar)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var kullanici = db.Kullanicilars.FirstOrDefault(x => x.KullaniciAdi == kullaniciadi);
                var model = db.Sepets.Where(x => x.KullaniciID == kullanici.Id).ToList();
                var kid = db.Sepets.FirstOrDefault(x => x.KullaniciID == kullanici.Id);
                if (model != null)
                {
                    if (kid == null)
                    {
                        ViewBag.mesaj = "Sepetde urun yok!";
                    }
                    else if (kid!=null)
                    {
                        Tutar = db.Sepets.Where(x => x.KullaniciID == kid.KullaniciID).Sum(x => x.ToplamFiyati);
                        ViewBag.mesaj = "Toplam tutar =" + Tutar+"$";
                    }
                    return View(model);
                }

                
            }
            return HttpNotFound();
        }

        public ActionResult SepeteEkle(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var model = db.Kullanicilars.FirstOrDefault(x => x.KullaniciAdi == kullaniciadi);
                var urun = db.Urunlers.Find(id);
                var sepet = db.Sepets.FirstOrDefault(x => x.KullaniciID == model.Id && x.UrunID == id);
                if (model != null)
                {
                    if (sepet != null)
                    {
                        sepet.Miktari++;
                        sepet.ToplamFiyati = urun.SatisFiyati * sepet.Miktari;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    var s = new Sepet
                    {
                        KullaniciID = model.Id,
                        UrunID = urun.ID,
                        Miktari = 1,
                        BirimFiyati = urun.SatisFiyati,
                        ToplamFiyati = urun.SatisFiyati,
                        Tarih = DateTime.Now,
                        Saat=DateTime.Now

                    };
                    db.Entry(s).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            return HttpNotFound();
        }

        public ActionResult TotalCount(int? count)
        {
            if (User.Identity.IsAuthenticated)
            {
                var model = db.Kullanicilars.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);
                count = db.Sepets.Where(x => x.KullaniciID == model.Id).Count();
                ViewBag.count = count;
                if (count == 0)
                {
                    ViewBag.count = "";
                }
                return PartialView();
            }
            return HttpNotFound();
        }

        public ActionResult Artir(int id)
        {
            var model = db.Sepets.Find(id);
            model.Miktari++;
            model.ToplamFiyati = model.BirimFiyati * model.Miktari;
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult Azalt(int id)
        {
            var model = db.Sepets.Find(id);
            if (model.Miktari == 1)
            {
                db.Sepets.Remove(model);
                db.SaveChanges();
            }
            model.Miktari--;
            model.ToplamFiyati = model.BirimFiyati * model.Miktari;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public void DinamikMiktar(int id,decimal miktari)
        {
            var model = db.Sepets.Find(id);
            model.Miktari = miktari;
            model.ToplamFiyati = model.BirimFiyati * model.Miktari;
            db.SaveChanges();
        }
        public ActionResult Sil(int id)
        {
            var model = db.Sepets.Find(id);
            db.Sepets.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult HepsiniSil()
        {
            if (User.Identity.IsAuthenticated)
            {
                var model = db.Kullanicilars.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);
                var sil = db.Sepets.Where(x => x.KullaniciID.Equals(model.Id));
                db.Sepets.RemoveRange(sil);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }

    }
}