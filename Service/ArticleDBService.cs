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
    public class ArticleDBService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["ASP.NET MVC"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection(cnstr);
       
        public void InsertArticle(Article art)
        {
            Random myObject = new Random();
            int Id = myObject.Next(0, 200);
            string sql = $@"INSERT INTO Article(Title,Content,Account,CreateTime,Image) VALUES ('{art.Title}','{art.Content}','{art.Account}','{art.CreateTime.ToString("yyyy/MM/dd HH:mm:ss")}','{art.Image}');";
            
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
        public List<Start> GetAllArticle(List<Start> data)
        {
            
            List<Article> Data = new List<Article>();
            List<ArticleAndMessage> aam = new List<ArticleAndMessage>();
            MessageDBService bService = new MessageDBService();
            for(int i=0;i<data.Count;i++)
            {
                Data = GetArticleByName(data[i].Friend);
                List<Message> m = new List<Message>();
                List<Agree> ar = new List<Agree>();
                for (int j=0;j<Data.Count;j++)
                {
                   ArticleAndMessage am = new ArticleAndMessage();
                   ar = GetArticleAgree(Data[j]);
                   m =  bService.GetAllMessage(Data[j]);
                   am.agree = ar;
                   am.article = Data[j];
                   am.message = m;
                   aam.Add(am);
                }
                data[i].AM = aam;
            

            }
            return data;
        }
        public List<Agree> GetArticleAgree(Article a)
        {
            List<Agree> ar = new List<Agree>();
            Agree ag = new Agree();
            string sql = $@"SELECT *FROM Agree where M_Id ='{a.A_Id}';";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ag.Ag_Id = Convert.ToInt32(dr["Ag_Id"]);
                    ag.Account = dr["Account"].ToString();
                    ag.M_Id = Convert.ToInt32(dr["M_Id"]);
                    ag.CreateTime = Convert.ToDateTime(dr["CreateTime"]);
                    ar.Add(ag);
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
          
            return ar;
        }

        public List<Article> GetArticleByName(string Account)
        {
            List<Article> art = new List<Article>();
            
            string sql = $@"SELECT *FROM Article where Account ='{Account}';";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Article a = new Article();
                    a.A_Id = Convert.ToInt32(dr["A_Id"]);
                    a.Account = dr["Account"].ToString();
                    a.Content = dr["Content"].ToString();
                    a.Image = dr["Image"].ToString();
                    a.CreateTime = Convert.ToDateTime(dr["CreateTime"]);
                    a.Title = dr["Title"].ToString();
                    art.Add(a);
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
            return art;
        }
        
        public List<Article> GetSelfAllData(string account)
        {
            List<Article> Data = new List<Article>();

            return Data;
        }
    }
}