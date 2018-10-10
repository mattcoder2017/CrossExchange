using System.Threading.Tasks;
using XOProject.Controller;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace XOProject.Tests
{
    public class TradeControllerTests
    {
        private readonly Mock<ITradeRepository> _tradeRepositoryMock = new Mock<ITradeRepository>();

        private readonly TradeController _tradeController;

        public TradeControllerTests()
        {
            _tradeController = new TradeController(null, _tradeRepositoryMock.Object, null);
        }

        [Test]
        public async Task GetAnalysis_ShouldReturnCalulatedData()
        {
            // Arrange
                    // Seeding the test data with both BUY and SELL data presents
           var returnTrade = new Trade [10]
            {
               new Trade { Id = 0, Action = TradeAnalysis.ACTION_BUY, NoOfShares = 10, PortfolioId = 1, Price = 10, Symbol = "CBI" },
               new Trade { Id = 0, Action = TradeAnalysis.ACTION_BUY, NoOfShares = 20, PortfolioId = 1, Price = 10, Symbol = "CBI" },
               new Trade { Id = 0, Action = TradeAnalysis.ACTION_BUY, NoOfShares = 30, PortfolioId = 1, Price = 10, Symbol = "CBI" },
               new Trade { Id = 0, Action = TradeAnalysis.ACTION_BUY, NoOfShares = 40, PortfolioId = 1, Price = 10, Symbol = "CBI" },
               new Trade { Id = 0, Action = TradeAnalysis.ACTION_BUY, NoOfShares = 50, PortfolioId = 1, Price = 10, Symbol = "CBI" },
               new Trade { Id = 0, Action = TradeAnalysis.ACTION_SELL, NoOfShares = 100, PortfolioId = 1, Price = 10, Symbol = "CBI" },
               new Trade { Id = 0, Action = TradeAnalysis.ACTION_SELL, NoOfShares = 200, PortfolioId = 1, Price = 10, Symbol = "CBI" },
               new Trade { Id = 0, Action = TradeAnalysis.ACTION_SELL, NoOfShares = 300, PortfolioId = 1, Price = 10, Symbol = "CBI" },
               new Trade { Id = 0, Action = TradeAnalysis.ACTION_SELL, NoOfShares = 400, PortfolioId = 1, Price = 10, Symbol = "CBI" },
               new Trade { Id = 0, Action = TradeAnalysis.ACTION_SELL, NoOfShares = 500, PortfolioId = 1, Price = 10, Symbol = "CBI" }

            };
            _tradeRepositoryMock.Setup(i => i.Query()).Returns(returnTrade.AsQueryable());
            // Act
            var result =  _tradeController.GetAnalysis("CBI").GetAwaiter().GetResult() as OkObjectResult;
            var list = result.Value as List<TradeAnalysis>;

            // Assert
            Assert.NotNull(list);
            Assert.AreEqual(2, list.Count);       // return 2 rows each with different group
            Assert.AreEqual(50, list[0].Maximum);  // Acquire the BUYs to verify calculated data  
            Assert.AreEqual(10, list[0].Minimum);
            Assert.AreEqual(30, list[0].Average);
            Assert.AreEqual(150, list[0].Sum);
        }

        [Test]
        public async Task GetAnalysis_NoSellData_ShouldReturnOnly1RowForBuyData()
        {
            //Arrange
                 // Seeding the test data with only BUY data presents
            var returnTrade1 = new Trade[5]
             {
               new Trade { Id = 0, Action = TradeAnalysis.ACTION_BUY, NoOfShares = 10, PortfolioId = 1, Price = 10, Symbol = "CBI" },
               new Trade { Id = 0, Action = TradeAnalysis.ACTION_BUY, NoOfShares = 20, PortfolioId = 1, Price = 10, Symbol = "CBI" },
               new Trade { Id = 0, Action = TradeAnalysis.ACTION_BUY, NoOfShares = 30, PortfolioId = 1, Price = 10, Symbol = "CBI" },
               new Trade { Id = 0, Action = TradeAnalysis.ACTION_BUY, NoOfShares = 40, PortfolioId = 1, Price = 10, Symbol = "CBI" },
               new Trade { Id = 0, Action = TradeAnalysis.ACTION_BUY, NoOfShares = 50, PortfolioId = 1, Price = 10, Symbol = "CBI" }
            
             };
            _tradeRepositoryMock.Setup(i => i.Query()).Returns(returnTrade1.AsQueryable());
            //Act
            var result = _tradeController.GetAnalysis("CBI").GetAwaiter().GetResult() as OkObjectResult;
            var list = result.Value as List<TradeAnalysis>;
            //Assert
            Assert.NotNull(list);
            Assert.AreEqual(1, list.Count);       // return 1 row each with different group
        }

        [Test]
        public async Task GetAnalysis_NoMatchingData_ShouldReturnNotFound()
        {
            //Arrange
                 // Seeding the test data with only BUY data presents
            var returnTrade1 = new Trade[5]
             {
               new Trade { Id = 0, Action = TradeAnalysis.ACTION_BUY, NoOfShares = 10, PortfolioId = 1, Price = 10, Symbol = "CBI" },
               new Trade { Id = 0, Action = TradeAnalysis.ACTION_BUY, NoOfShares = 20, PortfolioId = 1, Price = 10, Symbol = "CBI" },
               new Trade { Id = 0, Action = TradeAnalysis.ACTION_BUY, NoOfShares = 30, PortfolioId = 1, Price = 10, Symbol = "CBI" },
               new Trade { Id = 0, Action = TradeAnalysis.ACTION_BUY, NoOfShares = 40, PortfolioId = 1, Price = 10, Symbol = "CBI" },
               new Trade { Id = 0, Action = TradeAnalysis.ACTION_BUY, NoOfShares = 50, PortfolioId = 1, Price = 10, Symbol = "CBI" }
            
             };
            _tradeRepositoryMock.Setup(i => i.Query()).Returns(returnTrade1.AsQueryable());
            //Act
            var result = _tradeController.GetAnalysis("REL").GetAwaiter().GetResult() as NotFoundResult;
           //Assert
            Assert.AreEqual(404, result.StatusCode);
           
        }
    }
}
