using System;
using System.ComponentModel.DataAnnotations;
using Courses.BLL.DTO;
using Courses.BLL.Services;
using Courses.DAL.Entities;
using Courses.DAL.Interfaces;
using Moq;
using NUnit.Framework;

namespace Courses.UnitTests
{
    [TestFixture]
    public class CourseTests
    {
        [Test]
        [TestCase(null)]
        [TestCase(-5)]
        public void DeleteCourseTest(int? id)
        {
            //Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Courses.Get(-1)).Returns((Course)null);

            //Act
            var service = new CourseService(mock.Object);

            //Assert
            Assert.Throws<ValidationException>(() => service.DeleteCourse(id));
        }


        [Test]
        [TestCase(null, null, null, null, null, null)]
        [TestCase("test", null, null, null, null, null)]
        public void CreateCourseValidationTest(string name, string description, DateTime startDate,
            DateTime endDate, string tutorId, int specialization)
        {
            //Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Courses.Create(It.IsAny<Course>()));

            //Act
            var service = new CourseService(mock.Object);

            //Assert
            Assert.Throws<ValidationException>(() => service.Create(
                new CourseDto
                {
                    Name = name, Description = description,
                    StartDate = startDate, EndDate = endDate                   
                }, specialization, tutorId));
        }

        [Test]
        [TestCase(null)]
        [TestCase(-1)]
        public void AddSubscriberIdValidationTest(int? id)
        {
            //Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Courses.Get(-1)).Returns((Course)null);

            //Act
            var service = new CourseService(mock.Object);

            //Assert
            Assert.Throws<ValidationException>(() => service.AddSubscriber(id));
        }
    }
}
