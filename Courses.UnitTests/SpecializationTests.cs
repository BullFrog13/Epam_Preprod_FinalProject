using Courses.BLL.Services;
using Courses.DAL.Entities;
using Courses.DAL.Interfaces;
using NUnit.Framework;
using System.Collections.Generic;
using Courses.BLL.DTO;
using Assert = NUnit.Framework.Assert;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace Courses.UnitTests
{
    [TestFixture]
    public class SpecializationTests
    {
        [Test]
        [TestCase(null, null)]
        [TestCase(null, "1")]
        [TestCase("1", null)]
        public void CreateSpecializationValidationTest(string name, string description)
        {
            //Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Specializations.Create(It.IsAny<Specialization>()));

            //Act
            var service = new SpecializationService(mock.Object);

            //Assert
            Assert.Throws<ValidationException>(() => service.Create(
                new SpecializationDto {Name = name, Description = description}));
        }


        [Test]
        [TestCase(null)]
        [TestCase(-5)]
        public void DeleteSpecializationTest(int? id)
        {
            //Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Specializations.Get(-1)).Returns((Specialization)null);

            //Act
            var service = new SpecializationService(mock.Object);

            //Assert
            Assert.Throws<ValidationException>(() => service.DeleteSpecialization(id));
        }

        [Test]
        public void GetAllSpecializationsTest()
        {
            //Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Specializations.GetAll()).Returns(It.IsAny<List<Specialization>>);

            //Act
            var service = new SpecializationService(mock.Object);

            var list = new List<SpecializationDto>();

            Assert.AreSame(list.GetType(), service.GetSpecializations().GetType());
        }
    }
}
