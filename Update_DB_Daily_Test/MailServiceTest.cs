﻿using Castle.Core.Logging;
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
        private readonly string MailFrom = "swapnilsavat@gmail.com";
        private readonly string MailTO = "swapnilssawat@gmail.com";
        private readonly string hostName = "smtp.gmail.com";
        private readonly string password = "cxcuzibjriotvcox";

        [SetUp]
        public void init()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<MailService>>();
            /*_mockmailModel = new MailModel() {  TO = MailTO, HostName = hostName, Password = password};
            _mailService = new MailService(_mockLogger.Object, (Microsoft.Extensions.Configuration.IConfiguration)_mockConfiguration.Object,_mockmailModel);*/   
        }

        [Test]
        public void GivenSubjectAndBodyOfMailHavingNullFromAddress_ThenfunctionshouldReturnValueCannotBeNullException()
        {
            _mockmailModel = new MailModel() {FROM = null, TO = MailTO, HostName = hostName, Password = password };
            _mailService = new MailService(_mockLogger.Object, (Microsoft.Extensions.Configuration.IConfiguration)_mockConfiguration.Object, _mockmailModel);
            var subject = "Data inserted successfully";
            var body = "Data inserted to database successfully at: " + DateTime.Now.ToString("hh:mm:ss tt");
            var exception = Assert.Throws<ArgumentNullException>(() => _mailService.SendMail( subject, body));
            Assert.That(exception.Message, Is.EqualTo("Value cannot be null. (Parameter 'from')"));
        }
        [Test]
        public void GivenSubjectAndBodyOfMailHavingNullToAddress_ThenfunctionshouldReturnValueCannotBeNullException()
        {
            _mockmailModel = new MailModel() { FROM = MailFrom, HostName = hostName, Password = password };
            _mailService = new MailService(_mockLogger.Object, (Microsoft.Extensions.Configuration.IConfiguration)_mockConfiguration.Object, _mockmailModel);
            var subject = "Data inserted successfully";
            var body = "Data inserted to database successfully at: " + DateTime.Now.ToString("hh:mm:ss tt");

            var exception = Assert.Throws<ArgumentNullException>(() => _mailService.SendMail(subject, body));
            Assert.That(exception.Message, Is.EqualTo("Value cannot be null. (Parameter 'to')"));
        }
    }
}
