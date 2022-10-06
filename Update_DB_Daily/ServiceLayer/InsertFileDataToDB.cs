using Update_DB_Daily.Data;
using Update_DB_Daily.Models;

namespace Update_DB_Daily.ServiceLayer
{
    public class InsertFileDataToDB : IInsertFileDataToDB
    {
        private readonly ProjectDBContext _projectDBContext;
        private ILogger<InsertFileDataToDB> _logger;
        public InsertFileDataToDB(ProjectDBContext projectDBContext, ILogger<InsertFileDataToDB> logger)
        {
            _projectDBContext = projectDBContext;
            _logger = logger;
        }

        public void InsertDataToDB(List<Project> projectList)
        {
            _logger.LogInformation("Data insertion to DB started");
            _projectDBContext.Projects.AddRange(projectList);
            _projectDBContext.SaveChanges();
            _logger.LogInformation("Data insertion to DB successful");

        }

    }
}
