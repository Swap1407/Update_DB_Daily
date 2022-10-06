using Hangfire;
using MailKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Update_DB_Daily.ServiceLayer;
using Update_DB_Daily.Data;
using Update_DB_Daily.Models;

namespace Update_DB_Daily.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IReadFileData _readFileData;
        private readonly string _filePath;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IReadFileData readFileData,IConfiguration configuration)
        {
            _logger = logger;
            _readFileData = readFileData;
            _configuration = configuration;
            _filePath = _configuration.GetConnectionString("FilePath");
        }

        public IActionResult Index()
        {
            try
            {
                _logger.LogInformation("Recurring job started");
                //you should also define timing for job's execution, read it from appSettings.json
                RecurringJob.AddOrUpdate(() => _readFileData.ReadFile(_filePath), Cron.Daily);
                _logger.LogInformation("File data insertion task done and Mail sent");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("File data insertion failed"+ex.Message);
                return View("Error");
            }
            return View();
        }

    }
}