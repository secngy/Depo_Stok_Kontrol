using Depo_Stok_Kontrol.DTO;
using Depo_Stok_Kontrol.Model;
using MySql.Data.MySqlClient;
using System.Windows;

namespace Depo_Stok_Kontrol.ViewModel
{
    public class KullaniciViewModel
    {
        private string connectionString = "Server=127.0.0.1;Database=depo_stok_kontrol;Uid=root;Pwd='password';";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public bool DoesUsernameExist(string? kullaniciAdi)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(1) FROM tblkullanici WHERE kullanici_adi=@kullaniciAdi";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);

                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public TBLKullaniciModel GetUserDetails(string kullaniciAdi)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT kullanici_adi, ilk_isim, orta_isim, soyisim FROM tblkullanici WHERE kullanici_adi = @kullaniciAdi";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new TBLKullaniciModel
                        {
                            kullanici_adi = reader["kullanici_adi"].ToString(),
                            ilk_isim = reader["ilk_isim"].ToString(),
                            orta_isim = reader["orta_isim"].ToString(),
                            soyisim = reader["soyisim"].ToString()
                        };
                    }
                }
            }

            return null;
        }

        public bool UpdateUser()
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                if (!KullaniciDTO.Kullanici_Adi.Equals(KullaniciDTO.Eski_Kullanici_Adi, StringComparison.OrdinalIgnoreCase))
                {
                    if (DoesUsernameExist(KullaniciDTO.Kullanici_Adi))
                    {
                        MessageBox.Show("Bu kullanıcı adı zaten mevcut.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }

                string query = "UPDATE tblkullanici SET kullanici_adi = @kullaniciAdi, ilk_isim = @ilkIsim, orta_isim = @ortaIsim, soyisim = @soyisim WHERE kullanici_adi = @oldKullaniciAdi";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@kullaniciAdi", KullaniciDTO.Kullanici_Adi);
                cmd.Parameters.AddWithValue("@ilkIsim", KullaniciDTO.Ilk_isim);
                cmd.Parameters.AddWithValue("@ortaIsim", KullaniciDTO.Orta_isim);
                cmd.Parameters.AddWithValue("@soyisim", KullaniciDTO.Soyisim);
                cmd.Parameters.AddWithValue("@oldKullaniciAdi", KullaniciDTO.Eski_Kullanici_Adi);

                if (string.IsNullOrEmpty(KullaniciDTO.Kullanici_Adi))
                {
                    MessageBox.Show("Kullanıcı adı boş olamaz!", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                if (cmd.ExecuteNonQuery() > 0)
                {
                    return true;
                }
                return false;
            }
        }


        public bool CheckUserCredentials(string username, string password)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(1) FROM tblkullanici WHERE kullanici_adi=@kullaniciAdi AND sifre=@sifre";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@kullaniciAdi", username);
                cmd.Parameters.AddWithValue("@sifre", password);

                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public bool RegisterUser(TBLKullaniciModel user)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                if (DoesUsernameExist(user.kullanici_adi))
                {
                    MessageBox.Show("Bu kullanıcı adı zaten mevcut.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                string query = "INSERT INTO tblkullanici (kullanici_adi, sifre, ilk_isim, orta_isim, soyisim) VALUES (@kullaniciAdi, @sifre, @ilkIsim, @ortaIsim, @soyisim)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@kullaniciAdi", user.kullanici_adi);
                cmd.Parameters.AddWithValue("@sifre", user.sifre);
                cmd.Parameters.AddWithValue("@ilkIsim", user.ilk_isim);
                cmd.Parameters.AddWithValue("@ortaIsim", user.orta_isim);
                cmd.Parameters.AddWithValue("@soyisim", user.soyisim);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
