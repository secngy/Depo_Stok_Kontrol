using Depo_Stok_Kontrol.DTO;
using MySql.Data.MySqlClient;
using System.Windows;

namespace Depo_Stok_Kontrol.ViewModel
{
    internal class DepoViewModel
    {
        private static string _connectionString = "Server=127.0.0.1;Database=depo_stok_kontrol;Uid=root;Pwd='password';";



        public List<DepoDTO> GetStorages(string depoKodu, string depoIsmi)
        {
            List<DepoDTO>? storages = new List<DepoDTO>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM tbldepo WHERE 1=1";

                if (!string.IsNullOrEmpty(depoKodu))
                {
                    query += " AND depo_kodu LIKE @depo_kodu";
                }

                if (!string.IsNullOrEmpty(depoIsmi))
                {
                    query += " AND depo_ismi LIKE @depo_ismi";
                }

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(depoKodu))
                    {
                        command.Parameters.AddWithValue("@depo_kodu", "%" + depoKodu + "%");
                    }

                    if (!string.IsNullOrEmpty(depoIsmi))
                    {
                        command.Parameters.AddWithValue("@depo_ismi", "%" + depoIsmi + "%");
                    }

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DepoDTO depo = new DepoDTO()
                            {
                                depo_kodu = reader.GetString("depo_kodu"),
                                depo_ismi = reader.GetString("depo_ismi"),
                                kilitli = reader.IsDBNull(reader.GetOrdinal("kilitli")) ? null : reader.GetString("kilitli"),
                                eksi_bakiye = reader.IsDBNull(reader.GetOrdinal("eksi_bakiye")) ? null : reader.GetString("eksi_bakiye"),
                                dacik1 = reader.IsDBNull(reader.GetOrdinal("dacik1")) ? null : reader.GetString("dacik1"),
                                dacik2 = reader.IsDBNull(reader.GetOrdinal("dacik2")) ? null : reader.GetString("dacik2"),
                                dacik3 = reader.IsDBNull(reader.GetOrdinal("dacik3")) ? null : reader.GetString("dacik3"),
                                kayit_tarihi = reader.GetDateTime("kayit_tarihi"),
                                kayit_yapan = reader.GetString("kayit_yapan"),
                                duzeltme_tarihi = reader.IsDBNull(reader.GetOrdinal("duzeltme_tarihi")) ? (DateTime?)null : reader.GetDateTime("duzeltme_tarihi"),
                                duzeltme_yapan = reader.IsDBNull(reader.GetOrdinal("duzeltme_yapan")) ? null : reader.GetString("duzeltme_yapan")
                            };
                            storages.Add(depo);
                        }
                    }
                }
            }
            return storages;
        }

        public bool AddOrUpdateDepo(DepoDTO depo, string currentUser)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string checkQuery = "SELECT COUNT(*) FROM tbldepo WHERE depo_kodu = @depo_kodu";
                using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@depo_kodu", depo.depo_kodu);
                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (depo.kilitli != null && !(depo.kilitli.Equals("e", StringComparison.OrdinalIgnoreCase) ||
                                 depo.kilitli.Equals("h", StringComparison.OrdinalIgnoreCase)))
                    {
                        MessageBox.Show("Kilitli alanı sadece E/H değerleri alabilir.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }

                    if (count > 0)
                    {
                        MessageBoxResult result = MessageBox.Show("Böyle bir depo zaten mevcut. Güncellemek ister misiniz?",
                            "Güncelleme Onayı", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.Yes)
                        {

                           

                            string updateQuery = @"UPDATE tbldepo 
                                                 SET depo_ismi = @depo_ismi, 
                                                 kilitli = @kilitli, 
                                                 eksi_bakiye = @eksi_bakiye, 
                                                 dacik1 = @dacik1, 
                                                 dacik2 = @dacik2, 
                                                 dacik3 = @dacik3, 
                                                 duzeltme_tarihi = NOW(), 
                                                 duzeltme_yapan = @duzeltme_yapan 
                                                 WHERE depo_kodu = @depo_kodu";

                            using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection))
                            {
                                updateCommand.Parameters.AddWithValue("@depo_kodu", depo.depo_kodu);
                                updateCommand.Parameters.AddWithValue("@depo_ismi", depo.depo_ismi);
                                updateCommand.Parameters.AddWithValue("@kilitli", (object?)depo.kilitli ?? DBNull.Value);
                                updateCommand.Parameters.AddWithValue("@eksi_bakiye", (object?)depo.eksi_bakiye ?? DBNull.Value);
                                updateCommand.Parameters.AddWithValue("@dacik1", (object?)depo.dacik1 ?? DBNull.Value);
                                updateCommand.Parameters.AddWithValue("@dacik2", (object?)depo.dacik2 ?? DBNull.Value);
                                updateCommand.Parameters.AddWithValue("@dacik3", (object?)depo.dacik3 ?? DBNull.Value);
                                updateCommand.Parameters.AddWithValue("@duzeltme_yapan", currentUser);

                                updateCommand.ExecuteNonQuery();
                                MessageBox.Show("Depo başarıyla güncellendi!", "Güncelleme Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (depo.kilitli != null && !(depo.kilitli.Equals("e", StringComparison.OrdinalIgnoreCase) ||
                                  depo.kilitli.Equals("h", StringComparison.OrdinalIgnoreCase)))
                        {
                            MessageBox.Show("Kilitli alanı sadece E/H değerleri alabilir.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }

                        string insertQuery = @"INSERT INTO tbldepo (depo_kodu, depo_ismi, kilitli, eksi_bakiye, dacik1, dacik2, dacik3, 
                                      kayit_tarihi, kayit_yapan) 
                                      VALUES (@depo_kodu, @depo_ismi, @kilitli, @eksi_bakiye, @dacik1, @dacik2, @dacik3, NOW(), @kayit_yapan)";

                        using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@depo_kodu", depo.depo_kodu);
                            insertCommand.Parameters.AddWithValue("@depo_ismi", depo.depo_ismi);
                            insertCommand.Parameters.AddWithValue("@kilitli", (object?)depo.kilitli ?? DBNull.Value);
                            insertCommand.Parameters.AddWithValue("@eksi_bakiye", (object?)depo.eksi_bakiye ?? DBNull.Value);
                            insertCommand.Parameters.AddWithValue("@dacik1", (object?)depo.dacik1 ?? DBNull.Value);
                            insertCommand.Parameters.AddWithValue("@dacik2", (object?)depo.dacik2 ?? DBNull.Value);
                            insertCommand.Parameters.AddWithValue("@dacik3", (object?)depo.dacik3 ?? DBNull.Value);
                            insertCommand.Parameters.AddWithValue("@kayit_yapan", KullaniciDTO.Kullanici_Adi);

                            insertCommand.ExecuteNonQuery();
                            MessageBox.Show("Depo başarıyla eklendi!", "Ekleme Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            return true;
        }
    }
}
