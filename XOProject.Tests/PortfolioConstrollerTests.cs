using System.Threading.Tasks;
using XOProject.Controller;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace XOProject.Tests
{
    public class PortfolioControllerTests
    {
        private readonly Mock<IPortfolioRepository> _portfolioRepositoryMock = new Mock<IPortfolioRepository>();

        private readonly PortfolioController _portfolioController;

        public PortfolioControllerTests()
        {
            _portfolioController = new PortfolioController(null, null, _portfolioRepositoryMock.Object);
        }

        [Test]
        public async Task GetPortfolioInfo_ReturnOK()
        {
            // Arrange
            _portfolioRepositoryMock.Setup(i => i.GetAll()).Returns
                (new Portfolio[1] { new Portfolio { Id = 1 } }.AsQueryable());

            // Act
            var result = _portfolioController.GetPortfolioInfo(1).GetAwaiter().GetResult() as OkObjectResult;

            // Assert
            Assert.AreEqual(200, result.StatusCode);
            var resultSet = result.Value as IQueryable<Portfolio>;
           Assert.AreEqual(1, resultSet.Count());

        }

        [Test]
        public async Task Post_Portfolio_ReturnCreated()
        {
            // Arrange
               // Seeding the test entity that missing required value
            var portfolio = new Portfolio { Id = 1, Name = "test", Trade = new List<Trade>() };
           
            // Act
            var result = _portfolioController.Post(portfolio).GetAwaiter().GetResult() as CreatedResult;

            // Assert
            Assert.AreEqual(201, result.StatusCode);
           
            

        }


    }
}
