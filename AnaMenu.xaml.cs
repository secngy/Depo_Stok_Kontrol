using Depo_Stok_Kontrol.DTO;
using System.Windows;

namespace Depo_Stok_Kontrol
{
    public partial class AnaMenu : Window
    {

        public AnaMenu()
        {
            InitializeComponent();
            DataContext = this;
        }

        public string? FullName => KullaniciDTO.Kullanici_Fullname;

        private void btnCikis_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void btnKullaniciBilgileri_Click(object sender, RoutedEventArgs e)
        {
            KullaniciBilgileri kullaniciBilgileri = new KullaniciBilgileri();
            kullaniciBilgileri.Show();
            this.Close();
        }

        private void btnDepo_Click(object sender, RoutedEventArgs e)
        {
            DepoSayfasi depoSayfasi = new DepoSayfasi();
            depoSayfasi.Show();
            this.Close();
        }

        private void btnStok_Click(object sender, RoutedEventArgs e)
        {
            StokSayfasi stokSayfasi = new StokSayfasi();
            stokSayfasi.Show();
            this.Close();
        }
    }
}
