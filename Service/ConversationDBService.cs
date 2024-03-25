using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using test.Models;

namespace test.Service
{
    public class ConversationDBService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["ASP.NET MVC"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection(cnstr);
        public void InsertConversation(Conversation d)
        {
            Random myObject = new Random();
            int Id = myObject.Next(0, 200);
            string sql = $@"INSERT INTO Con(C_Id,Account_A ,Account_B,Content ,CreateTime) VALUES ('{Id}','{d.Account_A}','{d.Account_B}','{d.Content}','{d.CreateTime.ToString("yyyy/MM/dd HH:mm:ss")}');";
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
        public void InsertConversations(List<Conversation> dl)
        {
            for (int i = 0; i < dl.Count; i++)
            {
                Conversation d = new Conversation();
                d = dl[i];
                Random myObject = new Random();
                int Id = myObject.Next(0, 200);
                string sql = $@"INSERT INTO Con(C_Id,Account_A ,Account_B,Content ,CreateTime) VALUES ('{Id}','{d.Account_A}','{d.Account_B}','{d.Content}','{d.CreateTime.ToString("yyyy/MM/dd HH:mm:ss")}');";
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