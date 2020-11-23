using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Echo
{
    public static class Extensions
    {
        public static bool Validate(this AR.ARNews.User sender)
        {
            using(MySqlConnection con = new MySqlConnection(Security.ConnectionString))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(@"SELECT PW FROM USER
                            WHERE USERNAME = @UN", con))
                {
                    cmd.Parameters.AddWithValue("@UN", sender.Name);

                    MySqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                        if (Security.HashPassword(sender.PW) == dr[0].ToString())
                            return true;
                }
            }

            return false;
        }
    }
}
