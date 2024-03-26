Model-Member會員基本資料
      Image 大頭照
      Article會員貼文
      Friend會員好友
      Message貼文留言
      Conversation會員跟好友的對話
      Agree文章按讚
      ArticleAndMessage: Article        貼文
                         List<Agree>    貼文的讚
                         List<Message>  貼文的留言
      Group群組
ViewModels-MemViewModel      List<Member>
           ImageViewModel    List<Image>
           MessageViewModel  List<Message>
           FriendAndCon      Account              使用者
                             Friend               朋友
                             List<Conversation>   對話紀錄
    
          
Service-MemberDBService管理會員資料
        MailService會員email驗證
        ImageDBService管理會員的大頭照 
        ArticleDBService-管理會員個人文章
        FriendDBService-管理會員好友
        MessageDBService-管理貼文留言
        ConversationDBService-管理會員跟好友間的對話 
        GroupDBService-管理各群組
Login()           登入            MemberDBService 
Register()        註冊            MemberDBService 
EmailValidate()   電子郵件驗證     MemberDBService 

Search()          搜索好友        MemberDBService FriendServuce
AddImage()        上傳圖片        ImageService
FriendShow()      所有朋友        FreindService ImageService
ShowMyFriend()    顯示使用者的朋友 ImageService
ShowPFriend()     顯示好友的朋友   FriendDBService
ShowSelfArticle() 顯示自己的文章   ArticleDBService
ShowFArticle()    顯示好友的文章   FriendService 
Uploadp()         上傳大頭照       ImageService

AddArticle()      新增文章         ImageService
AddGroup()        加入群組         GroupDBService
CreateGroup()     創造群組         GroupDBService
AddGroupMem()     加入群組成員     FriendDBService
SearchGroup()     搜尋好友         GroupDBService


