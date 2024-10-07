using Depo_Stok_Kontrol.DTO;
using Depo_Stok_Kontrol.ViewModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
namespace Depo_Stok_Kontrol
{
    public partial class StokSayfasi : Window
    {

        private StokViewModel stokViewModel;
        public StokSayfasi()
        {
            InitializeComponent();
            stokViewModel = new StokViewModel();
            LoadStocks();
            DataContext = this;
        }
        public string? FullName => KullaniciDTO.Kullanici_Fullname;
        private void LoadStocks()
        {
            List<StokDTO> stocks = stokViewModel.GetStocks(txtStokKoduAra.txtInput.Text, txtStokAdiAra.txtInput.Text);
            dtgStokBilgileri.ItemsSource = stocks;
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
            LoadStocks();
        }


        private void btnTemizle_Click(object sender, RoutedEventArgs e)
        {
            txtStokKoduGuncelle.txtInput.Clear();
            txtStokAdiGuncelle.txtInput.Clear();
            txtKilitliGuncelle.txtInput.Clear();
            txtUreticiKoduGuncelle.txtInput.Clear();
            txtGrupKoduGuncelle.txtInput.Clear();
            txtOlcuBirimiGuncelle.txtInput.Clear();
            txtKDVOraniGuncelle.txtInput.Clear();
            txtKODBirGuncelle.txtInput.Clear();
            txtKODIkiGuncelle.txtInput.Clear();
            txtKODUcGuncelle.txtInput.Clear();
            txtOncekiKodGuncelle.txtInput.Clear();
            txtIngilizceIsimGuncelle.txtInput.Clear();
            dtgStokBilgileri.ItemsSource = null;
        }
        private void btnEkleGuncelle_Click(object sender, RoutedEventArgs e)

        {
            string stokKodu = txtStokKoduGuncelle.txtInput.Text;
            string stokAdi = txtStokAdiGuncelle.txtInput.Text;
            string kilitli = txtKilitliGuncelle.txtInput.Text.ToUpper();
            string ureticiKodu = txtUreticiKoduGuncelle.txtInput.Text;
            string olcuBirimi = txtOlcuBirimiGuncelle.txtInput.Text.ToUpper();
            string kdvOraniText = txtKDVOraniGuncelle.txtInput.Text;
            string grupKodu = txtGrupKoduGuncelle.txtInput.Text.ToUpper();
            string kod1 = txtKODBirGuncelle.txtInput.Text;
            string kod2 = txtKODIkiGuncelle.txtInput.Text;
            string kod3 = txtKODUcGuncelle.txtInput.Text;
            string oncekiKod = txtOncekiKodGuncelle.txtInput.Text;
            string ingilizceIsim = txtIngilizceIsimGuncelle.txtInput.Text;
            string currentUser = KullaniciDTO.Kullanici_Adi;



            if (string.IsNullOrEmpty(stokKodu) || string.IsNullOrEmpty(stokAdi) || string.IsNullOrEmpty(grupKodu))
            {
                MessageBox.Show("Lütfen tüm zorunlu alanları doldurun.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            double? kdvOrani = null;
            if (!string.IsNullOrEmpty(kdvOraniText))
            {
                if (double.TryParse(kdvOraniText, NumberStyles.Any, CultureInfo.InvariantCulture, out double parsedKdvOrani))
                {
                    kdvOrani = parsedKdvOrani;
                }
                else
                {
                    MessageBox.Show("KDV Oranı geçersiz. Lütfen geçerli bir sayı girin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            StokDTO stok = new StokDTO()
            {
                stok_kodu = stokKodu,
                stok_adi = stokAdi,
                kilitli = string.IsNullOrEmpty(kilitli) ? null : kilitli,
                uretici_kodu = string.IsNullOrEmpty(ureticiKodu) ? null : ureticiKodu,
                olcu_birimi = string.IsNullOrEmpty(olcuBirimi) ? null : olcuBirimi,
                kdv_orani = kdvOrani,
                grup_kodu = grupKodu,
                kod1 = string.IsNullOrEmpty(kod1) ? null : kod1,
                kod2 = string.IsNullOrEmpty(kod2) ? null : kod2,
                kod3 = string.IsNullOrEmpty(kod3) ? null : kod3,
                onceki_kod = string.IsNullOrEmpty(oncekiKod) ? null : oncekiKod,
                ingilizce_isim = string.IsNullOrEmpty(ingilizceIsim) ? null : ingilizceIsim
            };

            bool success = stokViewModel.AddOrUpdateStok(stok, currentUser);

            if (success)
            {
                LoadStocks();
            }
        }

        private void dtgStokBilgileri_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                txtStokKoduGuncelle.txtInput.Text = (dtgStokBilgileri.SelectedCells[0].Column.GetCellContent(dtgStokBilgileri.SelectedCells[0].Item) as TextBlock)?.Text ?? string.Empty;
                txtStokAdiGuncelle.txtInput.Text = (dtgStokBilgileri.SelectedCells[1].Column.GetCellContent(dtgStokBilgileri.SelectedCells[1].Item) as TextBlock)?.Text ?? string.Empty;
                txtKilitliGuncelle.txtInput.Text = (dtgStokBilgileri.SelectedCells[2].Column.GetCellContent(dtgStokBilgileri.SelectedCells[2].Item) as TextBlock)?.Text ?? string.Empty;
                txtUreticiKoduGuncelle.txtInput.Text = (dtgStokBilgileri.SelectedCells[3].Column.GetCellContent(dtgStokBilgileri.SelectedCells[3].Item) as TextBlock)?.Text ?? string.Empty;
                txtOlcuBirimiGuncelle.txtInput.Text = (dtgStokBilgileri.SelectedCells[4].Column.GetCellContent(dtgStokBilgileri.SelectedCells[4].Item) as TextBlock)?.Text ?? string.Empty;
                txtKDVOraniGuncelle.txtInput.Text = (dtgStokBilgileri.SelectedCells[5].Column.GetCellContent(dtgStokBilgileri.SelectedCells[5].Item) as TextBlock)?.Text ?? string.Empty;
                txtGrupKoduGuncelle.txtInput.Text = (dtgStokBilgileri.SelectedCells[6].Column.GetCellContent(dtgStokBilgileri.SelectedCells[6].Item) as TextBlock)?.Text ?? string.Empty;
                txtKODBirGuncelle.txtInput.Text = (dtgStokBilgileri.SelectedCells[7].Column.GetCellContent(dtgStokBilgileri.SelectedCells[7].Item) as TextBlock)?.Text ?? string.Empty;
                txtKODIkiGuncelle.txtInput.Text = (dtgStokBilgileri.SelectedCells[8].Column.GetCellContent(dtgStokBilgileri.SelectedCells[8].Item) as TextBlock)?.Text ?? string.Empty;
                txtKODUcGuncelle.txtInput.Text = (dtgStokBilgileri.SelectedCells[9].Column.GetCellContent(dtgStokBilgileri.SelectedCells[9].Item) as TextBlock)?.Text ?? string.Empty;
                txtOncekiKodGuncelle.txtInput.Text = (dtgStokBilgileri.SelectedCells[10].Column.GetCellContent(dtgStokBilgileri.SelectedCells[10].Item) as TextBlock)?.Text ?? string.Empty;
                txtIngilizceIsimGuncelle.txtInput.Text = (dtgStokBilgileri.SelectedCells[11].Column.GetCellContent(dtgStokBilgileri.SelectedCells[11].Item) as TextBlock)?.Text ?? string.Empty;
            }
            catch
            {

            }
        }
    }
}
