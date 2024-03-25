using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using test.Models;
using test.Security;
using test.Service;
using test.ViewModels;

namespace test.Controllers
{
    public class HomeController : Controller
    {
       

        ItemDBService itemService = new ItemDBService();
        MemberDBService memService = new MemberDBService();
        FriendDBService friService = new FriendDBService();
        ArticleDBService artService = new ArticleDBService();
        MessageDBService msgService = new MessageDBService();
        ImageDBService imService = new ImageDBService();
        GroupDBService gService = new GroupDBService();
        BuyerInfoDBService bService = new BuyerInfoDBService();
        ImageNewDBService iService = new ImageNewDBService();
        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(MemberLogin Data)
        {
            if (memService.AccountCheck(Data.Account, Data.Password) == true)
            {
                var ticket = new FormsAuthenticationTicket(1,                                                       //使用者名稱
           Data.Account,
           DateTime.Now,
           DateTime.Now.AddMinutes(60),
           false,
           JsonConvert.SerializeObject(Data),
           FormsAuthentication.FormsCookiePath);
                var encTicket = FormsAuthentication.Encrypt(ticket);
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                return RedirectToAction("FreindShow", "Home");
            }
            else
            {
                ViewData["Message"] = "查無此帳號";
            }
            return View();
        }
        public ActionResult FreindShow()
        {
            FCon f = new FCon();
            f = friService.GetAllFriend(User.Identity.Name);
            f.MImage = imService.GetImage(User.Identity.Name);
            return View(f);
        }
      
        public ActionResult Search(string Name)
        {
            List<Member> m = new List<Member>();
            m = memService.GetDataByName(Name);
            FriendViewModel f = new FriendViewModel();
            f = friService.FriendState(m, User.Identity.Name);
            return View(f);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Product()
        {
            List<Product> m = new List<Product>();
            m = bService.getProduct();
            ProductViewModel pv = new ProductViewModel();
            pv.p = m;
            return View(pv);
        }
        [HttpPost]
        public ActionResult Product(ImageNew im)
        {
            if(im!=null)
            {
                BuyerInfo b = new BuyerInfo();
                bService.InsertBuyerInfo(b);
            }
            return View();
        }
        public ActionResult UploadProduct()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadProduct(Product m)
        {
            bService.InsertProduct(m);
            return View();
        }
        public ActionResult ShowFArticle()
        {
            List<Friend> f = new List<Friend>();
            f = friService.GetDataList(User.Identity.Name);
            StartViewModel st = new StartViewModel();
            List<Start> ss = new List<Start>();
            for(int i=0;i<f.Count;i++)
            {
                Start s = new Start();
                if(f[i].Account_A==User.Identity.Name)
                {
                    s.Friend = f[i].Account_B;
                }
                else
                {
                    s.Friend = f[i].Account_A;
                }
                ss.Add(s);
            }
            ss= artService.GetAllArticle(ss);
            st.StartInfo = ss;
            return View(st);
        }
        
        public ActionResult UploadP()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadP(ImageSelfViewModel im)
        {
            if(im.ImageA!=null)
            {
                string filename = Path.GetFileName(im.ImageA.FileName);
                string Url = Path.Combine(Server.MapPath("~/Images/"), filename);
                im.ImageA.SaveAs(Url);
                Image ig = new Image();
                ig.A_Image = filename;
                ig.Name = User.Identity.Name;
                ig.Account = User.Identity.Name;
                imService.InsertImage(ig);
                return RedirectToAction("FreindShow");
            }
            return View(im);
        }
       
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("FreindShow", "Home");
            return View();
        }
        
        
        [HttpPost]
        public ActionResult Register(MemberReg Data)
        {
            MemberLogin data = new MemberLogin();
            data.Account = Data.Account;
            data.Password = Data.Password;
            if (memService.AccountCheck(data) == true)
            {
                return View();
            }
            else 
            {
                memService.Register(Data);
                TempData["Message"] = Data.Account;
                HttpContext.Session["Account"] = Data.Account;
                return RedirectToAction("EmailValidate", "Home");
            }
        }
        
        public ActionResult EmailValidate()
         {
             string Account = (String)TempData["Message"];
             string vs = memService.SendValidateCode(Account);
             ViewData["Message"] = vs;
             ViewData["Account"] = Account;
             HttpContext.Session["Message"] = vs;
             return View();
         }
         [HttpPost]
        
         public ActionResult EmailValidate(string validate)
         {
             string Account = (String)TempData["Message"];
             string vs ="";
             Member data = new Member();
             if (Account != null)
             {
                 vs = memService.SendValidateCode(Account);
                 memService.WriteValidateCode(Account, vs);
                 data = memService.GetDataByAccount(Account);
                 return View(data);
             }
             else
             {
                 Account = (string)HttpContext.Session["Account"];
                 data = memService.GetDataByAccount(Account);
                 string vss = (string)HttpContext.Session["Message"];
                 vs = data.AuthCode;
                 if (vss.Equals(validate))
                 {

                     return RedirectToAction("Login", "Home");
                 }
                 return RedirectToAction("Register", "Home");
             }
         }
        public ActionResult AddGoup()
        {
            Groups g = new Groups();
            g=gService.getGroups(User.Identity.Name);
            return View(g);
        }
        public ActionResult CreateGroup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateGroup(string Name)
        {
            Group g = new Group();
            g.GroupName = Name;
            gService.addGroup(g);
            int  id = gService.getIdbyGroupName(Name);
            gService.addGroupCreater(id, User.Identity.Name);
            return RedirectToAction("AddGroupMem",new { id = id, name = Name });
        }
        public ActionResult AddGroupMem(int id,string name)
        {
            List<Friend> f = new List<Friend>();
            f = friService.GetDataList(User.Identity.Name);
            List<Member> lm = new List<Member>();
            Member m = new Member();
            for(int i=0;i<f.Count;i++)
            {
                if(f[i].State==3)
                {
                    if (f[i].Account_A == User.Identity.Name)
                    {
                        m.Name = f[i].Account_B;
                        m.Account = f[i].Account_B;
                    }
                    else
                    {
                        m.Name = f[i].Account_A;
                        m.Account = f[i].Account_A;
                    }
                }
                lm.Add(m);
            }
            MemViewModel mv = new MemViewModel();
            mv.Friend = lm;
            mv.GroupId = id;
            mv.GroupName = name;
            return View(mv);
        }
        public ActionResult SearchGoup(string Name)
        {
            GroupInfos gf = new GroupInfos();
            gf.g=gService.getGroupInfo(Name);
            return View(gf);
        }

        public ActionResult AddArticle()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult AddArticle(ArticleImage Data)
        {
            if (Data.AImage != null)
            {
                string filename = Path.GetFileName(Data.AImage.FileName);
                string Url = Path.Combine(Server.MapPath("~/Images/"), filename);
                Data.AImage.SaveAs(Url);
                Data.article.Image = filename;
                Data.article.Account = User.Identity.Name;
                Data.article.CreateTime = DateTime.Now;
                artService.InsertArticle(Data.article);
                return RedirectToAction("FreindShow");
            }
            return View(Data);
           
        }
        public ActionResult Add(ItemCreateViewModel Data)
        {
            if(Data.ItemImage!=null)
            {
                string filename = Path.GetFileName(Data.ItemImage.FileName);
                string Url = Path.Combine(Server.MapPath("~/Images/"), filename);
                Data.ItemImage.SaveAs(Url);
                Data.NewData.Image = filename;
                
                itemService.Insert(Data.NewData);
                return RedirectToAction("Index");
            }
            return View(Data);
        }
       
        
        public ActionResult PersonalData(string Name)
        {
            StartViewModel st = new StartViewModel();
            List<Start> ss = new List<Start>();
            Start s = new Start();
            s.Friend = Name;
            ss.Add(s);
            ss = artService.GetAllArticle(ss);
            st.StartInfo = ss;
            return View(st);
        }
        public ActionResult ShowMyFriend()
        {
            List<Friend> f = new List<Friend>();
            FCon fc = new FCon();
            fc = friService.GetAllFriend(User.Identity.Name);
            List<Image> Im = new List<Image>();
            Image g = new Image();
            for (int i=0;i<fc.Fc.Count;i++)
            {
                g.Account = fc.Fc[i].Account;
                g.Name = fc.Fc[i].Account;
                Im.Add(g);
            }
            for(int i=0;i<Im.Count;i++)
            {
                g = imService.GetImage(Im[i].Account);
                if (g != null)
                    Im[i] = g;
            }
            ImageViewModel iv = new ImageViewModel();
            iv.Images = Im;
            return View(iv);
        }
        public ActionResult ShowPFriend(string Account)
        {
            List<Friend> f = new List<Friend>();
            FCon fc = new FCon();
            fc = friService.GetAllFriend(Account);

            List<int> s = new List<int>();
            PFreindViewModel pf = new PFreindViewModel();
            PFriend p = new PFriend();
            int j = 0;
            if (fc.Fc != null)
            {
                for (int i = 0; i < fc.Fc.Count; i++)
                {
                    p.Friend = fc.Fc[i].Account;
                    if (fc.Fc[i].Account.Equals(User.Identity.Name) || fc.Fc[i].Account.Equals(User.Identity.Name))
                    {
                        j = f[i].State;
                    }
                    else
                    {
                        j = 0;
                    }
                    p.State = j;
                    pf.Fs.Add(p);
                }
                List<Image> Im = new List<Image>();
                Image g = new Image();
                Image g1 = new Image();
                for (int i = 0; i < fc.Fc.Count; i++)
                {
                    g.Account = fc.Fc[i].Account;
                    g.Name = fc.Fc[i].Account;
                    g1 = imService.GetImage(fc.Fc[i].Account);
                    if (g1 != null)
                        g = g1;
                    pf.Fs[i].FImage = g;
                }
            }
            pf.Name = Account;
            return View(pf);
        }
        public ActionResult ShowSelfArticle()
        {
            StartViewModel st = new StartViewModel();
            List<Start> ss = new List<Start>();
            Start s = new Start();
            s.Friend = User.Identity.Name;
            ss.Add(s);
            ss = artService.GetAllArticle(ss);
            st.StartInfo = ss;
            return View(st);
        }
        public ActionResult ShowArticle(ArticleViewModel art)
        {
            /* ViewModels.ArticleAndMessage am = new ViewModels.ArticleAndMessage();
             am.Art = art.Data;*/
            List<Message> m = new List<Message>();
            MessageViewModel mv = new MessageViewModel();
            mv.Data = m;
           /* m = msgService.GetAllData(art);
            am.Msg = m;
            am.Index = msgService.calIndex(mv);*/
            return View();
        }

        public ActionResult Show()
        {
            ItemDetailViewModel Data = new ItemDetailViewModel();
            Data.Data = itemService.GetDataList();
            return View(Data);
        }
        public ActionResult AddImage()
        {
            List<Image> m = new List<Image>();
            ImageViewModel g = new ImageViewModel();
            m =  imService.getAllImage(User.Identity.Name);
            g.Images = m;
            return View(g);
        }
        [HttpPost]
        public ActionResult AddImage(ImageSelfViewModel Data)
        {

            if (Data.ImageA != null)
            {
                string filename = Path.GetFileName(Data.ImageA.FileName);
                string Url = Path.Combine(Server.MapPath("~/Images/"), filename);
                Image g = new Image();
                Data.ImageA.SaveAs(Url);
                g.A_Image = filename;
                g.Account = User.Identity.Name;
                imService.InsertImages(g);
            }
            return View(Data);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}