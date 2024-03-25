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
    public class GroupDBService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["ASP.NET MVC"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection(cnstr);
        private readonly SqlConnection conn1 = new SqlConnection(cnstr);
        private readonly SqlConnection conn3 = new SqlConnection(cnstr);
        private readonly SqlConnection conn4 = new SqlConnection(cnstr);
        public void addGroup(Group g)
        {
            Random myObject = new Random();
            int Id = myObject.Next(0, 200);
            string sql = $@"INSERT INTO Group1(Group_Name, CreateTime) VALUES ('{g.GroupName}','{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}');";
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
       
        public void addGroupCreater(int gid, string Name)
        {
            Random myObject = new Random();
            int Id = myObject.Next(0, 200);
            string sql = $@"INSERT INTO GroupCreater1(Account,Group_Id) VALUES ('{Name}','{gid}');";
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
public void addGroupMem(List<GroupMem> gm)
        {
            for(int i=0;i<gm.Count;i++)
            {
                Random myObject = new Random();
                int Id = myObject.Next(0, 200);
                string sql = $@"INSERT INTO GroupM1(Account,Group_Id,CreateTime) VALUES ('{gm[i].Account}','{gm[i].GroupId}','{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}');";
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
        public void addGroupCon(List<GroupCon> gn)
        {
            for(int i=0;i<gn.Count;i++)
            {
                Random myObject = new Random();
                int Id = myObject.Next(0, 200);
                string sql = $@"INSERT INTO GroupC(Account_A,Account_B,Conv,File,CreateTime,Group_Id) VALUES ('{gn[i].Account_A}','{gn[i].Account_B}','{gn[i].Conv}','{gn[i].File}','{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}','{gn[i].Group_Id}');";
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
        public void addGroupNewM(List<GroupAndNewMem> f,string name)
        {
            for (int i = 0; i < f.Count; i++)
            {
                Random myObject = new Random();
                int Id = myObject.Next(0, 200);
                string sql = $@"INSERT INTO GroupNewM1(Group_Id,Account_A,Account_B,CreateTime) VALUES ({Convert.ToInt32(f[i].GroupName)},'{name}','{f[i].NewMem}','{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}');";
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
        public int getIdbyGroupName(string name)
        {
            string sql = $@"SELECT *FROM Group1 where Group_Name = '{name}';";
            int id=-1;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                id = Convert.ToInt32(dr["Group_Id"]);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            return id;
        }
        public string getGroupNamebyId(int gId)
        {
            string sql = $@"SELECT *FROM Group1 where Group_Id = {gId};";
            string name = null;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                name = dr["Group_Name"].ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            return name;
        }
        public string getGroupCreater(int gid)
        {
            string sql = $@"SELECT *FROM GroupCreater1 where Group_Id = {gid};";
            string gname = null;
            try
            {
                conn3.Open();
                SqlCommand cmd = new SqlCommand(sql, conn3);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                gname = dr["Account"].ToString();
            }

            catch (Exception e)
            {
                gname = null;
            }
            finally
            {
                conn3.Close();
            }
            return gname;
        }
        public List<GroupInfo> getGroupInfo(string name)
        {
            List<GroupInfo> lg = new List<GroupInfo>();
            GroupInfo g = new GroupInfo();
            string sql = $@"SELECT *FROM Group1 where Group_Name = '{name}';";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    g.GroupName = dr["Group_Name"].ToString();
                    g.GroupId = Convert.ToInt32(dr["Group_Id"]);
                    g.Creater = getGroupCreater(g.GroupId);

                    string sql1 = $@"SELECT *From GroupM1 where Account ='{name}' and  Group_Id ={ g.GroupId}";
                    try
                    {
                        conn1.Open();
                        SqlCommand cmd1 = new SqlCommand(sql1, conn);
                        SqlDataReader dr1 = cmd1.ExecuteReader();
                        g.state = 1;
                    }
                    catch (Exception e)
                    {
                        g.state = 0;
                    }
                    finally
                    {
                       conn1.Close();
                    }
                    g.Mem = getAllGMem(g.GroupId);
                    lg.Add(g);

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
            return lg;
        }
            public List<Group> getAccountGroup(string Account)
        {
            List<Group> lg = new List<Group>();
            Group g = new Group();
            string sql = $@"SELECT *FROM GroupM1 where Account = '{Account}';";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    g.Group_Name = Convert.ToInt32(dr["Group_Id"]);
                    lg.Add(g);
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
            return lg;
        }
        public List<GroupMem> getAllGMem(int gid)
        {
            List<GroupMem> lg = new List<GroupMem>();
            GroupMem g = new GroupMem();
            string sql = $@"SELECT *FROM GroupM1 where Group_Id = {gid};";
            try
            {
                conn4.Open();
                SqlCommand cmd = new SqlCommand(sql, conn4);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    g.GroupId = Convert.ToInt32(dr["Group_Id"]);
                    g.Account = dr["Account"].ToString();
                    lg.Add(g);
                }
            }
            catch (Exception e)
            {
                lg = null;
            }
            finally
            {
                conn4.Close();
            }
            return lg;
        }
        public List<GroupCon> getAllGCon(int gid)
        {
            List<GroupCon> lg = new List<GroupCon>();
            GroupCon g = new GroupCon();
            string sql = $@"SELECT *FROM GroupC where Group_Id = '{gid}' ORDER BY CreateTime ;";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    g.Group_Id = Convert.ToInt32(dr["Group_Id"]);
                    g.Account_A = dr["Account_A"].ToString();
                    g.Account_B = dr["Account_B"].ToString();
                    g.Conv = dr["Conv"].ToString();
                    g.File = dr["File"].ToString();
                    lg.Add(g);
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
            return lg;
        }
            public Groups getGroups(string Account)
        {
            List<Group> lg = new List<Group>();
            lg = getAccountGroup(Account);
            Groups gs = new Groups();
            List<GroupData> gd = new List<GroupData>();
            GroupData d = new GroupData();
            List<GroupMem> gm = new List<GroupMem>();
            List<GroupCon> gn = new List<GroupCon>();
            for(int i=0;i<lg.Count;i++)
            {
                gm = getAllGMem(lg[i].Group_Name);
                gn = getAllGCon(lg[i].Group_Name);
                d.Group_Id = lg[i].Group_Name;
                d.AllMem = gm;
                d.AllCon = gn;
                d.GroupName = getGroupNamebyId(lg[i].Group_Name);
                gd.Add(d);
            }
            return gs;

        }
    }

}