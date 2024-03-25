using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using test.Models;
using test.ViewModels;
namespace test.Service
{
    public class ImageNewDBService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["ASP.NET MVC"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection(cnstr);
        public List<ImageNew> getAllImage()
        {
            List<ImageNew> m = new List<ImageNew>();
            string sql = $@"select * from ImageNew;";
            ImageNew g = new ImageNew();
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    g.ImageN = dr["ImageN"].ToString();
                    g.Image= dr["Image"].ToString();
                    m.Add(g);
                }
            }
            catch (Exception e)
            {
                g = null;
            }
            finally
            {
                conn.Close();
            }

            return m;
        }
     }
}