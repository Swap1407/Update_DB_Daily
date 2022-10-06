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
        private Mock<IInsertFileDataToDB> _mockinsertFileDataToDB;


        [SetUp]
        public void init()
        {
            _mockLogger = new Mock<ILogger<ReadFileData>>();
            _mockmailService = new Mock<IMailService>();
            _mockinsertFileDataToDB = new Mock<IInsertFileDataToDB>();
        }


        [Test]
        public void GivenfilePathwithNullValue_ThenfunctionshouldReturnValueCannotBeNullException()
        {
            _readFileData = new ReadFileData(_mockLogger.Object,  _mockmailService.Object, _mockinsertFileDataToDB.Object);
            string? filePath = null;
            var exception = Assert.Throws<ArgumentNullException>(() => _readFileData.ReadFile(filePath));
            Assert.That(exception.Message, Is.EqualTo("Value cannot be null. (Parameter 'path')"));
        }
    }
}
