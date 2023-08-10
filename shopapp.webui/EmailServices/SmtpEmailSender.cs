using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace shopapp.webui.EmailServices
{
    public class SmtpEmailSender:IEmailSender
    {
        private string host;
       private int port;
      private bool enableSSL;
        private string  userName;
        private string  password;
        public SmtpEmailSender(string _host,int _port,bool _enableSSL,string _userName,string _password)
        {
            host=_host;
            port=_port;
            enableSSL=_enableSSL;
            userName=_userName;
            password=_password;
        }
        public Task SendEmailAsync(string email,string subject, string message){
                var client = new SmtpClient(host,port){
                    Credentials= new NetworkCredential(userName,password),
                    EnableSsl=enableSSL,
                };
                return client.SendMailAsync(
                    new MailMessage(userName,email,subject,message){
                        IsBodyHtml=true
                    }
                );
    
        }
    }
}