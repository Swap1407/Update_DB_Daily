using Castle.Core.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Update_DB_Daily.ServiceLayer;
using Update_DB_Daily.Models;
using static Update_DB_Daily.ServiceLayer.MailService;
using System.Net.Mail;

namespace Update_DB_Daily_Test
{
    [TestFixture]
    public class MailServiceTest
    {
        //private Microsoft.Extensions.Configuration.IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection().Build();

        private MailService _mailService;
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<ILogger<MailService>> _mockLogger;
        private MailModel _mockmailModel;
        private readonly string _mailFrom = "swapnilsavat@gmail.com";
        private readonly string _mailTO = "swapnilssawat@gmail.com";
        private readonly string _hostName = "smtp.gmail.com";
        private readonly string _password = "password";

        [SetUp]
        public void init()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<MailService>>();
            /*_mockmailModel = new MailModel() {  TO = MailTO, HostName = hostName, Password = password};
            _mailService = new MailService(_mockLogger.Object, (Microsoft.Extensions.Configuration.IConfiguration)_mockConfiguration.Object,_mockmailModel);*/   
        }
        // we write test cases for all possinle scenarios not only for exceptions

        [Test]
        public void GivenSubjectAndBodyOfMailHavingNullFromAddress_ThenfunctionshouldReturnValueCannotBeNullException()
        {
            _mockmailModel = new MailModel() { To = _mailTO, HostName = _hostName, Password = _password };
            _mailService = new MailService(_mockLogger.Object, (Microsoft.Extensions.Configuration.IConfiguration)_mockConfiguration.Object, _mockmailModel);
            var status = 0;
            var exception = Assert.Throws<ArgumentNullException>(() => _mailService.SendMail( status));
            Assert.That(exception.Message, Is.EqualTo("Value cannot be null. (Parameter 'from')"));
        }
        [Test]
        public void GivenSubjectAndBodyOfMailHavingNullToAddress_ThenfunctionshouldReturnValueCannotBeNullException()
        {
            _mockmailModel = new MailModel() { From = _mailFrom, HostName = _hostName, Password = _password };
            _mailService = new MailService(_mockLogger.Object, (Microsoft.Extensions.Configuration.IConfiguration)_mockConfiguration.Object, _mockmailModel);
            var status = 0;
            var exception = Assert.Throws<ArgumentNullException>(() => _mailService.SendMail(status));
            Assert.That(exception.Message, Is.EqualTo("Value cannot be null. (Parameter 'to')"));
        }
        [Test]
        public void GivenSubjectAndBodyOfMailHavingWrongStatus_ThenfunctionshouldReturnIndexOutOfRangeException()
        {
            _mockmailModel = new MailModel() { From = _mailFrom, To = _mailTO, HostName = _hostName, Password = _password };
            _mailService = new MailService(_mockLogger.Object, (Microsoft.Extensions.Configuration.IConfiguration)_mockConfiguration.Object, _mockmailModel);
            var status = 2;
            var exception = Assert.Throws<IndexOutOfRangeException>(() => _mailService.SendMail(status));
            Assert.That(exception.Message, Is.EqualTo("Index was outside the bounds of the array."));
        }
        
        /*
        [Test]
        public void GivenStatusOfMail_ThenfunctionshouldHaveSuccessSubject()
        {
            _mockmailModel = new MailModel() { From = _mailFrom, To = _mailTO, HostName = _hostName, Password = _password };
            _mailService = new MailService(_mockLogger.Object, (Microsoft.Extensions.Configuration.IConfiguration)_mockConfiguration.Object, _mockmailModel);
            var status = 0;
            _mailService.SendMail(status);
            string[] taskstatus = Status.GetNames(typeof(Status));
            var Subject = "Data Insertion: " + taskstatus[status];
            Assert.That(Subject, Is.EqualTo("Data Insertion: Successful"));
        }
        */
    }
}
