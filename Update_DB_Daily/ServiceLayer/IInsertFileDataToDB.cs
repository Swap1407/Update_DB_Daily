using Update_DB_Daily.Models;

namespace Update_DB_Daily.ServiceLayer
{
    public interface IInsertFileDataToDB
    {
        public void InsertDataToDB(List<Project> projectList);
    }
}
