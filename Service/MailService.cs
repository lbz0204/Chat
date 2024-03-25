using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Windows;

namespace test.Service
{
    public class MailService
    {
        private string gmail_account = "power7210480@gmail.com";
        private string gmail_password = "zvwbrfpfmnednonr";
        private string gmail_mail = " ";
        public string GetValidateCode()
        {
            string[] Code = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            string ValidateCode = string.Empty;
            Random rd = new Random();
            for (int i = 0; i < 10; i++)
            {
                ValidateCode += Code[rd.Next(Code.Count())];
            }
            return ValidateCode;
        }
        public void SendRegisterMail(string MailBody, string ToEmail)
        {
              MailMessage mail = new MailMessage();
                mail.From = new MailAddress("power7210480@gmail.com", "驗證信", System.Text.Encoding.UTF8);
                mail.To.Add(ToEmail);
                mail.Priority = MailPriority.Normal;
                mail.Subject = "會員註冊確認信";
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Body = "<h1>" + MailBody + "</h1>";
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                SmtpServer.Credentials = new System.Net.NetworkCredential(gmail_account, gmail_password);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                SmtpServer.Dispose();
             
        }
    }
}