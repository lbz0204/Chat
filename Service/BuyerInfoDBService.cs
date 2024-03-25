using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using test.Models;
using test.ViewModels;
namespace test.Service
{
 
    public class BuyerInfoDBService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["ASP.NET MVC"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection(cnstr);
        public void InsertBuyerInfo(BuyerInfo by)
        {
            string sql = $@"INSERT INTO BuyerInfo(Buyer,Member,Age,Height,Fdescription,Image,Procession,Location) VALUES ('{by.Buyer}','{by.Member}','{by.Age}','{by.Height}','{by.Fdescription}','{by.Location}','{by.Procession}','{by.Location}');";
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
            try
            {
                conn.Open();
                SqlCommand sqlCmd = new SqlCommand("Select * from BuyerInfo", conn);
                SqlDataReader reader = sqlCmd.ExecuteReader();

                string fileName = "BuyerInfo.csv";
                StreamWriter sw = new StreamWriter(fileName);
                object[] output = new object[reader.FieldCount];

                for (int i = 0; i < reader.FieldCount; i++)
                    output[i] = reader.GetName(i);

                sw.WriteLine(string.Join(",", output));

                while (reader.Read())
                {
                    reader.GetValues(output);
                    sw.WriteLine(string.Join(",", output));
                }
                sw.Close();
                reader.Close();
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
        public void InsertProduct(Product p)
        {
            string sql = $@"INSERT INTO Product(Image,name,price) VALUES ('{p.Image}','{p.name}','{p.price}');";
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
        public List<Product> getProduct()
        {
            List<Product> m = new List<Product>();
            string sql = $@"select * from Product;";
            Product g = new Product();
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    g.Image = dr["Image"].ToString();
                    g.name = dr["name"].ToString();
                    g.price = (int)dr["price"];
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