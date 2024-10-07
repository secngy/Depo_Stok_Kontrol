using Depo_Stok_Kontrol.DTO;
using Depo_Stok_Kontrol.ViewModel;
using System.Windows;

namespace Depo_Stok_Kontrol
{
    public partial class KullaniciBilgileri : Window
    {
        private KullaniciViewModel _kullaniciViewModel = new KullaniciViewModel();
        public KullaniciBilgileri()
        {
            InitializeComponent();
            LoadCurrentUserDetails();
            DataContext = this;
        }

        public string? FullName => KullaniciDTO.Kullanici_Fullname;

        private void LoadCurrentUserDetails()
        {
            

            if (KullaniciDTO.Kullanici_Adi != null)
            {
                txtKullaniciAdi.txtInput.Text = KullaniciDTO.Kullanici_Adi;
                txtIlkIsim.txtInput.Text = KullaniciDTO.Ilk_isim;
                txtOrtaIsim.txtInput.Text = KullaniciDTO.Orta_isim;
                txtSoyisim.txtInput.Text = KullaniciDTO.Soyisim;
            }
            else
            {
                MessageBox.Show("Kullanıcı bilgileri yüklenemedi.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnGuncelle_Click(object sender, RoutedEventArgs e)
        {
            string yeniKullaniciAdi = txtKullaniciAdi.txtInput.Text.Trim();
            string yeniIlkIsim = txtIlkIsim.txtInput.Text.Trim();
            string yeniOrtaIsim = txtOrtaIsim.txtInput.Text.Trim();
            string yeniSoyisim = txtSoyisim.txtInput.Text.Trim();

            KullaniciDTO.Eski_Kullanici_Adi = KullaniciDTO.Kullanici_Adi;
            KullaniciDTO.Kullanici_Adi = yeniKullaniciAdi;
            KullaniciDTO.Ilk_isim = yeniIlkIsim;
            KullaniciDTO.Orta_isim = yeniOrtaIsim;
            KullaniciDTO.Soyisim = yeniSoyisim;
            

            bool updateSuccess = _kullaniciViewModel.UpdateUser();

            if (updateSuccess)
            {
                MessageBox.Show("Bilgiler güncellendi!", "Güncelleme Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                KullaniciDTO.Kullanici_Fullname = KullaniciDTO.Ilk_isim + " " + KullaniciDTO.Orta_isim + " " + KullaniciDTO.Soyisim + " Hosgeldiniz.";
                KullaniciBilgileri kullaniciBilgileri = new KullaniciBilgileri();
                kullaniciBilgileri.Show();
                this.Close();
            }
            
            
        }
        private void btnGeri_Click(object sender, RoutedEventArgs e)
        {
            AnaMenu anaMenu = new AnaMenu();
            anaMenu.Show();
            this.Close();
        }
    }
}
