
using MailKit.Security;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using Update_DB_Daily.Models;

namespace Update_DB_Daily.ServiceLayer
{
    public class MailService : IMailService
    {
        private readonly ILogger<MailService> _logger;
        private readonly IConfiguration _configuration;
        private readonly MailModel _mailModel;

        public MailService(ILogger<MailService> logger, IConfiguration configuration,MailModel mailModel)
        {
            _logger = logger;
            _configuration = configuration;
            _mailModel = mailModel;
        }

        //you should declare this enum in separate file as it will be referenced from different files
        //It is a good practice to assign numbers to enum values
       // public enum Status{Successful,Failed};

        public void SendMail(int status, List<Project> errors)
        {
            try
            {
                _logger.LogInformation("Mail sending started From: "+ _mailModel.From+" To: "+ _mailModel.To);
                using (var mailmessage = new MailMessage(_mailModel.From, _mailModel.To))
                {
                    mailmessage.Subject = "Import  "+ status;
                    switch(status)
                    {
                        case MailSubject.Successful: mailmessage.Body = "";
                            break;
                        
                    }
                    mailmessage.Body = "Data insertion to database" + status + "at:" + DateTime.Now.ToString("hh:mm:ss tt") ;
                    mailmessage.IsBodyHtml = false;

                    using (var smtpClient = new SmtpClient())
                    {
                        smtpClient.Host = _mailModel.HostName;
                        smtpClient.Port = 587;
                        smtpClient.UseDefaultCredentials = false;
                        var NetworkCred = new NetworkCredential(_mailModel.From, _mailModel.Password);
                        smtpClient.Credentials = NetworkCred;
                        smtpClient.EnableSsl = true;
                        smtpClient.Send(mailmessage);
                        _logger.LogInformation("Mail sent successfully...");
                    }
                }
            }
            catch (Exception)
            {
                _logger.LogInformation("Email sending failed");
                throw ;
            }
            
        }
    }
}
