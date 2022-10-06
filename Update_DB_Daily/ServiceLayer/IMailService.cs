using Update_DB_Daily.Models;

namespace Update_DB_Daily.ServiceLayer
{
    public interface IMailService
    {
        public void SendMail( string mailsubject, string mailbody);
    }
}
