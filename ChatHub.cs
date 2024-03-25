using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using test.Models;
using test.Service;

namespace test
{
    public class ChatHub : Hub
    {
        public static List<User> ConnId = new List<User>();
        MemberDBService mService = new MemberDBService();
        FriendDBService fService = new FriendDBService();
        ConversationDBService cService = new ConversationDBService();
        MessageDBService msService = new MessageDBService();
        AgreeDBService aService = new AgreeDBService();
        GroupDBService gService = new GroupDBService();


        public static List<string> ConnIDList = new List<string>();
        public static List<Conversation> Conv = new List<Conversation>();
        public static List<Friend> Fd = new List<Friend>();
        public static List<Message> Mg = new List<Message>();
        public static List<Agree> Ag = new List<Agree>();

        public static List<string> Gn = new List<string>();

        public static List<GroupAndNewMem> gm = new List<GroupAndNewMem>();
        public static List<GroupCon> gc = new List<GroupCon>();
        public static List<GroupMem> gr = new List<GroupMem>();
        public void addAllMemtoGroup(string[] groups, string []mem,int []index)
        {
            for (int i = 0; i < groups.Length; i++)
            {
                
                for(int j=0;j<index[i];j++)
                {
                    User user = ConnId.Find(delegate (User user1) { return user1.UserName == mem[i]; });
                    Gn.Add(groups[i]);
                    Groups.Add(user.ConnectionID, groups[i]);
                }
            }
        }

        public void addMessageToGroup(string gid,string creater,string gname)
        {
            User user1 = ConnId.Find(delegate (User user) { return user.UserName == creater; });
            Clients.Client(user1.ConnectionID).UpdateGM(Context.User.Identity.Name,gid,gname);

        }
        public void SendMessageGroup(int i,string gid,string []mem,string msg)
        { 
            if (i==0)
            {
                GroupCon c = new GroupCon();
                c.Account_A = Context.User.Identity.Name;
                c.Account_B = mem[0];
                c.Conv = msg;
                c.File = "xxx";
                c.Group_Id = Convert.ToInt32(gid);
                gc.Add(c);
                Clients.Group(gid).UpdateGMessage(Context.User.Identity.Name, msg, gid);
            }
            else if(i==1)
            {
                for (int j = 0; j < mem.Length; j++)
                {
                    GroupCon c = new GroupCon();
                    c.Account_A = Context.User.Identity.Name;
                    c.Account_B = mem[i];
                    c.Conv = msg;
                    c.File = "xxx";
                    c.Group_Id= Convert.ToInt32(gid);
                    gc.Add(c);
                }
                Clients.Group(gid,mem).UpdateGMessage(Context.User.Identity.Name, msg, gid);
            }
            else
            {

            }    
        }

        public void addFriendToGroup(string Account,string gid,string gname)
        { 
            GroupAndNewMem m = new GroupAndNewMem();
            m.GroupName = gid;
            m.NewMem = Account;
            gm.Add(m);
            User user1 = ConnId.Find(delegate (User user) { return user.UserName == Account; });
            Clients.Client(user1.ConnectionID).UpdateGM(Account, gid, gname);
        }
        public void addMemToGroup(string Account, string gname)
        {
            GroupMem g = new GroupMem();
            g.Account = Account;
            g.GroupId = Convert.ToInt32(gname);
            gr.Add(g);
            User user1 = ConnId.Find(delegate (User user) { return user.UserName == Account; });
            Clients.Client(user1.ConnectionID).UpdateC(g.GroupId);
        }
        public void testc()
        {
            User user1 = ConnId.Find(delegate (User user) { return user.UserName == Context.User.Identity.Name; });
            User user2 = ConnId.Find(delegate (User user) { return user.UserName == "power"; });
            Clients.Client(user1.ConnectionID).Utest(user2.UserName);
            Clients.Client(user2.ConnectionID).utest(user1.UserName);
        }
        public void SendInvite(string who, string msg)
        {
            
            User user2 = ConnId.Find(delegate (User user) { return user.UserName=="power1"; });
            Friend f = new Friend();
           
            if (msg.Equals("add"))
            {

                f.Account_A = Context.User.Identity.Name;
                f.Account_B = who;
                f.State = 1;
                f.S_Time = DateTime.Now;
                Fd.Add(f);
                Clients.Client(Context.ConnectionId).UpdateFriend(who, "已送出交友請函");
                Clients.Client(user2.ConnectionID).ShowF(Context.User.Identity.Name, "向你送出邀請");

            }

            else if (msg.Equals("cancel"))
            {
                Friend f1 = Fd.Find(delegate (Friend f2) { return f2.Account_B == Context.User.Identity.Name; });
                f1.State = 0;
                Clients.Client(Context.ConnectionId).UpdateFriend(who, "加入");
                Clients.Client(user2.ConnectionID).ShowF(who, "加入");
            }
            else if (msg.Equals("accept"))
            {
               
                Friend f1 = Fd.Find(delegate (Friend f2) { return f2.Account_B == Context.User.Identity.Name; });
                Friend f3 = new Friend();
                f3.Account_A = Context.User.Identity.Name;
                f3.Account_B = who;
                f3.State = 3;
                f3.F_Time = DateTime.Now;
                f3.R_Time = DateTime.Now;
                f3.S_Time = DateTime.Now;
                Fd.Add(f3);
                f1.State = 3;
                Clients.Client(user2.ConnectionID).UpdateFriend(Context.User.Identity.Name, "朋友");
                Clients.Client(Context.ConnectionId).UpdateFriend(who, "朋友");

            }
        }
        public void SendG(string id)
        {
           
            Agree a = new Agree();
            a.Account = Context.User.Identity.Name;
            a.M_Id = Convert.ToInt32(id);
            a.CreateTime = DateTime.Now;
            Ag.Add(a);
            Clients.Client(Context.ConnectionId).UpdateAgree(id);
        }
        public void SendA(string name, string message)
        {
           
            Message m = new Message();
            m.Account = Context.User.Identity.Name;
            m.A_Id = Convert.ToInt32(name);
            m.Content = message;
            m.CreateTime = DateTime.Now;
            Mg.Add(m);
            Clients.All.UpdateMessagef(name, Context.User.Identity.Name, message, m.CreateTime.ToString("yyyy/MM/dd HH:mm:ss"));
        }
        public void Send(string name,string message)
        {

          
             Conversation con = new Conversation();
             con.Account_A = Context.User.Identity.Name;
             con.Account_B = name;
             con.Content = message;
             con.CreateTime = DateTime.Now;
             Conv.Add(con);

             User user2 = ConnId.Find(delegate (User user) { return user.UserName == name; });
             User user1 = ConnId.Find(delegate (User user) { return user.UserName == Context.User.Identity.Name; });


             Clients.Client(user2.ConnectionID).UpdateMessage(Context.User.Identity.Name, message, con.CreateTime.ToString("yyyy/MM/dd HH:mm:ss"));

             Clients.Client(Context.ConnectionId).UpdateMessage(name,message, con.CreateTime.ToString("yyyy/MM/dd HH:mm:ss"));
           
        }
        
        public override Task OnConnected()
        {

            
            User user1 = new User();
            if (ConnId.Find(delegate (User user) { return user.UserName == Context.User.Identity.Name; }) == null)
            {
                user1.UserName = Context.User.Identity.Name;
                user1.ConnectionID = Context.ConnectionId;
                user1.Connected = true;
                ConnId.Add(user1);

            }
            else
            {
                user1 = ConnId.Find(delegate (User user) { return user.UserName == Context.User.Identity.Name; });
                user1.ConnectionID = Context.ConnectionId;
                user1.Connected = true;
            }
           
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            cService.InsertConversations(Conv);

            if(Fd!=null)
            {
                fService.SendInvite(Fd);
            }
            if(Mg!=null)
            {
                msService.InsertMessages(Mg);
            }
            if(Ag!=null)
            {
                aService.InsertAgrees(Ag);
            }
            if(gc!=null)
            {
                gService.addGroupCon(gc);
            }
            if(gm!=null)
            {
                gService.addGroupNewM(gm, Context.User.Identity.Name);
            }
            if(gr!=null)
            {
                gService.addGroupMem(gr);
            }
            User user1 = ConnId.Find(delegate (User user) { return user.UserName == Context.User.Identity.Name; });
            user1.Connected = false;
            return base.OnDisconnected(stopCalled);
        }
       
    }
}