using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webprojesi.Models.Entity;

namespace webprojesi.Controllers
{
    public class SatislarController : Controller
    {
        // GET: Satislar
        MVCStokTakipEntities db = new MVCStokTakipEntities();
        public ActionResult Index()
        {
            var model = db.Satislars.ToList();
            return View(model);
        }

        public ActionResult SatinAl(int id)
        {
            var model = db.Sepets.FirstOrDefault(x => x.ID == id);
            return View(model);
        }
        [HttpPost]
        public ActionResult SatinAl2(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = db.Sepets.FirstOrDefault(x => x.ID == id);
                    var urun = db.Urunlers.FirstOrDefault(x => x.ID == model.UrunID);
                    urun.Miktari = urun.Miktari - model.Miktari;

                    var satis = new Satislar
                    {
                        KullaniciID = model.KullaniciID,
                        UrunID = model.UrunID,
                        SepetID = model.ID,
                        BarkoNo = model.Urunler.BarkodNo,
                        BirimFiyati = model.BirimFiyati,
                        Miktari = model.Miktari,
                        ToplamFiyati = model.ToplamFiyati,
                        KDV = model.Urunler.KDV,
                        BirimID = model.Urunler.BirimID,
                        Tarih = DateTime.Now,
                        Saat = DateTime.Now
                    };
                    db.Sepets.Remove(model);
                    db.Satislars.Add(satis);
                    db.SaveChanges();
                    ViewBag.islem = "Satin alma islemi basarili bir sekilde gerceklesdi!";
                }
                
                
            }
            catch (Exception)
            {

                ViewBag.islem = "Satin alma islemi basarisiz!";
            }
            
            
                return View("islem");
            
        }
        public ActionResult HepsiniSatinAl(decimal? Tutar)
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
                        ViewBag.mesaj = "Sepetde urun bulunmuyor!";
                    }
                    else
                    {
                        Tutar = db.Sepets.Where(x => x.KullaniciID == kid.KullaniciID).Sum(x => x.ToplamFiyati);
                        ViewBag.mesaj = "Tutat=" + Tutar + "$";

                    }

                }
                return View(model);
            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult HepsiniSatinAl2()
        {
            var username = User.Identity.Name;
            var kullanici = db.Kullanicilars.FirstOrDefault(x => x.KullaniciAdi == username);
            var model = db.Sepets.Where(x => x.KullaniciID.Equals(kullanici.Id)).ToList();
            int row = 0;
            foreach (var item in model)
            {
                var satis = new Satislar
                {
                    KullaniciID = model[row].KullaniciID,
                    UrunID = model[row].UrunID,
                    SepetID = model[row].ID,
                    BarkoNo = model[row].Urunler.BarkodNo,
                    BirimFiyati = model[row].BirimFiyati,
                    Miktari = model[row].Miktari,
                    ToplamFiyati = model[row].ToplamFiyati,
                    KDV = model[row].Urunler.KDV,
                    BirimID = model[row].Urunler.BirimID,
                    Tarih = DateTime.Now,
                    Saat = DateTime.Now
                };
                db.Satislars.Add(satis);
                row++;
            }

            foreach (var item in model)
            {
                var urun = db.Urunlers.FirstOrDefault(x => x.ID == item.UrunID);
                if (urun != null)
                {
                    urun.Miktari = urun.Miktari - item.Miktari;
                }
            }
        
            db.Sepets.RemoveRange(model);
            db.SaveChanges();
            return RedirectToAction("Index", "Sepet");
        
        }
    }
}