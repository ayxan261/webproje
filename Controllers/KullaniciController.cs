using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webprojesi.Models.Entity;
using System.Web.Security;
using System.Net.Mail;
using System.Net;

namespace webprojesi.Controllers
{
    public class KullaniciController : Controller
    {
        // GET: Kullanici

        MVCStokTakipEntities db = new MVCStokTakipEntities();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Kullanicilar k)
        {
            var kullanici = db.Kullanicilars.FirstOrDefault(x => x.KullaniciAdi == k.KullaniciAdi && x.Sifre == k.Sifre);
            if (kullanici != null)
            {
                FormsAuthentication.SetAuthCookie(k.KullaniciAdi, false);
                return RedirectToAction("Index", "Urunler");
            }
            ViewBag.hata = "Kullanici adi veya sifre yanlis";
            return View();

        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(Kullanicilar k)
        {
            var model = db.Kullanicilars.Where(x => x.Email == k.Email).FirstOrDefault();
            if (model != null)
            {
                Guid rastgele = new Guid();
                model.Sifre = rastgele.ToString().Substring(0, 8);
                db.SaveChanges();
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("ayxan90mv261@gmail.com","Sifre sifirlama");
                mail.To.Add(model.Email);
                mail.IsBodyHtml = true;
                mail.Subject="Sifre degistirme istegi";
                mail.Body = "Merhaba" + model.AdiSoyadi + "<br/> Kullanici adiniz" + model.KullaniciAdi + "<br/> Sifreniz" + model.Sifre;
                NetworkCredential net = new NetworkCredential("ayxan90mv261@gmail.com", "subaru90mv261");
                client.Credentials = net;
                client.Send(mail);
                return RedirectToAction("Login");


            }
            ViewBag.hata = "Boyle bir email yok";
            return View();
        }

        public ActionResult Kaydol()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Kaydol(Kullanicilar k)
        {
            if (!ModelState.IsValid) return View();
            
            db.Entry(k).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();
            return RedirectToAction("Login");


        }

        public ActionResult Guncelle()
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var model = db.Kullanicilars.FirstOrDefault(x => x.KullaniciAdi == kullaniciadi);
                if (model != null)
                {
                    return View(model);

                }
                else
                {
                    return View(new Kullanicilar());
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Guncelle(Kullanicilar k)
        {
            db.Entry(k).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            FormsAuthentication.SignOut();
            FormsAuthentication.SetAuthCookie(k.KullaniciAdi, false);
            return RedirectToAction("Login");
        }

    }
}