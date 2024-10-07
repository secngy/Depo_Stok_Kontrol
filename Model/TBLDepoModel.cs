namespace Depo_Stok_Kontrol.Model
{
    class TBLDepoModel
    {
        public string? depo_kodu { get; set; }
        public string? depo_ismi { get; set; }
        public string? kilitli { get; set; }
        public string? eksi_bakiye { get; set; }
        public string? dacik1 { get; set; }
        public string? dacik2 { get; set; }
        public string? dacik3 { get; set; }
        public DateTime? kayit_tarihi { get; set; }
        public string? kayit_yapan { get; set; }
        public DateTime? duzeltme_tarihi { get; set; }
        public string? duzeltme_yapan { get; set; }
    }
}
