using Depo_Stok_Kontrol.Model;
using Depo_Stok_Kontrol.ViewModel;
using System.Windows;

namespace Depo_Stok_Kontrol
{
    public partial class KayitSayfasi : Window
    {
        public KayitSayfasi()
        {
            InitializeComponent();
        }

        private void btnGeri_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void btnKayitOl_Click(object sender, RoutedEventArgs e)
        {
            string kullaniciAdi = clearableTextBoxKullaniciAdi.txtInput.Text;
            string sifre = clearablePasswordBoxSifre.pwInput.Password;
            string sifreTekrar = clearablePasswordBoxSifreTekrar.pwInput.Password;
            string ilkIsim = clearableTextBoxIlkIsim.txtInput.Text;
            string ortaIsim = clearableTextBoxOrtaIsim.txtInput.Text;
            string soyisim = clearableTextBoxSoyisim.txtInput.Text;

            if (string.IsNullOrWhiteSpace(kullaniciAdi) ||
                string.IsNullOrWhiteSpace(sifre) ||
                string.IsNullOrWhiteSpace(sifreTekrar) ||
                string.IsNullOrWhiteSpace(ilkIsim) ||
                string.IsNullOrWhiteSpace(soyisim))
            {
                MessageBox.Show("Lütfen tüm zorunlu alanları doldurun.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (sifre != sifreTekrar)
            {
                MessageBox.Show("Şifreler eşleşmiyor.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            TBLKullaniciModel yeniKullanici = new TBLKullaniciModel
            {
                kullanici_adi = kullaniciAdi,
                sifre = sifre,
                ilk_isim = ilkIsim,
                orta_isim = ortaIsim,
                soyisim = soyisim
            };

            KullaniciViewModel kullaniciViewModel = new KullaniciViewModel();
            if (kullaniciViewModel.RegisterUser(yeniKullanici))
            {
                MessageBox.Show("Kayıt başarıyla tamamlandı!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }

    }
}
