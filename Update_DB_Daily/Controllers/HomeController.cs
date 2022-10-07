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
        private readonly string _hours;
        private readonly string _minutes;

        public HomeController(ILogger<HomeController> logger, IReadFileData readFileData,IConfiguration configuration)
        {
            _logger = logger;
            _readFileData = readFileData;
            _configuration = configuration;
            _filePath = _configuration.GetConnectionString("FilePath");
            var section = _configuration.GetSection("ScheduleJobTime");
            _hours = section["Hour"];
            _minutes = section["Minutes"];
        }

        public IActionResult Index()
        {
            try
            {
                _logger.LogInformation("Recurring job restart at"+_hours + "  " + _minutes);
                int hh = int.Parse(_hours);
                int mm = int.Parse(_minutes);
                //you should also define timing for job's execution, read it from appSettings.json
                RecurringJob.AddOrUpdate(() => _readFileData.ReadFile(_filePath), Cron.Minutely);
                //RecurringJob.AddOrUpdate(() => _readFileData.ReadFile(_filePath), Cron.Daily(hh,mm));
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