using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Update_DB_Daily.Models
{
    public class MailModel
    {
        //follow proper naming like From
        public string From { get; set; }
        public string To { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
       
    }
}
