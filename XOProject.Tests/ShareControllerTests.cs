using System;
using System.Threading.Tasks;
using XOProject.Controller;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace XOProject.Tests
{
    public class ShareControllerTests
    {
        private readonly Mock<IShareRepository> _shareRepositoryMock = new Mock<IShareRepository>();
        private readonly ShareController _shareController;
      
        public ShareControllerTests()
        {
            _shareController = new ShareController(_shareRepositoryMock.Object);
        }

        [Test]
        public async Task Get_NoMatchingData_ShouldReturnNotFound()
        {
            // Arrange
            var hourRates = new List<HourlyShareRate>() {
                 new HourlyShareRate {
                    Symbol = "CBI",
                    Rate = 330.0M,
                    TimeStamp = new DateTime(2019, 08, 17, 5, 0, 0)
                  },
                  new HourlyShareRate {
                    Symbol = "CBI",
                    Rate = 130.0M,
                    TimeStamp = new DateTime(2020, 08, 17, 5, 0, 0)
                  },
                  new HourlyShareRate {
                    Symbol = "CBI",
                    Rate = 430.0M,
                    TimeStamp = new DateTime(2018, 08, 17, 5, 0, 0)
                 }
            };
            var mockSet = new Mock<DbSet<HourlyShareRate>>();
            mockSet.MockDbSet<HourlyShareRate>(hourRates);

            var mockContext = new Mock<ExchangeContext>();
            var mockRepository = new Mock<ShareRepository>(mockContext.Object);
            var shareController = new ShareController(mockRepository.Object);

            mockContext.Setup(i => i.Set<HourlyShareRate>()).Returns(mockSet.Object);

            // Act
            var result = shareController.Get("CEL").GetAwaiter().GetResult() as NotFoundResult;

            // Assert
            Assert.AreEqual(404, result.StatusCode);
            }

        [Test]
        public async Task Get_FoundMatchingData_ShouldReturnOk()
        {
            // Arrange
            var hourRates = new List<HourlyShareRate>() {
                    new HourlyShareRate {
                    Symbol = "CBI",
                    Rate = 430.0M,
                    TimeStamp = new DateTime(2018, 08, 17, 5, 0, 0)
                 }
            };
            var mockSet = new Mock<DbSet<HourlyShareRate>>();
            mockSet.MockDbSet<HourlyShareRate>(hourRates);

            var mockContext = new Mock<ExchangeContext>();
            var mockRepository = new Mock<ShareRepository>(mockContext.Object);
            var shareController = new ShareController(mockRepository.Object);

            mockContext.Setup(i => i.Set<HourlyShareRate>()).Returns(mockSet.Object);

            // Act
            var result = shareController.Get("CBI").GetAwaiter().GetResult() as OkObjectResult;

            // Assert
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task Post_ShouldInsertHourlySharePrice()
        {
            var hourRate = new HourlyShareRate
            {
                Symbol = "CBI",
                Rate = 330.0M,
                TimeStamp = new DateTime(2018, 08, 17, 5, 0, 0)
            };

            // Arrange

            // Act
            var result = await _shareController.Post(hourRate);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }

        [Test]
        public async Task GetLatestPrice_ShouldReturnMostRecentPrice()
        {
            // Arrange
            var hourRates = new List<HourlyShareRate>() {
                 new HourlyShareRate {
                    Symbol = "CBI",
                    Rate = 330.0M,
                    TimeStamp = new DateTime(2019, 08, 17, 5, 0, 0)
                  },
                  new HourlyShareRate {
                    Symbol = "CBI",
                    Rate = 530.0M,
                    TimeStamp = new DateTime(2020, 08, 17, 5, 0, 0)
                  },
                  new HourlyShareRate {
                    Symbol = "CBI",
                    Rate = 430.0M,
                    TimeStamp = new DateTime(2018, 08, 17, 5, 0, 0)
                 }
            };
            var mockSet = new Mock<DbSet<HourlyShareRate>>();
            mockSet.MockDbSet<HourlyShareRate>(hourRates);

            var mockContext = new Mock<ExchangeContext>();
            var mockRepository = new Mock<ShareRepository>(mockContext.Object);
            var shareController = new ShareController(mockRepository.Object);

            mockContext.Setup(i => i.Set<HourlyShareRate>()).Returns(mockSet.Object);
     
            // Act
            var result = shareController.GetLatestPrice("CBI").GetAwaiter().GetResult() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(530.0M, (decimal)result.Value);

        }

        [Test]
        public async Task UpdateLatestPrice_ShouldUpdateMostRecentPrice()
        {
            // Arrange
            var hourRates = new List<HourlyShareRate>() {
                 new HourlyShareRate {
                    Symbol = "CBI",
                    Rate = 330.0M,
                    TimeStamp = new DateTime(2019, 08, 17, 5, 0, 0)
                  },
                  new HourlyShareRate {
                    Symbol = "CBI",
                    Rate = 130.0M,
                    TimeStamp = new DateTime(2020, 08, 17, 5, 0, 0)
                  },
                  new HourlyShareRate {
                    Symbol = "CBI",
                    Rate = 430.0M,
                    TimeStamp = new DateTime(2018, 08, 17, 5, 0, 0)
                 }
            };
            var mockSet = new Mock<DbSet<HourlyShareRate>>();
            mockSet.MockDbSet<HourlyShareRate>(hourRates);

            HourlyShareRate share = null;
            var mockContext = new Mock<ExchangeContext>();
            var mockRepository = new Mock<ShareRepository>(mockContext.Object);
            var shareController = new ShareController(mockRepository.Object);

            mockContext.Setup(i => i.Set<HourlyShareRate>()).Returns(mockSet.Object);
            mockRepository.Setup(i => i.UpdateAsync(It.IsAny<HourlyShareRate>())).Returns(Task.FromResult<object>(null)).Callback<HourlyShareRate>((p) => { share = p; });
            // Act

            shareController.UpdateLastPrice("CBI");
           
            // Assert
            Assert.AreEqual(new DateTime(2020, 08, 17, 5, 0, 0), share.TimeStamp);
            Assert.AreEqual(10.0M, share.Rate);

        }
    }
}
    