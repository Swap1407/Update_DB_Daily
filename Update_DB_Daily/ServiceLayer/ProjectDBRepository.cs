﻿using Update_DB_Daily.Data;
using Update_DB_Daily.Models;

namespace Update_DB_Daily.ServiceLayer
{
    public class ProjectDBRepository : IProjectRepository
    {
        private readonly ProjectDBContext _projectDBContext;
        private ILogger<ProjectDBRepository> _logger;
        public ProjectDBRepository(ProjectDBContext projectDBContext, ILogger<ProjectDBRepository> logger)
        {
            _projectDBContext = projectDBContext;
            _logger = logger;
        }

        public void AddProject(List<Project> projectList)
        {
            _logger.LogInformation("Data insertion to DB started");
            _projectDBContext.Projects.AddRange(projectList);
            _projectDBContext.SaveChanges();
            _logger.LogInformation("Data insertion to DB successful");

        }

    }
}
