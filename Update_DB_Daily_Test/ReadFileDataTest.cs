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
namespace Update_DB_Daily_Test
{
    public class ReadFileDataTest
    {
        private ReadFileData _readFileData;
        private Mock<ILogger<ReadFileData>> _mockLogger;
        private Mock<IMailService> _mockmailService;
        private Mock<IProjectRepository> _mockprojectRepository;


        [SetUp]
        public void init()
        {
            _mockLogger = new Mock<ILogger<ReadFileData>>();
            _mockmailService = new Mock<IMailService>();
            _mockprojectRepository = new Mock<IProjectRepository>();
        }

        // You should write some more unit tests like insertDB is called when everything is okay. Exception raised. Mail sent properly

        [Test]
        public void GivenfilePathwithNullValue_ThenfunctionshouldReturnValueCannotBeNullException()
        {
            _readFileData = new ReadFileData(_mockLogger.Object,  _mockmailService.Object, _mockprojectRepository.Object);
            //string is a reference type it is implicitly nullable
            // this exception is not handled by you in code. You should handle all the excpetions
            // we write unit test for only handled exception
            var exception = Assert.Throws<ArgumentNullException>(() => _readFileData.ReadFile(null));
            Assert.That(exception.Message, Is.EqualTo("Value cannot be null. (Parameter 'path')"));
        }
        [Test]
        public void GivenAllCorrectValues_ThenfunctionshouldHaveStatusValue0WhichisSuccessMail()
        {
            _readFileData = new ReadFileData(_mockLogger.Object, _mockmailService.Object, _mockprojectRepository.Object);
            string filePath = @"C:\Users\swapnil.siddheshwar\Documents\C# Tasks\projectsdata.csv";
            _readFileData.ReadFile(filePath);
            Assert.That(_readFileData._status, Is.EqualTo(0));
        }
        [Test]
        public void GivenAllCorrectValues_ThenfunctionshouldHaveStatusValue1WhichisFailureMail()
        {
            _readFileData = new ReadFileData(_mockLogger.Object, _mockmailService.Object, _mockprojectRepository.Object);
            //string filePath = @"C:\Users\swapnil.siddheshwar\Documents\C# Tasks\projectsdata.csv";
            
            _readFileData.ReadFile(null);
            Assert.That(_readFileData._status, Is.EqualTo(1));
        }
    }
}
