using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webprojesi.Models.Entity;
using Excel = Microsoft.Office.Interop.Excel;

namespace webprojesi.Controllers
{
    [Authorize]
    public class UrunlerController : Controller
    {
        // GET: Urunler
        MVCStokTakipEntities db = new MVCStokTakipEntities();
        public ActionResult Index(string ara)
        {
            var model = db.Urunlers.ToList();
            if (!string.IsNullOrEmpty(ara))
            {
                model = db.Urunlers.Where(x => x.UrunAdi.Contains(ara) || x.BarkodNo.Contains(ara)).ToList();
            }
            return View(model);
        }

        private void Yenile(Urunler model)
        {
            //burada model.birimlistesi,kategorilistesi urunler.cs faylindan alinib. evvel orda yaratmaq lazim.
            List<Kategoriler> kat = db.Kategorilers.OrderBy(x => x.Kategori).ToList();
            model.KategoriListesi = (from x in kat
                                     select new SelectListItem
                                     {
                                         Text = x.Kategori,
                                         Value = x.ID.ToString()

                                     }).ToList();

            List<Birimler> birim = db.Birimlers.OrderBy(x => x.Birim).ToList();
            model.BirimListesi = (from x in birim
                                  select new SelectListItem
                                  {
                                      Text = x.Birim,
                                      Value = x.ID.ToString()

                                  }).ToList();
        }
        public ActionResult YeniUrun()
        {
            var model = new Urunler();
            Yenile(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult YeniUrun(Urunler u)
        {
            if (!ModelState.IsValid)
            {
                var model = new Urunler();
                Yenile(model);
                return View(model);
            }
            db.Entry(u).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult MiktarEkle(int id)
        {
            var model = db.Urunlers.Find(id);
            return View();
        }
        [HttpPost]
        public ActionResult MiktarEkle(Urunler p)
        {
            var model = db.Urunlers.Find(p.ID);
            model.Miktari = model.Miktari + p.Miktari;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetMarka(int id)
        {
            //Marka kategoriye gore listelenir
            var model = new Urunler();
            List<Markalar> mark = db.Markalars.Where(x => x.KategoriID == id).OrderBy(x => x.Marka).ToList();
            model.MarkaListesi = (from x in mark
                                  select new SelectListItem
                                  {
                                      Text = x.Marka,
                                      Value = x.ID.ToString()

                                  }).ToList();
            model.MarkaListesi.Insert(0, new SelectListItem {Text="seciniz",Value="" });
            return Json(model.MarkaListesi, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuncelBilgiGetir(int id)
        {
            var model = db.Urunlers.Find(id);
            Yenile(model);
            List<Markalar> markalist = db.Markalars.Where(x => x.KategoriID == model.KategoriID).OrderBy(x => x.Marka).ToList();
            model.MarkaListesi = (from x in markalist
                                  select new SelectListItem
                                  {
                                      Value = x.ID.ToString(),
                                      Text=x.Marka

                                  }).ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult Guncelle(Urunler p)
        {
            if (!ModelState.IsValid)
            {
                var model = db.Urunlers.Find(p.ID);
                Yenile(model);
                List<Markalar> markalist = db.Markalars.Where(x => x.KategoriID == model.KategoriID).OrderBy(x => x.Marka).ToList();
                model.MarkaListesi = (from x in markalist
                                      select new SelectListItem
                                      {
                                          Value = x.ID.ToString(),
                                          Text = x.Marka

                                      }).ToList();

            }
            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Sil(int id)
        {
            var model = db.Urunlers.FirstOrDefault(x => x.ID == id);
            db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult ExcelExport()
        {
            try
            {
                var liste = db.Urunlers.ToList();
                Excel.Application application = new Excel.Application();
                Excel.Workbook workbook = application.Workbooks.Add(System.Reflection.Missing.Value);
                Excel.Worksheet worksheet = workbook.ActiveSheet;
                worksheet.Cells[1, 1] = "ID";
                worksheet.Cells[1, 2] = "Urun adi";
                worksheet.Cells[1, 3] = "Barkod No";
                worksheet.Cells[1, 4] = "Fiyati";
                worksheet.Cells[1, 5] = "Miktari";
                worksheet.Cells[1, 6] = "Tarihi";
                int row = 2;
                foreach (var item in liste)
                {
                    worksheet.Cells[row, 1] = item.ID;
                    worksheet.Cells[row, 2] = item.UrunAdi;
                    worksheet.Cells[row, 3] = item.BarkodNo;
                    worksheet.Cells[row, 4] = item.SatisFiyati;
                    worksheet.Cells[row, 5] = item.Miktari;
                    worksheet.Cells[row, 6] = item.Tarih;
                    row++;
                    
                }
                var sum = db.Urunlers.Sum(x => x.SatisFiyati).ToString("#,###,###.00");
                var range_sum = worksheet.get_Range("D" + row);
                range_sum.Value2 = "Total="+sum;
                range_sum.Font.Bold = true;          
                workbook.SaveAs("d:\\test.xlsx");
                workbook.Close();
                application.Quit();
                ViewBag.mesaj = "Islem basarili!";
            }
            catch (Exception ex)
            {

                ViewBag.mesaj = ex.Message;
            }
            return Json(ViewBag.mesaj, JsonRequestBehavior.AllowGet);
        } 


    }
}