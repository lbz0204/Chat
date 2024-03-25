using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using test.Models;

namespace test.Service
{
    public class AgreeDBService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["ASP.NET MVC"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection(cnstr);
        
        public void InsertAgrees(List<Agree> ag)
        {
            for (int i = 0; i < ag.Count; i++)
            {
                string sql = $@"INSERT INTO Agree(M_Id,Account,CreateTime) VALUES ('{ag[i].M_Id}','{ag[i].Account}','{ag[i].Account}','{ag[i].CreateTime.ToString("yyyy/MM/dd HH:mm:ss")}');";
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