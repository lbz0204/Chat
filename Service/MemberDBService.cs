using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using test.Models;
using test.ViewModels;

namespace test.Service
{
    public class MemberDBService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["ASP.NET MVC"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection(cnstr);
        private readonly MailService ms = new MailService();
        public void Register(Member newMember)
        {
            string Auth = "xxx";
            string sql = $@"INSERT INTO Member(Account,Password,Name,Email,AuthCode,IsAdmin) VALUES ('{newMember.Account}','{newMember.Password}',
             '{newMember.Name}','{newMember.Email}','{Auth}','0')";
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
        public Member GetDataByAccount(String Account)
        {
            Member Data = new Member();
            string sql = $@"select * from Member where Account='{Account}'";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.Account = dr["Account"].ToString();
                Data.Password = dr["Password"].ToString();
                Data.Name = dr["Name"].ToString();
                Data.Email = dr["Email"].ToString();
                Data.AuthCode = dr["AuthCode"].ToString();
                Data.IsAdmin = Convert.ToBoolean(dr["IsAdmin"]);
            }
            catch (Exception e)
            {
                Data = null;
            }
            finally
            {
                conn.Close();
            }
            return Data;
        }
        public List<Member> GetDataByName(String Name)
        {
            List<Member> data = new List<Member>();
            string sql = $@"select * from Member where Name='{Name}'";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Member Data = new Member();
                    Data.Account = dr["Account"].ToString();
                    Data.Password = dr["Password"].ToString();
                    Data.Name = dr["Name"].ToString();
                    Data.Email = dr["Email"].ToString();
                    Data.AuthCode = dr["AuthCode"].ToString();
                    Data.IsAdmin = Convert.ToBoolean(dr["IsAdmin"]);
                    data.Add(Data);
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
            return data;
        }
        public bool AccountCheck(string Name, string Password)
        {
            bool result = true;
            string sql = $@"select * from Member where Name='{Name}' and Password ='{Password}'";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                result = false;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }
        public String SendValidateCode(String Account)
        {
            Member m = new Member();
            m = GetDataByAccount(Account);
            string ToEmail = m.Email;
            string s = ms.GetValidateCode();
            ms.SendRegisterMail(s, ToEmail);
            return s;
        }
        public void Register(MemberReg newMember)
        {
            newMember.Password = HashPassword(newMember.Password);
            string Auth = "xxx";
            string sql = $@"INSERT INTO Member(Account,Password,Name,Email,AuthCode,IsAdmin) VALUES ('{newMember.Account}','{newMember.Password}',
             '{newMember.Name}','{newMember.Email}','{Auth}','0')";
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
        public string HashPassword(string Password)
        {
            string saltkey = "1q2w3e4r5t6y7u8ui9o0po7tyy";
            string saltAndPassword = String.Concat(Password, saltkey);
            SHA256CryptoServiceProvider sha256Hasher = new SHA256CryptoServiceProvider();
            byte[] PasswordData = Encoding.Default.GetBytes(saltAndPassword);
            byte[] HashDate = sha256Hasher.ComputeHash(PasswordData);
            string HashResult = Convert.ToBase64String(HashDate);
            return HashResult;
        }
        public bool AccountCheck(MemberLogin data)
        {
            Member Data = GetDataByAccountandP(data);
            bool result;
            if (Data == null)
                result = false;
            else
                result = true;
            return result;
        }
        private Member GetDataByAccountandP(MemberLogin data)
        {
            Member Data = new Member();
            string Account = data.Account;
            string sql = $@"select * from Member where Account='{Account}'";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.Account = dr["Account"].ToString();
                Data.Password = dr["Password"].ToString();
                Data.Name = dr["Name"].ToString();
                Data.Email = dr["Email"].ToString();
                Data.AuthCode = dr["AuthCode"].ToString();
                Data.IsAdmin = Convert.ToBoolean(dr["IsAdmin"]);

                if (Data.Password.Equals(HashPassword(data.Password)) == false)
                {
                    Data = null;

                }
                string au = "xxx";
                if (au.Equals(Data.AuthCode) == true)
                {
                    Data = null;
                }
            }
            catch (Exception e)
            {
                Data = null;
            }
            finally
            {
                conn.Close();
            }

            return Data;
        }
        public void WriteValidateCode(string Account, string validate)
        {
            string sql = $@"UPDATE Member SET AuthCode='{validate}'WHERE Account='{Account}';";
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
        /*
       
        public void Register(Member newMember)
        {
            string Auth = "xxx";
            string sql = $@"INSERT INTO Member1(Account,Password,Name,Email,AuthCode,IsAdmin) VALUES ('{newMember.Account}','{newMember.Password}',
             '{newMember.Name}','{newMember.Email}','{Auth}','0')";
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
       
       
       
        
        public string GetRole(string Account)
        {
            string Role = "User";
            Member LoginMember = GetDataByAccount(Account);
            if(LoginMember.IsAdmin)
            {
                Role += ",Admin";
            }
            return Role;
        }
        public Member GetDataByAccount(String Account)
        {
            Member Data = new Member();
            string sql = $@"select * from Member1 where Account='{Account}'";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.Account = dr["Account"].ToString();
                Data.Password = dr["Password"].ToString();
                Data.Name = dr["Name"].ToString();
                Data.Email = dr["Email"].ToString();
                Data.AuthCode = dr["AuthCode"].ToString();
                Data.IsAdmin = Convert.ToBoolean(dr["IsAdmin"]);
            }
            catch (Exception e)
            {
                Data = null;
            }
            finally
            {
                conn.Close();
            }
            return Data;
        }
        public List<Member> GetDataByName(String Name)
        {
            List<Member> data = new List<Member>();
            string sql = $@"select * from Member1 where Name='{Name}'";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Member Data = new Member();
                    Data.Account = dr["Account"].ToString();
                    Data.Password = dr["Password"].ToString();
                    Data.Name = dr["Name"].ToString();
                    Data.Email = dr["Email"].ToString();
                    Data.AuthCode = dr["AuthCode"].ToString();
                    Data.IsAdmin = Convert.ToBoolean(dr["IsAdmin"]);
                    data.Add(Data);
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
            return data;

        }
        public List<Image> GetImage(List<Member> data)
        {
            List<Image> Idata = new List<Image>();
            for (int i = 0; i < data.Count; i++)
            {
                try
                {
                    Image Im = new Image();
                    string sql = $@"select * from AImage where Account='{data[i].Account}'";
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader dr = cmd.ExecuteReader();
                    Im.Account = data[i].Account;
                    Im.Name = data[i].Name;
                    Im.AImage = dr["AImage"].ToString();
                    Idata.Add(Im);
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
                return Idata;
        }
        
        
       */
    }
}