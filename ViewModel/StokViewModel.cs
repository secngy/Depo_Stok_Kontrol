using Depo_Stok_Kontrol.DTO;
using MySql.Data.MySqlClient;
using System.Windows;

namespace Depo_Stok_Kontrol.ViewModel;

class StokViewModel
{
    private static string _connectionString = "Server=127.0.0.1;Database=depo_stok_kontrol;Uid=root;Pwd='password';";

    public List<StokDTO> GetStocks(string stokKodu, string stokAdi)
    {
        List<StokDTO>? stocks = new List<StokDTO>();

        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();

            string query = "SELECT * FROM tblstok s INNER JOIN tblstokek e ON s.stok_kodu = e.stok_kodu WHERE 1=1";

            if (!string.IsNullOrEmpty(stokKodu))
            {
                query += " AND s.stok_kodu LIKE @stok_kodu";
            }
            if (!string.IsNullOrEmpty(stokAdi))
            {
                query += " AND s.stok_adi LIKE @stok_adi";
            }

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                if (!string.IsNullOrEmpty(stokKodu))
                {
                    command.Parameters.AddWithValue("@stok_kodu", "%" + stokKodu + "%");
                }
                if (!string.IsNullOrEmpty(stokAdi))
                {
                    command.Parameters.AddWithValue("@stok_adi", "%" + stokAdi + "%");
                }
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        StokDTO stok = new StokDTO()
                        {
                            stok_kodu = reader.GetString("stok_kodu"),
                            stok_adi = reader.GetString("stok_adi"),
                            uretici_kodu = reader.IsDBNull(reader.GetOrdinal("uretici_kodu")) ? null : reader.GetString("uretici_kodu"),
                            grup_kodu = reader.GetString("grup_kodu"),
                            olcu_birimi = reader.IsDBNull(reader.GetOrdinal("olcu_birimi")) ? null : reader.GetString("olcu_birimi"),
                            kdv_orani = reader.IsDBNull(reader.GetOrdinal("kdv_orani")) ? (Double?)null : reader.GetDouble("kdv_orani"),
                            kilitli = reader.IsDBNull(reader.GetOrdinal("kilitli")) ? null : reader.GetString("kilitli"),
                            kod1 = reader.IsDBNull(reader.GetOrdinal("kod1")) ? null : reader.GetString("kod1"),
                            kod2 = reader.IsDBNull(reader.GetOrdinal("kod2")) ? null : reader.GetString("kod2"),
                            kod3 = reader.IsDBNull(reader.GetOrdinal("kod3")) ? null : reader.GetString("kod3"),
                            onceki_kod = reader.IsDBNull(reader.GetOrdinal("onceki_kod")) ? null : reader.GetString("onceki_kod"),
                            ingilizce_isim = reader.IsDBNull(reader.GetOrdinal("ingilizce_isim")) ? null : reader.GetString("ingilizce_isim"),
                            kayit_tarihi = reader.GetDateTime("kayit_tarihi"),
                            kayit_yapan = reader.GetString("kayit_yapan"),
                            duzeltme_tarihi = reader.IsDBNull(reader.GetOrdinal("duzeltme_tarihi")) ? (DateTime?)null : reader.GetDateTime("duzeltme_tarihi"),
                            duzeltme_yapan = reader.IsDBNull(reader.GetOrdinal("duzeltme_yapan")) ? null : reader.GetString("duzeltme_yapan")
                        };
                        stocks.Add(stok);
                    }
                }
            }
        }
        return stocks;
    }

    public bool AddOrUpdateStok(StokDTO stok, string currentUser)
    {
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();

            string checkQuery = "SELECT COUNT(*) FROM tblstok s " +
                                "INNER JOIN tblstokek e ON s.stok_kodu = e.stok_kodu " +
                                "WHERE s.stok_kodu = @stok_kodu";
            using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection))
            {
                checkCommand.Parameters.AddWithValue("@stok_kodu", stok.stok_kodu);
                int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (stok.kilitli != null && !(stok.kilitli.Equals("e", StringComparison.OrdinalIgnoreCase) ||
                                stok.kilitli.Equals("h", StringComparison.OrdinalIgnoreCase)))
                        {
                            MessageBox.Show("Kilitli alanı sadece E/H değerleri alabilir.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }

                        if (stok.grup_kodu != null && !(stok.grup_kodu.Equals("hm", StringComparison.OrdinalIgnoreCase) ||
                            stok.grup_kodu.Equals("ym", StringComparison.OrdinalIgnoreCase) || stok.grup_kodu.Equals("m", StringComparison.OrdinalIgnoreCase)))
                        {
                            MessageBox.Show("Grup Kodu alanı sadece HM/YM/M değerleri alabilir.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }
                        if (stok.kdv_orani != null && !double.TryParse(stok.kdv_orani.ToString(), out double kdvOrani))
                        {
                            MessageBox.Show("KDV Oranı geçerli bir sayı olmalıdır (Ör. 17.04).", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }

                if (count > 0)
                {
                    MessageBoxResult result = MessageBox.Show("Böyle bir stok zaten mevcut. Güncellemek ister misiniz?",
                            "Güncelleme Onayı", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        

                        string updateTblStokQuery = @"UPDATE tblstok 
                              SET stok_adi = @stok_adi, 
                                  uretici_kodu = @uretici_kodu, 
                                  grup_kodu = @grup_kodu, 
                                  olcu_birimi = @olcu_birimi, 
                                  kdv_orani = @kdv_orani, 
                                  kilitli = @kilitli 
                              WHERE stok_kodu = @stok_kodu";

                        string updateTblStokEkQuery = @"UPDATE tblstokek 
                                SET kod1 = @kod1, 
                                    kod2 = @kod2, 
                                    kod3 = @kod3, 
                                    onceki_kod = @onceki_kod, 
                                    ingilizce_isim = @ingilizce_isim, 
                                    duzeltme_tarihi = NOW(), 
                                    duzeltme_yapan = @duzeltme_yapan 
                                WHERE stok_kodu = @stok_kodu";

                        using (MySqlTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                using (MySqlCommand updateStokCommand = new MySqlCommand(updateTblStokQuery, connection, transaction))
                                {
                                    updateStokCommand.Parameters.AddWithValue("@stok_kodu", stok.stok_kodu);
                                    updateStokCommand.Parameters.AddWithValue("@stok_adi", stok.stok_adi);
                                    updateStokCommand.Parameters.AddWithValue("@uretici_kodu", (object?)stok.uretici_kodu ?? DBNull.Value);
                                    updateStokCommand.Parameters.AddWithValue("@grup_kodu", stok.grup_kodu);
                                    updateStokCommand.Parameters.AddWithValue("@olcu_birimi", (object?)stok.olcu_birimi ?? DBNull.Value);
                                    updateStokCommand.Parameters.AddWithValue("@kdv_orani", (object?)stok.kdv_orani ?? DBNull.Value);
                                    updateStokCommand.Parameters.AddWithValue("@kilitli", (object?)stok.kilitli ?? DBNull.Value);

                                    updateStokCommand.ExecuteNonQuery();
                                }

                                using (MySqlCommand updateStokEkCommand = new MySqlCommand(updateTblStokEkQuery, connection, transaction))
                                {
                                    updateStokEkCommand.Parameters.AddWithValue("@stok_kodu", stok.stok_kodu);
                                    updateStokEkCommand.Parameters.AddWithValue("@kod1", (object?)stok.kod1 ?? DBNull.Value);
                                    updateStokEkCommand.Parameters.AddWithValue("@kod2", (object?)stok.kod2 ?? DBNull.Value);
                                    updateStokEkCommand.Parameters.AddWithValue("@kod3", (object?)stok.kod3 ?? DBNull.Value);
                                    updateStokEkCommand.Parameters.AddWithValue("@onceki_kod", (object?)stok.onceki_kod ?? DBNull.Value);
                                    updateStokEkCommand.Parameters.AddWithValue("@ingilizce_isim", (object?)stok.ingilizce_isim ?? DBNull.Value);
                                    updateStokEkCommand.Parameters.AddWithValue("@duzeltme_yapan", currentUser);

                                    updateStokEkCommand.ExecuteNonQuery();
                                }

                                transaction.Commit();
                                MessageBox.Show("Stok başarıyla güncellendi!", "Güncelleme Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                MessageBox.Show("Güncelleme başarısız oldu: KDV değeri uyumlu değil!", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }

                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    string insertTblStokQuery = @"INSERT INTO tblstok (stok_kodu, stok_adi, uretici_kodu, grup_kodu, olcu_birimi, kdv_orani, kilitli) 
                              VALUES (@stok_kodu, @stok_adi, @uretici_kodu, @grup_kodu, @olcu_birimi, @kdv_orani, @kilitli)";

                    string insertTblStokEkQuery = @"INSERT INTO tblstokek (stok_kodu, kod1, kod2, kod3, onceki_kod, ingilizce_isim, kayit_tarihi, kayit_yapan) 
                                VALUES (@stok_kodu, @kod1, @kod2, @kod3, @onceki_kod, @ingilizce_isim, NOW(), @kayit_yapan)";

                    using (MySqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (MySqlCommand insertStokCommand = new MySqlCommand(insertTblStokQuery, connection, transaction))
                            {
                                insertStokCommand.Parameters.AddWithValue("@stok_kodu", stok.stok_kodu);
                                insertStokCommand.Parameters.AddWithValue("@stok_adi", stok.stok_adi);
                                insertStokCommand.Parameters.AddWithValue("@uretici_kodu", (object?)stok.uretici_kodu ?? DBNull.Value);
                                insertStokCommand.Parameters.AddWithValue("@grup_kodu", stok.grup_kodu);
                                insertStokCommand.Parameters.AddWithValue("@olcu_birimi", (object?)stok.olcu_birimi ?? DBNull.Value);
                                insertStokCommand.Parameters.AddWithValue("@kdv_orani", (object?)stok.kdv_orani ?? DBNull.Value);
                                insertStokCommand.Parameters.AddWithValue("@kilitli", (object?)stok.kilitli ?? DBNull.Value);

                                insertStokCommand.ExecuteNonQuery();
                            }

                            using (MySqlCommand insertStokEkCommand = new MySqlCommand(insertTblStokEkQuery, connection, transaction))
                            {
                                insertStokEkCommand.Parameters.AddWithValue("@stok_kodu", stok.stok_kodu);
                                insertStokEkCommand.Parameters.AddWithValue("@kod1", (object?)stok.kod1 ?? DBNull.Value);
                                insertStokEkCommand.Parameters.AddWithValue("@kod2", (object?)stok.kod2 ?? DBNull.Value);
                                insertStokEkCommand.Parameters.AddWithValue("@kod3", (object?)stok.kod3 ?? DBNull.Value);
                                insertStokEkCommand.Parameters.AddWithValue("@onceki_kod", (object?)stok.onceki_kod ?? DBNull.Value);
                                insertStokEkCommand.Parameters.AddWithValue("@ingilizce_isim", (object?)stok.ingilizce_isim ?? DBNull.Value);
                                insertStokEkCommand.Parameters.AddWithValue("@kayit_yapan", KullaniciDTO.Kullanici_Adi);

                                insertStokEkCommand.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            MessageBox.Show("Stok başarıyla eklendi!", "Ekleme Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("Ekleme başarısız oldu: " + ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            return true;
        }
    }
}
