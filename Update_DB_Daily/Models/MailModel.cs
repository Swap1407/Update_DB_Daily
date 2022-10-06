using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Update_DB_Daily.Models
{
    public class MailModel
    {
        public string FROM { get; set; }
        public string TO { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
    }
}
