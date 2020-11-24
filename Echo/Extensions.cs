using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Echo
{
    public static class Extensions
    {
        public static bool Validate(this AR.ARNews.User sender, out int ID)
        {
            ID = -1;
            using(MySqlConnection con = new MySqlConnection(Security.ConnectionString))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(@"SELECT USERID, PW FROM USER
                            WHERE USERNAME = @UN", con))
                {
                    cmd.Parameters.AddWithValue("@UN", sender.Name);

                    MySqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                        if (Security.HashPassword(sender.PW) == dr["PW"].ToString())
                        {
                            ID = Convert.ToInt32(dr["USERID"]);
                            return true;
                        }
                }
            }

            return false;
        }
        public static AR.ARWebAuthorization.User GetUser(this HttpRequest Request)
        {
            return AR.ARWebAuthorization.GetUser(Request.Cookies["h"]);
        }
    }
}
