using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBusiness;
using TradingBusiness.Contracts;
using TradingData.Contracts;
using TradingData.Models;
using Xunit;

namespace TradingTest
{

    public class EquityHandlerFixture
    {
        public SetupEquityHandlerDependency GetNewInstance()
        {
            return new SetupEquityHandlerDependency();
        }
    }

    public class SetupEquityHandlerDependency
    {
        private IEquityHandler _equityHandler;

        public Mock<IUserRepository> _mockUserRepo { get; set; } = new Mock<IUserRepository>();
        public Mock<IEquityRepository> _mockEquityRepo { get; set; } = new Mock<IEquityRepository>();
        public Mock<IOrderRepository> _mockOrderRepo { get; set; } = new Mock<IOrderRepository>();
        public Mock<IFundHandler> _fundHandler { get; set; } = new Mock<IFundHandler>();
        public Mock<ITradingUtilWrapper> _wrapper { get; set; } = new Mock<ITradingUtilWrapper>();
        public IEquityHandler GetEquityHandler()
        {
            _equityHandler =
                new EquityHandler(
                    _mockEquityRepo.Object,
                    _mockOrderRepo.Object,
                    _mockUserRepo.Object,
                    _wrapper.Object);

            return _equityHandler;
        }
    }


    public class EquityBusinessLayerMockTest : IClassFixture<EquityHandlerFixture>
    {
        EquityHandlerFixture _fixture;

        public EquityBusinessLayerMockTest(EquityHandlerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void WhenTradeFailIfEquityDoesNotExists()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            setup._mockEquityRepo.Setup((c) => c.GetEquityByName(It.IsAny<string>())).Returns(value: null);
            setup._wrapper.Setup((c) => c.IsTradingOpen()).Returns(true);
            var manager = setup.GetEquityHandler();

            //Act
            var exception = Assert.Throws<InvalidEquityNameException>(() =>
             {
                 manager.BuySellEquity("ABC", "Stock", 20);
             });

            Mock.VerifyAll();
        }

        [Fact]
        public void WhenTradeFailIfUserDoesNotExists()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            setup._mockEquityRepo.Setup((c) => c.GetEquityByName(It.IsAny<string>())).Returns(new Equity()
            {
                Id = 1,
                EquityName = "AAPL",
                BuyPrice = 1,
                SellPrice = 1.5
            });
            setup._mockUserRepo.Setup((c) => c.GetUsers()).Returns(new List<User>()
            {
                new User {
                    UserId = 1,
                    UserName = "ABC",
                    FundsAvailable = 10000
                }
            });
            setup._wrapper.Setup((c) => c.IsTradingOpen()).Returns(true);

            var manager = setup.GetEquityHandler();

            //Act
            var exception = Assert.Throws<InvalidUserNameException>(() =>
            {
                manager.BuySellEquity("test", "AAPL", 20);
            });

            Mock.VerifyAll();
        }

        [Fact]
        public void WhenTradeFailTradingOutsideTradingHours()
        {
            var setup = _fixture.GetNewInstance();
            setup._mockEquityRepo.Setup((c) => c.GetEquityByName(It.IsAny<string>())).Returns(new Equity()
            {
                Id = 1,
                EquityName = "AAPL",
                BuyPrice = 1,
                SellPrice = 1.5
            });
            setup._mockUserRepo.Setup((c) => c.GetUsers()).Returns(new List<User>()
            {
                new User {
                    UserId = 1,
                    UserName = "ABC",
                    FundsAvailable = 10000
                }
            });
            setup._wrapper.Setup((c) => c.IsTradingOpen()).Returns(false);
            var manager = setup.GetEquityHandler();
            string username = "ABC";
            string equityName = "AAPL";
            int quantity = 10;


            //Act
            var exception = Assert.Throws<TradingCloseException>(() =>
            {
                manager.BuySellEquity(username, equityName, quantity);
            });

            Mock.VerifyAll();
        }

        [Fact]
        public void WhenTradeFailIfUserSellsEquityNotOwned()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();

            setup._mockEquityRepo.Setup((c) => c.GetEquityByName(It.IsAny<string>())).Returns(new Equity()
            {
                Id = 1,
                EquityName = "AAPL",
                BuyPrice = 1,
                SellPrice = 1.5
            });
            setup._mockUserRepo.Setup((c) => c.GetUsers()).Returns(new List<User>()
            {
                new User {
                    UserId = 1,
                    UserName = "ABC",
                    FundsAvailable = 10000
                }
            });
            setup._mockOrderRepo.Setup((c) => c.GetAllEquityRelatedToAUser(It.IsAny<int>())).Returns(new List<Orders>()
            {
                 new Orders()
                {
                    OrderId = 1,
                    EquityId = 2,
                    UserId = 1,
                    QuantityPurchased = 20
                }
             });
            setup._wrapper.Setup((c) => c.IsTradingOpen()).Returns(true);


            var manager = setup.GetEquityHandler();

            //Act
            var exception =
                 Assert.Throws<EquityNotOwnedException>(() =>
                 {
                     manager.BuySellEquity("ABC", "AAPL", -20);
                 });
            Mock.VerifyAll();
        }

        [Fact]
        public void WhenTradeFailIfUserSellMoreQuantityOfEquity()
        {
            var setup = _fixture.GetNewInstance();

            List<User> users = new List<User>()
            {
                new User {
                    UserId = 1,
                    UserName = "ABC",
                    FundsAvailable = 10000
                }
            };

            var equity = new Equity()
            {
                Id = 1,
                EquityName = "AAPL",
                BuyPrice = 1,
                SellPrice = 1.5
            };

            List<Orders> orders = new List<Orders>()
            {
                 new Orders()
                {
                    OrderId = 1,
                    EquityId = 1,
                    UserId = 1,
                    QuantityPurchased = 20
                }
             };

            setup._mockEquityRepo.Setup((c) => c.GetEquityByName(It.IsAny<string>())).Returns(equity);
            setup._mockUserRepo.Setup((c) => c.GetUsers()).Returns(users);
            setup._mockOrderRepo.Setup((c) => c.GetAllEquityRelatedToAUser(It.IsAny<int>())).Returns(orders);
            setup._wrapper.Setup((c) => c.IsTradingOpen()).Returns(true);

            var manager = setup.GetEquityHandler();

            //Act
            var exception =
                 Assert.Throws<InsufficientEquityQuantityException>(() =>
                 {
                     manager.BuySellEquity("ABC", "AAPL", -40);
                 });
            Mock.VerifyAll();
        }

        [Fact]
        public void WhenTradeFailIfUserHasInsufficientFunds()
        {
            var setup = _fixture.GetNewInstance();

            List<User> users = new List<User>()
            {
                new User {
                    UserId = 1,
                    UserName = "ABC",
                    FundsAvailable = 100
                }
            };

            var equity = new Equity()
            {
                Id = 1,
                EquityName = "AAPL",
                BuyPrice = 10,
                SellPrice = 1.5
            };

            List<Orders> orders = new List<Orders>()
            {
                 new Orders()
                {
                    OrderId = 1,
                    EquityId = 1,
                    UserId = 1,
                    QuantityPurchased = 20
                }
             };

            setup._mockEquityRepo.Setup((c) => c.GetEquityByName(It.IsAny<string>())).Returns(equity);
            setup._mockUserRepo.Setup((c) => c.GetUsers()).Returns(users);
            setup._mockOrderRepo.Setup((c) => c.GetAllEquityRelatedToAUser(It.IsAny<int>())).Returns(orders);
            setup._wrapper.Setup((c) => c.IsTradingOpen()).Returns(true);
            setup._wrapper.Setup((c) => c.UserHasEnoughFunds(It.IsAny<int>(), It.IsAny<double>())).Returns(false);


            var manager = setup.GetEquityHandler();

            //Act
            var exception =
                 Assert.Throws<InsufficientFundsException>(() =>
                 {
                     manager.BuySellEquity("ABC", "AAPL", 40);
                 });
            Mock.VerifyAll();
        }

        [Fact]
        public void WhenBuyTradeForNewEquity()
        {
            var setup = _fixture.GetNewInstance();

            List<User> users = new List<User>()
            {
                new User {
                    UserId = 1,
                    UserName = "ABC",
                    FundsAvailable = 1000
                }
            };

            var equity = new Equity()
            {
                Id = 2,
                EquityName = "USD",
                BuyPrice = 10,
                SellPrice = 1.5
            };

            List<Orders> orders = new List<Orders>()
            {
                 new Orders()
                {
                    OrderId = 1,
                    EquityId = 1,
                    UserId = 1,
                    QuantityPurchased = 20
                }
             };

            setup._mockEquityRepo.Setup((c) => c.GetEquityByName(It.IsAny<string>())).Returns(equity);
            setup._mockUserRepo.Setup((c) => c.GetUsers()).Returns(users);
            setup._mockOrderRepo.Setup((c) => c.GetAllEquityRelatedToAUser(It.IsAny<int>())).Returns(orders);
            setup._wrapper.Setup((c) => c.IsTradingOpen()).Returns(true);
            setup._wrapper.Setup((c) => c.UserHasEnoughFunds(It.IsAny<double>(), It.IsAny<double>())).Returns(true);
            setup._mockOrderRepo.Setup((c) => c.AddOrderRelatedToUser(It.IsAny<Orders>())).Callback(() => orders.Add(new Orders
            {
                EquityId = 2,
                UserId = 1,
                QuantityPurchased = 10
            }));

            var manager = setup.GetEquityHandler();

            //Act

            var response = manager.BuySellEquity("ABC", "USD", 10);
            var purChasedEquity = response.Where(c => c.EquityId == 2).FirstOrDefault();

            Assert.Equal(10, purChasedEquity.QuantityPurchased);
            Mock.VerifyAll();
        }

        [Fact]
        public void WhenBuyTradeForExisitngEquity()
        {
            var setup = _fixture.GetNewInstance();

            List<User> users = new List<User>()
            {
                new User {
                    UserId = 1,
                    UserName = "ABC",
                    FundsAvailable = 1000
                }
            };

            var equity = new Equity()
            {
                Id = 1,
                EquityName = "USD",
                BuyPrice = 10,
                SellPrice = 1.5
            };

            List<Orders> orders = new List<Orders>()
            {
                 new Orders()
                {
                    OrderId = 1,
                    EquityId = 1,
                    UserId = 1,
                    QuantityPurchased = 20
                }
             };

            setup._mockEquityRepo.Setup((c) => c.GetEquityByName(It.IsAny<string>())).Returns(equity);
            setup._mockUserRepo.Setup((c) => c.GetUsers()).Returns(users);
            setup._mockOrderRepo.Setup((c) => c.GetAllEquityRelatedToAUser(It.IsAny<int>())).Returns(orders);
            setup._wrapper.Setup((c) => c.IsTradingOpen()).Returns(true);
            setup._wrapper.Setup((c) => c.UserHasEnoughFunds(It.IsAny<double>(), It.IsAny<double>())).Returns(true);
            setup._mockOrderRepo.Setup((c) => c.UpdateExistingOrderForUser(It.IsAny<Orders>(), It.IsAny<int>())).Callback(() => orders.Where(c => c.UserId == 1).First().QuantityPurchased = 30);

            var manager = setup.GetEquityHandler();

            //Act

            var response = manager.BuySellEquity("ABC", "USD", 10);
            var purChasedEquity = response.Where(c => c.EquityId == 1).FirstOrDefault();

            Assert.Equal(30, purChasedEquity.QuantityPurchased);
            Mock.VerifyAll();
        }

        [Fact]
        public void WhenSellTradeForExisitngEquity()
        {
            var setup = _fixture.GetNewInstance();

            List<User> users = new List<User>()
            {
                new User {
                    UserId = 1,
                    UserName = "ABC",
                    FundsAvailable = 1000
                }
            };

            var equity = new Equity()
            {
                Id = 1,
                EquityName = "USD",
                BuyPrice = 10,
                SellPrice = 1.5
            };

            List<Orders> orders = new List<Orders>()
            {
                 new Orders()
                {
                    OrderId = 1,
                    EquityId = 1,
                    UserId = 1,
                    QuantityPurchased = 20
                }
             };

            setup._mockEquityRepo.Setup((c) => c.GetEquityByName(It.IsAny<string>())).Returns(equity);
            setup._mockUserRepo.Setup((c) => c.GetUsers()).Returns(users);
            setup._mockOrderRepo.Setup((c) => c.GetAllEquityRelatedToAUser(It.IsAny<int>())).Returns(orders);
            setup._wrapper.Setup((c) => c.IsTradingOpen()).Returns(true);
            setup._wrapper.Setup((c) => c.CalculateBrokerage(It.IsAny<Equity>(), It.IsAny<int>())).Returns(20);
            setup._mockOrderRepo.Setup((c) => c.UpdateExistingOrderForUser(It.IsAny<Orders>(), It.IsAny<int>())).Callback(() => orders.Where(c => c.UserId == 1).First().QuantityPurchased-=10);

            var manager = setup.GetEquityHandler();

            //Act

            var response = manager.BuySellEquity("ABC", "USD", -10);
            var purChasedEquity = response.Where(c => c.EquityId == 1).FirstOrDefault();

            Assert.Equal(10, purChasedEquity.QuantityPurchased);
            Mock.VerifyAll();
        }
    }
}
