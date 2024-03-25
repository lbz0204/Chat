using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using test.Models;

namespace test.Service
{
    public class ItemDBService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["ASP.NET MVC"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection(cnstr);
        public void Insert(Item Data)
        {
            Random myObject = new Random();
            int Id = myObject.Next(0, 200);
            string sql = $@"INSERT INTO Item(Name,Price,Image) VALUES ('{Data.Name}',{0},'{Data.Image}')";
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
        public List<Item> GetDataList()
        {
            List<Item> DataList = new List<Item>();
            string sql = @"SELECT * FROM Item;";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Item Data = new Item();
                    Data.Id = Convert.ToInt32(dr["Id"]);
                    Data.Name = dr["Name"].ToString();
                    Data.Price = Convert.ToInt32(dr["Price"]);
                    Data.Image = dr["Image"].ToString();
                    DataList.Add(Data);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            return DataList;
        }
    }
}