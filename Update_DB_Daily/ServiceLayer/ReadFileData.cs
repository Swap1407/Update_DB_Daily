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
        private readonly IInsertFileDataToDB _insertFileDataToDB;

        public ReadFileData(ILogger<ReadFileData> logger, IMailService mailService, IInsertFileDataToDB insertFileDataToDB)
        {
            _insertFileDataToDB = insertFileDataToDB;
            _mailService = mailService;
            _logger = logger;
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
                _logger.LogInformation("Data read successfully");
                _insertFileDataToDB.InsertDataToDB(projectList);

                _logger.LogInformation("Success mail sending started");

                // use enum for mailSubjects and import status. Manage import using it's status. whether error, failed or successful
                var mailsubject = "Data insertion Successful";
                var mailbody = "Data inserted to database successfully at: "+ DateTime.Now.ToString("hh:mm:ss tt");
                _mailService.SendMail(mailsubject,mailbody);
                _logger.LogInformation("Success mail sent");
            }

            catch (Exception ex)
            {
                _logger.LogInformation("Failure mail sending started");
                var mailsubject = "Data insertion Failed";
                var mailbody = "Data not inserted to database at: " + DateTime.Now.ToString("hh:mm:ss tt") + ex.Message;
                _mailService.SendMail(mailsubject, mailbody);
                _logger.LogInformation("Failure mail sent");
                throw;
            }
        }
    }
}
