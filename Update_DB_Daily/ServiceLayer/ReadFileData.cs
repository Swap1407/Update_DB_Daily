using CsvHelper;
using System;
using System.Data;
using System.Globalization;
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
        private MailSubject _mailSubject;
        public ReadFileData(ILogger<ReadFileData> logger, IMailService mailService, IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
            _mailService = mailService;
            _logger = logger;
        }

        public void ReadFile( string filePath)
        {
            var errors = new List<string>();
            var projectList = new List<Project>();
            try
            {
                _logger.LogInformation("Data reading started");
                var csvFileData = File.ReadAllText(filePath);
                var rowNumber = 0;
                
                //use try catch in foreach also if exception occur in one row whole data should not be rejected but only that row, and it should be logged also sent in me stating error at this line of file

                //try using datatable with csv reader
                foreach (var row in csvFileData.Split('\n'))
                {
                    rowNumber++;
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
                        errors.Add("Error at row number " + rowNumber + " " + ex.Message);
                        _mailSubject = MailSubject.Error;
                        _logger.LogInformation("Exception occured"+ex.Message);
                    }
                }
                
                //Use meaningful log information, and club them if coming one after other without doing any processing
                _logger.LogInformation("Data Read");
                _projectRepository.AddProject(projectList);


                // use enum for mailSubjects and import status. Manage import using it's status. whether error, failed or successful
                _logger.LogInformation("Setting Success mail status to 0");
                
                _mailSubject = MailSubject.Successful;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Data insertion failed"+ex.Message);
                _logger.LogInformation("Setting Failure mail status to 1");
                _mailSubject = MailSubject.Failed;
                throw;
            }
            finally
            {
                _logger.LogInformation("Mail sending started");
                _mailService.SendMail((int)_mailSubject, errors);
            }
        }
    }
}
