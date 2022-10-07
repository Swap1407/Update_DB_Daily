using CsvHelper.Configuration.Attributes;

namespace Update_DB_Daily.Models
{
    public class Project
    {
        public int ID { get; set; }
        [Index(0)]
        public string ProjectName { get; set; }
        [Index(1)]
        public string ProjectDetails { get; set; }
        
    }
}
