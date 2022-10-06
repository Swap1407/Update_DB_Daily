using MailKit.Security;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.Net;
using System.Net.Mail;
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

        public void SendMail(string mailsubject, string mailbody)
        {
            try
            {
                _logger.LogInformation("Mail sending started From: "+ _mailModel.FROM+" To: "+ _mailModel.TO);
                using (var mailmessage = new MailMessage(_mailModel.FROM, _mailModel.TO))
                {
                    mailmessage.Subject = mailsubject;
                    mailmessage.Body = mailbody;
                    mailmessage.IsBodyHtml = false;

                    using (var smtpClient = new SmtpClient())
                    {
                        smtpClient.Host = _mailModel.HostName;
                        smtpClient.Port = 587;
                        smtpClient.UseDefaultCredentials = false;
                        var NetworkCred = new NetworkCredential(_mailModel.FROM, _mailModel.Password);
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
