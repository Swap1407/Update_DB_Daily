using System.Data;
using System.Runtime.CompilerServices;
using Update_DB_Daily.Controllers;
using Update_DB_Daily.Data;
using Update_DB_Daily.Models;
using Update_DB_Daily.ServiceLayer;

namespace Update_DB_Daily.ServiceLayer
{
    public class ReadFileData : IReadFileData
    {
        private readonly IMailService _mailService;
        private readonly ILogger<ReadFileData> _logger;
        private readonly IProjectRepository _projectRepository;
        public int _status ;
        public enum Status{Successful,Failed};
        public ReadFileData(ILogger<ReadFileData> logger, IMailService mailService, IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
            _mailService = mailService;
            _logger = logger;
            _status = -1;
        }

        public void ReadFile( string filePath)
        {
            try
            {
                _logger.LogInformation("Data reading started");
                var projectList = new List<Project>();
                var csvFileData = File.ReadAllText(filePath);
                //use try catch in foreach also if exception occur in one row whole data should not be rejected but only that row, and it should be logged also sent in me stating error at this line of file
                foreach (var row in csvFileData.Split('\n'))
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            var project = new Project()
                            {
                                ProjectName = row.Split(",")[0],
                                ProjectDetails = row.Split(",")[1]
                            };
                            projectList.Add(project);
                        }
                    }
                    catch (Exception ex){
                        _logger.LogInformation("Exception occured"+ex.Message);
                    }
                }
                _logger.LogInformation("Data read successfully");
                _projectRepository.InsertProject(projectList);


                // use enum for mailSubjects and import status. Manage import using it's status. whether error, failed or successful
                _logger.LogInformation("Setting Success mail status to 0");
                _status = 0;
                //_mailService.SendMail(0);
                _logger.LogInformation("Success mail sent");
            }

            catch (Exception ex)
            {
                _logger.LogInformation("Data insertion failed"+ex.Message);
                _logger.LogInformation("Setting Failure mail status to 1");
                _status = 1;
                //_mailService.SendMail(1);
                throw;
            }
            finally
            {
                _logger.LogInformation("Mail sending started");
                _mailService.SendMail(_status);
            }
        }
    }
}
