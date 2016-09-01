using System.ComponentModel.DataAnnotations;
using Courses.BLL.Services;
using Courses.DAL.Entities;
using Courses.DAL.Interfaces;
using Moq;
using NUnit.Framework;

namespace Courses.UnitTests
{
    public class JournalTests
    {
        [Test]
        [TestCase(null)]
        [TestCase(-5)]
        public void DeleteCourseTest(int? id)
        {
            //Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Journals.Get(-1)).Returns((Journal)null);

            //Act
            var service = new JournalService(mock.Object);

            //Assert
            Assert.Throws<ValidationException>(() => service.DeleteJournal(id));
        }

        [Test]
        [TestCase(null, null)]
        [TestCase("test", null)]
        [TestCase(null, 13)]
        public void CreateJournalValidationTest(string studentId, int? courseId)
        {
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Journals.Create(It.IsAny<Journal>()));

            //Act
            var service = new JournalService(mock.Object);

            //Assert
            Assert.Throws<ValidationException>(() => service.Create(studentId, courseId));
        }
    }
}
