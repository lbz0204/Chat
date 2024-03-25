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
    public class FriendDBService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["ASP.NET MVC"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection(cnstr);
        private readonly SqlConnection conn1 = new SqlConnection(cnstr);
        public void addFriend(Friend f)
        {
           
            string sql = $@"INSERT INTO Friend(Account_A,Account_B,State,F_Time,S_Time,R_Time) VALUES ('{f.Account_A}','{f.Account_B}',
             '{f.State}','{f.F_Time}','{f.S_Time}','{f.R_Time}')";
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
        public void delete()
        {
            try
            {

                string sql = $@"delete  from Friend where Account_B='{"林任任"}'";
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
        
        public FCon GetAllFriend(string Account)
        {

            FCon Fc = new FCon();
            List<FriendAndCon> F = new List<FriendAndCon>();
            List<Conversation> lc = new List<Conversation>();
            string sql = $@"select * from Friend where Account_A='{Account}' or Account_B='{Account}'";/*Account_A ='{Account}' or Account_B='{Account}'*/
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                FriendAndCon fc = new FriendAndCon();
                /*
                dr.Read();
                fc.Friend = dr["Account_B"].ToString();
                F.Add(fc);
                Fc.Fc = F;
               */
                SqlCommand cmd1;
                SqlDataReader dr1;
                while (dr.Read())
               {
                    
                   /*FriendAndCon fc = new FriendAndCon();*/
                   if (Convert.ToInt32(dr["State"]) == 3)
                   {
                       string sql1 = $@"SELECT *FROM Con where (Account_A ='{Account}' OR Account_B='{dr["Account_B"]}') OR (Account_A ='{dr["Account_A"]}' OR Account_B='{Account}');";
                       try
                       {
                           conn1.Open();
                           cmd1 = new SqlCommand(sql1, conn1);
                           dr1 = cmd1.ExecuteReader();
                           while (dr1.Read())
                           {
                               Conversation c = new Conversation();
                               c.Account_A = dr1["Account_A"].ToString();
                               c.Account_B = dr1["Account_B"].ToString();
                               c.Content = dr1["Content"].ToString();
                               c.CreateTime = Convert.ToDateTime(dr1["CreateTime"]);
                               c.C_Id = Convert.ToInt32(dr1["C_Id"]);
                               lc.Add(c);
                           }
                           fc.Con = lc;
                       }
                       catch (Exception e)
                       {
                           throw new Exception(e.Message.ToString());
                       }
                       finally
                       {
                           conn1.Close();
                       }

                       if (Account.Equals(dr["Account_A"]) == true)
                       {
                           fc.Account = dr["Account_B"].ToString();
                       }
                       else
                       {
                           fc.Account = dr["Account_A"].ToString();
                       }
                       fc.Friend = dr["Account_A"].ToString();
                       sql1 = $@"SELECT *FROM Member1 where Account ='{fc.Account}';";
                       conn1.Open();
                       cmd1 = new SqlCommand(sql1, conn1);
                       dr1 = cmd1.ExecuteReader();
                       dr1.Read();
                       fc.Friend = dr1["Name"].ToString();
                       F.Add(fc);
                       Fc.Fc = F;
                       conn1.Close();
                    }

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
            return Fc;
        }
        public List<Friend> GetDataList(string Search)
        {
            List<Friend> Datal = new List<Friend>();
            string sql = $@"SELECT *FROM Friend where Account_A ='{Search}' or Account_B ='{Search}';";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Friend Data = new Friend();
                    Data.F_Id = Convert.ToInt32(dr["F_Id"]);
                    Data.Account_A = dr["Account_A"].ToString();
                    Data.Account_B = dr["Account_B"].ToString();
                    Data.State = Convert.ToInt32(dr["State"]);
                    Data.F_Time = Convert.ToDateTime(dr["F_Time"]);
                    Data.S_Time = Convert.ToDateTime(dr["S_Time"]);
                    Data.R_Time = Convert.ToDateTime(dr["R_Time"]);
                    Datal.Add(Data);
                }
            }
            catch(Exception e)
            {
                /*throw new Exception(e.Message.ToString());*/
                Datal= null;
            }
            finally
            {
                conn.Close();
            }
            return Datal;
        }
       
        public void SendInvite(List<Friend> fs)
        {
            Friend f = new Friend();
            for (int i = 0; i < fs.Count; i++)
            {
                Random myObject = new Random();
                int Id = myObject.Next(0, 200);
                f = fs[i];
                try
                {
                    string sql = $@"INSERT INTO Friend(F_Id,Account_A,Account_B,State,F_Time,S_Time,R_Time) VALUES ('{Id}','{f.Account_A}','{f.Account_B}','{f.State}','{f.S_Time.ToString("yyyy/MM/dd HH:mm:ss")}','{f.S_Time.ToString("yyyy/MM/dd HH:mm:ss")}','{f.S_Time.ToString("yyyy/MM/dd HH:mm:ss")}');";
                    Id++;
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader dr = cmd.ExecuteReader();
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
        /*
        public void SendInvite(string UserName, string FriendName)
        {
            try
            {
                string sql = $@"INSERT INTO Friend(F_Id,Account_A,Account_B,State,F_Time,S_Time,R_Time) VALUES ('{Id}','{UserName}','{FriendName}','0','0','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','0');";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }*/
        /*
        public bool CheckExistence(string UserName, string FriendName)
        {
            bool f = true;
            string sql1 = $@"SELECT *FROM Friend where Account_A ={UserName} and Account_B = {FriendName};";
            string sql2 = $@"SELECT *FROM Friend where Account_B ={FriendName} and Account_A ={UserName} ;";
            SqlCommand cmd = new SqlCommand(sql1, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr == null)
                f = false;
            cmd = new SqlCommand(sql2, conn);
            dr = cmd.ExecuteReader();
            if (dr == null)
                f = false;
            return f;
        }
        */
        public FriendViewModel FriendState(List<Member> m, string name)
        {
            FriendViewModel f = new FriendViewModel();
            List<FriendInfo> fi = new List<FriendInfo>();
            FriendInfo d = new FriendInfo();
            for (int i = 0; i < m.Count; i++)
            {
                d.State = CheckExistence(name, m[i].Account);
                d.Account = m[i].Account;
                d.FriendName = m[i].Name;
                fi.Add(d);
            }
            f.LFriend = fi;
            return f;
        }
        public int CheckExistence(string UserName, string FriendName)
        {
            int f=0 ;
            try
            {
                string sql1 = $@"SELECT *FROM Friend where (Account_A ='{UserName}' and Account_B = '{FriendName}') or  (Account_B ='{FriendName}' and Account_A ='{UserName}');";
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql1, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (dr == null)
                        f = 0;
                    else
                        f = Convert.ToInt32(dr["State"]);
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
            return f;
        }
    }
}