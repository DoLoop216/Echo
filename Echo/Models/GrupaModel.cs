using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Echo.Models
{
    public class GrupaModel
    {
        public int GrupaID { get; set; }
        public string Naziv { get; set; }
        public int DisplayIndex { get; set; }

        public static List<GrupaModel> List()
        {
            List<GrupaModel> list = new List<GrupaModel>();
            using (MySqlConnection con = new MySqlConnection(Security.ConnectionString))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT GRUPAID, NAZIV, DISPLAYINDEX FROM GRUPA", con))
                {
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                        list.Add(new GrupaModel() { GrupaID = Convert.ToInt32(dr[0]), Naziv = dr[1].ToString(), DisplayIndex = Convert.ToInt32(dr[2]) });
                }
            }

            return list;
        }
    }
}
