using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
namespace Echo.Models
{
    public class ClanakModel
    {
        public int? ClanakID { get; set; }
        public string Naslov { get; set; }
        public string Info { get; set; }
        public string Slika { get; set; }
        public string Tekst { get; set; }
        public int GrupaID { get; set; }
        public DateTime Datum { get; set; }
        public int KorisnikID { get; set; }
        public AR.ARNews.ClanakStatus Status { get; set; }

        public ClanakModel()
        {

        }
        public ClanakModel(int ID)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Security.ConnectionString))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(@"SELECT NASLOV, INFO, SLIKA, TEKST, GRUPAID, 
                        DATUM, KORISNIKID, STATUS FROM CLANAK WHERE CLANAKID = @ID", con))
                    {
                        cmd.Parameters.AddWithValue("@ID", ID);

                        MySqlDataReader dr = cmd.ExecuteReader();

                        if(dr.Read())
                        {
                            this.ClanakID = ID;
                            this.Naslov = dr[0].ToString();
                            this.Info = dr[1].ToString();
                            this.Slika = dr[2].ToString();
                            this.Tekst = dr[3].ToString();
                            this.GrupaID = Convert.ToInt32(dr[4]);
                            this.Datum = Convert.ToDateTime(dr[5]);
                            this.KorisnikID = Convert.ToInt32(dr[6]);
                            this.Status = (AR.ARNews.ClanakStatus)Convert.ToInt32(dr[7]);
                        }
                        else
                        {
                            throw new Exception("Story doesn't exist!");
                        }
                    }
                }
            }
            catch
            {
                throw new Exception("Error while communicating with database!");
            }
        }
        public static List<ClanakModel> List(HttpRequest Request)
        {
            if (!Networking.isAdmin(Request))
                return null;

            List<ClanakModel> list = new List<ClanakModel>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(Security.ConnectionString))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(@"SELECT CLANAK.CLANAKID, CLANAK.NASLOV, 
                            CLANAK.INFO, CLANAK.SLIKA, CLANAK.TEKST, CLANAK.GRUPAID, CLANAK.DATUM, 
                            CLANAK.KORISNIKID, CLANAK.STATUS FROM CLANAK", con))
                    {
                        MySqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                            list.Add(
                                new ClanakModel()
                                {
                                    ClanakID = Convert.ToInt32(dr[0]),
                                    Naslov = dr[1].ToString(),
                                    Info = dr[2].ToString(),
                                    Slika = dr[3].ToString(),
                                    Tekst = dr[4].ToString(),
                                    GrupaID = Convert.ToInt32(dr[5]),
                                    Datum = Convert.ToDateTime(dr[6]),
                                    KorisnikID = Convert.ToInt32(dr[7]),
                                    Status = (AR.ARNews.ClanakStatus)Convert.ToInt32(dr[7])
                                });
                    }
                }
            }
            catch(Exception ex)
            {

            }

            return list;
        }

        public bool Kreiraj()
        {
            if (KorisnikID == null || KorisnikID < 1)
                throw new Exception("Invalid user ID!");

            if (Naslov != null && Naslov.Length > 256)
                throw new Exception("Invalid title!");

            if (Info != null && Info.Length > 128)
                throw new Exception("Invalid info!");

            if (Slika != null && Slika.Length > 1024)
                throw new Exception("Invalid image!");

            if (Tekst != null && Tekst.Length > 65535)
                throw new Exception("Invalid body (possible length overflow)!");

            try
            {
                using (MySqlConnection con = new MySqlConnection(Security.ConnectionString))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand("INSERT INTO CLANAK (NASLOV, INFO, SLIKA, TEKST, GRUPAID, DATUM, KORISNIKID, STATUS) VALUES (@N, @I, @S, @T, @G, @D, @KID, @STAT)", con))
                    {
                        cmd.Parameters.AddWithValue("@N", Naslov);
                        cmd.Parameters.AddWithValue("@I", Info);
                        cmd.Parameters.AddWithValue("@S", Slika);
                        cmd.Parameters.AddWithValue("@T", Tekst);
                        cmd.Parameters.AddWithValue("@G", GrupaID);
                        cmd.Parameters.AddWithValue("@D", DateTime.Now);
                        cmd.Parameters.AddWithValue("@STAT", (int)Status);
                        cmd.Parameters.AddWithValue("@KID", KorisnikID);

                        cmd.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public static int GetMaxID()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Security.ConnectionString))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand("SELECT MAX(CLANAKID) FROM CLANAK", con))
                    {
                        MySqlDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                            return Convert.ToInt32(dr[0]);
                        else
                            throw new Exception("Error while selecting!");
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception("ERROR");
            }
        }
    }
}
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
