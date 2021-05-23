//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace webprojesi.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public partial class Urunler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Urunler()
        {
            this.Satislars = new HashSet<Satislar>();
            this.Sepets = new HashSet<Sepet>();
            this.MarkaListesi = new List<SelectListItem>();
            MarkaListesi.Insert(0, new SelectListItem { Text = "Once Kategori Seciniz", Value = "" });
        }

        public int ID { get; set; }
        [Required(ErrorMessage = "Kategori alani bos gecilmez!")]
        public int KategoriID { get; set; }
        [Required(ErrorMessage = "Marka alani bos gecilmez!")]
        public int MarkaID { get; set; }
        [Required(ErrorMessage = "Urun alani bos gecilmez!")]
        public string UrunAdi { get; set; }
        [Required(ErrorMessage = "Barkod alani bos gecilmez!")]
        public string BarkodNo { get; set; }
        [Required(ErrorMessage = "AlisFiyati alani bos gecilmez!")]
        public Nullable<decimal> AlisFiyati { get; set; }
        [Required(ErrorMessage = "StisFiyati alani bos gecilmez!")]
        public decimal SatisFiyati { get; set; }
        [Required(ErrorMessage = "K.D.V. alani bos gecilmez!")]
        public Nullable<int> KDV { get; set; }
        [Required(ErrorMessage = "Birim alani bos gecilmez!")]
        public Nullable<int> BirimID { get; set; }
        [Required(ErrorMessage = "Tarih alani bos gecilmez!")]
        public DateTime Tarih { get; set; }
        [Required(ErrorMessage = "Aciklama alani bos gecilmez!")]
        public string Aciklama { get; set; }
        [Required(ErrorMessage = "Miktari alani bos gecilmez!")]
        public Nullable<decimal> Miktari { get; set; }

        public virtual Birimler Birimler { get; set; }

        public virtual Kategoriler Kategoriler { get; set; }
        public virtual Markalar Markalar { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Satislar> Satislars { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sepet> Sepets { get; set; }
        public List<SelectListItem> KategoriListesi { get; set; }
        public List<SelectListItem> MarkaListesi { get; set; }
        public List<SelectListItem> BirimListesi { get; set; }
    }
}
