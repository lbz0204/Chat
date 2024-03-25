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
    public class MessageDBService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["ASP.NET MVC"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection(cnstr);

      
        public List<Message> GetAllMessage(Article data)
        {
            List<Message> Data = new List<Message>();
            Message m = new Message();
            string sql = $@"SELECT *FROM Message where M_Id ='{data.A_Id}';";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                { 
                    m.A_Id = Convert.ToInt32(dr["A_Id"]);
                    m.Account = dr["Account"].ToString();
                    m.Content= dr["Content"].ToString();
                    m.CreateTime = Convert.ToDateTime(dr["CreateTime"]);
                    m.M_Id = Convert.ToInt32(dr["M_Id"]);
                    Data.Add(m);
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
            return Data;
        }
        public void InsertMessage(Message m)
        {
            Random myObject = new Random();
            int Id = myObject.Next(0, 200);
            string sql = $@"INSERT INTO Message(M_Id,Account,Content,CreateTime) VALUES ('{m.M_Id}','{m.Account}','{m.Content}','{m.CreateTime.ToString("yyyy/MM/dd HH:mm:ss")}');";
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
        public void InsertMessages(List<Message> ms)
        {
            for (int i = 0; i < ms.Count; i++)
            {
                Random myObject = new Random();
                int Id = myObject.Next(0, 200);
                Message m = new Message();
                m = ms[i];
                string sql = $@"INSERT INTO Message(M_Id,Account,Content,CreateTime) VALUES ('{m.M_Id}','{m.Account}','{m.Content}','{m.CreateTime.ToString("yyyy/MM/dd HH:mm:ss")}');";
              
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
        }
    }
}