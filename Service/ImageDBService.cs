using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using test.Models;
using test.ViewModels;

namespace test.Service
{
    public class ImageDBService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["ASP.NET MVC"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection(cnstr);
        public void InsertImage(Image imv)
        {
            string sql = $@"INSERT INTO AImage(Account,Image) VALUES ('{imv.Account}','{imv.A_Image}');";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public void InsertImages(Image imv)
        {
            string sql = $@"INSERT INTO Image(Account,Image,CreateTime) VALUES ('{imv.Account}','{imv.A_Image}','{DateTime.Now.Date.ToString("yyyy/MM/dd HH:mm:ss")}');";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public List<Image>getAllImage(string Account)
        {
            List<Image> m = new List<Image>();
            string sql = $@"select * from Image where Account = '{Account}';";
            Image g = new Image();
            try 
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    g.Account = dr["Account"].ToString();
                    g.Name = dr["Account"].ToString();
                    g.A_Image = dr["Image"].ToString();
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
        public Image GetImage(string Account)
        {
            Image g = new Image();
            string sql = $@"select * from AImage where Account = '{Account}';";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                g.Account = dr["Account"].ToString();
                g.Name = dr["Account"].ToString();
                g.A_Image = dr["Image"].ToString();
            }
            catch (Exception e)
            {
                g = null;
            }
            finally
            {
                conn.Close();
            }
            return g;
        }
       

    }
}