using Depo_Stok_Kontrol.DTO;
using Depo_Stok_Kontrol.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Depo_Stok_Kontrol
{
    public partial class DepoSayfasi : Window
    {

        private DepoViewModel depoViewModel;

        public DepoSayfasi()
        {
            InitializeComponent();
            depoViewModel = new DepoViewModel();
            LoadStorages();
            DataContext = this;
        }
        public string? FullName => KullaniciDTO.Kullanici_Fullname;
        private void LoadStorages()
        {
            List<DepoDTO> storages = depoViewModel.GetStorages(txtDepoKoduAra.txtInput.Text, txtDepoIsmiAra.txtInput.Text);
            dtgDepoBilgileri.ItemsSource = storages;
        }

        private void btnGeri_Click(object sender, RoutedEventArgs e)
        {
            AnaMenu anaMenu = new AnaMenu();
            anaMenu.Show();
            this.Close();
        }

        private void btnAra_Click(object sender, RoutedEventArgs e)
        {

            grid1.Visibility = Visibility.Visible;
            grid2.Visibility = Visibility.Visible;
            txtZorunlu.Visibility = Visibility.Visible;
            dockPanel.Visibility = Visibility.Visible;
            LoadStorages();

        }

        private void btnTemizle_Click(object sender, RoutedEventArgs e)
        {
            txtDepoKoduGuncelle.txtInput.Clear();
            txtDepoIsmiGuncelle.txtInput.Clear();
            txtKilitliGuncelle.txtInput.Clear();
            txtEksiBakiyeGuncelle.txtInput.Clear();
            txtAciklamaBirGuncelle.txtInput.Clear();
            txtAciklamaIkiGuncelle.txtInput.Clear();
            txtAciklamaUcGuncelle.txtInput.Clear();
            dtgDepoBilgileri.ItemsSource = null;
        }

        private void btnEkleGuncelle_Click(object sender, RoutedEventArgs e)
        {

            string depoKodu = txtDepoKoduGuncelle.txtInput.Text;
            string depoIsmi = txtDepoIsmiGuncelle.txtInput.Text;
            string kilitli = txtKilitliGuncelle.txtInput.Text.ToUpper();
            string eksiBakiye = txtEksiBakiyeGuncelle.txtInput.Text;
            string dacik1 = txtAciklamaBirGuncelle.txtInput.Text;
            string dacik2 = txtAciklamaIkiGuncelle.txtInput.Text;
            string dacik3 = txtAciklamaUcGuncelle.txtInput.Text;
            string currentUser = KullaniciDTO.Kullanici_Adi;

            if (string.IsNullOrEmpty(depoKodu) || string.IsNullOrEmpty(depoIsmi))
            {
                MessageBox.Show("Lütfen tüm zorunlu alanları doldurun.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DepoDTO depo = new DepoDTO
            {
                depo_kodu = depoKodu,
                depo_ismi = depoIsmi,
                kilitli = string.IsNullOrEmpty(kilitli) ? null : kilitli,
                eksi_bakiye = string.IsNullOrEmpty(eksiBakiye) ? null : eksiBakiye,
                dacik1 = string.IsNullOrEmpty(dacik1) ? null : dacik1,
                dacik2 = string.IsNullOrEmpty(dacik2) ? null : dacik2,
                dacik3 = string.IsNullOrEmpty(dacik3) ? null : dacik3
            };

            bool success = depoViewModel.AddOrUpdateDepo(depo, currentUser);

            if (success)
            {
                LoadStorages();
            }
            
        }

        private void dtgDepoBilgileri_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                txtDepoKoduGuncelle.txtInput.Text = (dtgDepoBilgileri.SelectedCells[0].Column.GetCellContent(dtgDepoBilgileri.SelectedCells[0].Item) as TextBlock)?.Text ?? string.Empty;
                txtDepoIsmiGuncelle.txtInput.Text = (dtgDepoBilgileri.SelectedCells[1].Column.GetCellContent(dtgDepoBilgileri.SelectedCells[1].Item) as TextBlock)?.Text ?? string.Empty;
                txtKilitliGuncelle.txtInput.Text = (dtgDepoBilgileri.SelectedCells[2].Column.GetCellContent(dtgDepoBilgileri.SelectedCells[2].Item) as TextBlock)?.Text ?? string.Empty;
                txtEksiBakiyeGuncelle.txtInput.Text = (dtgDepoBilgileri.SelectedCells[3].Column.GetCellContent(dtgDepoBilgileri.SelectedCells[3].Item) as TextBlock)?.Text ?? string.Empty;
                txtAciklamaBirGuncelle.txtInput.Text = (dtgDepoBilgileri.SelectedCells[4].Column.GetCellContent(dtgDepoBilgileri.SelectedCells[4].Item) as TextBlock)?.Text ?? string.Empty;
                txtAciklamaIkiGuncelle.txtInput.Text = (dtgDepoBilgileri.SelectedCells[5].Column.GetCellContent(dtgDepoBilgileri.SelectedCells[5].Item) as TextBlock)?.Text ?? string.Empty;
                txtAciklamaUcGuncelle.txtInput.Text = (dtgDepoBilgileri.SelectedCells[6].Column.GetCellContent(dtgDepoBilgileri.SelectedCells[6].Item) as TextBlock)?.Text ?? string.Empty;
            }
            catch
            {

            }
        }


    }
}
