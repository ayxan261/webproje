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
    
    public partial class Satislar
    {
        public int ID { get; set; }
        public Nullable<int> KullaniciID { get; set; }
        public Nullable<int> UrunID { get; set; }
        public Nullable<int> SepetID { get; set; }
        public string BarkoNo { get; set; }
        public Nullable<decimal> BirimFiyati { get; set; }
        public Nullable<decimal> Miktari { get; set; }
        public Nullable<decimal> ToplamFiyati { get; set; }
        public Nullable<int> KDV { get; set; }
        public Nullable<int> BirimID { get; set; }
        public DateTime Tarih { get; set; }
        public DateTime Saat { get; set; }
    
        public virtual Kullanicilar Kullanicilar { get; set; }
        public virtual Urunler Urunler { get; set; }
    }
}
