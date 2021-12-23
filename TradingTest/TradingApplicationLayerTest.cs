using Moq;
using System;
using System.Collections.Generic;
using TradingApp.Controllers;
using TradingApp.Models;
using TradingBusiness;
using TradingBusiness.Contracts;
using TradingData.Models;
using Xunit;

namespace TradingTest
{
    public class TradingApplicationLayerTest:IClassFixture<TradingApplicationFixture>
    {
        TradingApplicationFixture _fixture;

        public TradingApplicationLayerTest(TradingApplicationFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void BuyTradeSuccessFully()
        {
            var setup = _fixture.GetNewInstance();
            List<Orders> orders = new List<Orders>()
            {
                 new Orders()
                {
                    OrderId = 1,
                    EquityId = 2,
                    UserId = 1,
                    QuantityPurchased = 20
                }
             };

            setup._mockEquityHandler.Setup(x => x.BuySellEquity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(orders);

            var StockController = setup.GetTradingHandler();
            //// act
            var response = StockController.BuySellEquity("ABC","USD",20);
            Assert.Equal(response.Status, Constants.Successful);
        }

        [Fact]
        public void BuyTradeUnsuccesfulIfNoOrdersReturnAfterTrading()
        {
            var setup = _fixture.GetNewInstance();
            setup._mockEquityHandler.Setup(x => x.BuySellEquity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(new List<Orders>());

            var StockController = setup.GetTradingHandler();
            //// act
            var response = StockController.BuySellEquity("ABC", "USD", 20);
            Assert.Equal(response.Status, Constants.UnSuccessful);
        }

        [Fact]
        public void BuyTradeUnsuccesfullDueToTradingClose()
        {
            var setup = _fixture.GetNewInstance();
            setup._mockEquityHandler.Setup(x => x.BuySellEquity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Throws(new TradingCloseException());

            var StockController = setup.GetTradingHandler();
            //// act
            var response = StockController.BuySellEquity("ABC", "USD", 20);
            Assert.Equal(response.Status, Constants.UnSuccessful);
        }

        [Fact]
        public void BuyTradeUnsuccesfullDueToInsuffcientFundsInUserAccount()
        {
            var setup = _fixture.GetNewInstance();
            
            setup._mockEquityHandler.Setup(x => x.BuySellEquity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Throws(new InsufficientFundsException(10));

            var StockController = setup.GetTradingHandler();
            //// act
            var response = StockController.BuySellEquity("ABC", "USD", 20);
            Assert.Equal(response.Status, Constants.UnSuccessful);
        }

        [Fact]
        public void BuyTradeUnsuccesfullDueToInsuffcientEquityHoldsInUserAccount()
        {
            var setup = _fixture.GetNewInstance();

            setup._mockEquityHandler.Setup(x => x.BuySellEquity(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Throws(new InsufficientEquityQuantityException(1,2));

            var StockController = setup.GetTradingHandler();
            //// act
            var response = StockController.BuySellEquity("ABC", "USD", 20);
            Assert.Equal(response.Status, Constants.UnSuccessful);
        }

        [Fact]
        public void FundsAddedSuccessfully()
        {
            var setup = _fixture.GetNewInstance();

            setup._mockFundHandler.Setup(x => x.AddFunds(It.IsAny<string>(), It.IsAny<double>())).Returns(3);

            var StockController = setup.GetTradingHandler();
            //// act
            var response = StockController.AddFunds("ABC", 20);
            Assert.Equal(response.Status, Constants.Successful);
        }

        [Fact]
        public void FundsAddedUnSuccessfully()
        {
            var setup = _fixture.GetNewInstance();

            setup._mockFundHandler.Setup(x => x.AddFunds(It.IsAny<string>(), It.IsAny<double>())).Returns(0);

            var StockController = setup.GetTradingHandler();
            //// act
            var response = StockController.AddFunds("ABC", 20);
            Assert.Equal(response.Status, Constants.UnSuccessful);
        }

        [Fact]
        public void FundsAddedUnSuccessfullyDueToException()
        {
            var setup = _fixture.GetNewInstance();

            setup._mockFundHandler.Setup(x => x.AddFunds(It.IsAny<string>(), It.IsAny<double>())).Throws(new InsufficientEquityQuantityException(1, 2));

            var StockController = setup.GetTradingHandler();
            //// act
            var response = StockController.AddFunds("ABC", 20);
            Assert.Equal(response.Status, Constants.UnSuccessful);
        }
    }
    public class TradingApplicationFixture
    {
        public SetupTradingApplicationDependency GetNewInstance()
        {
            return new SetupTradingApplicationDependency();
        }
    }

    public class SetupTradingApplicationDependency
    {
        private Trading _tradingApplication;

        public Mock<IEquityHandler> _mockEquityHandler { get; set; } = new Mock<IEquityHandler>();
        public Mock<IFundHandler> _mockFundHandler { get; set; } = new Mock<IFundHandler>();
        public Trading GetTradingHandler()
        {
            _tradingApplication =
                new Trading(
                    _mockEquityHandler.Object,_mockFundHandler.Object);
            return _tradingApplication;
        }
    }


}
