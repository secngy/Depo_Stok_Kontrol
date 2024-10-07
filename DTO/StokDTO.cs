namespace Depo_Stok_Kontrol.DTO
{
    class StokDTO
    {
        public string? stok_kodu { get; set; }
        public string? uretici_kodu { get; set; }
        public string? stok_adi { get; set; }
        public string? grup_kodu { get; set; }
        public string? olcu_birimi { get; set; }
        public double? kdv_orani { get; set; }
        public string? kilitli { get; set; }
        public string? kod1 { get; set; }
        public string? kod2 { get; set; }
        public string? kod3 { get; set; }
        public string? onceki_kod { get; set; }
        public string? ingilizce_isim { get; set; }
        public DateTime? kayit_tarihi { get; set; }
        public string? kayit_yapan { get; set; }
        public DateTime? duzeltme_tarihi { get; set; }
        public string? duzeltme_yapan { get; set; }
    }
}
