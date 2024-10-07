using Depo_Stok_Kontrol.DTO;
using Depo_Stok_Kontrol.Model;
using Depo_Stok_Kontrol.ViewModel;
using System.Windows;

namespace Depo_Stok_Kontrol
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnKayitOl_Click(object sender, RoutedEventArgs e)
        {
            KayitSayfasi kayitSayfasi = new KayitSayfasi();
            kayitSayfasi.Show();
            this.Close();

        }

        private void btnGirisYap_Click(object sender, RoutedEventArgs e)
        {
            string kullaniciAdi = txtKullaniciAdi.txtInput.Text;
            string sifre = pwSifre.pwInput.Password;

            if (string.IsNullOrWhiteSpace(kullaniciAdi) || string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Lütfen kullanıcı adı ve şifre girin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            KullaniciViewModel kullaniciViewModel = new KullaniciViewModel();
            if (kullaniciViewModel.CheckUserCredentials(kullaniciAdi, sifre))
            {
                TBLKullaniciModel Giren_kullanici = kullaniciViewModel.GetUserDetails(kullaniciAdi);
                KullaniciDTO.Kullanici_Adi = Giren_kullanici.kullanici_adi;
                KullaniciDTO.Ilk_isim = Giren_kullanici.ilk_isim;
                KullaniciDTO.Orta_isim = Giren_kullanici.orta_isim;
                KullaniciDTO.Soyisim = Giren_kullanici.soyisim;
                KullaniciDTO.Kullanici_Fullname = KullaniciDTO.Ilk_isim + " " + KullaniciDTO.Orta_isim + " " + KullaniciDTO.Soyisim + " Hosgeldiniz.";


                AnaMenu anaMenu = new AnaMenu();
                anaMenu.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre hatalı.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}