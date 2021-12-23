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
    public class FundBusinessLayerMockTest : IClassFixture<FundHandlerFixture>
    {
        FundHandlerFixture _fixture;

        public FundBusinessLayerMockTest(FundHandlerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ShouldSuccesfullyAddFunds()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();
            double expectedFunds = 10020;
            var users = new List<User>()
            {
                new User {
                    UserId = 1,
                    UserName = "ABC",
                    FundsAvailable = 10000
                }
            };
            setup._mockUserRepo.Setup((c) => c.GetUsers()).Returns(users);

            setup._mockUserRepo.Setup((c) => c.AddFunds(It.IsAny<int>(), It.IsAny<double>())).Returns(expectedFunds);
            var manager = setup.GetFundHandler();
            var funds = manager.AddFunds("ABC", 20);
            Assert.Equal(expectedFunds, funds);

        }

        [Fact]
        public void ShouldSuccesfullyAddFundsAfterDeductingCharges()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();
            double expectedFunds = 199100;
            var users = new List<User>()
            {
                new User {
                    UserId = 1,
                    UserName = "ABC",
                    FundsAvailable = 100
                }
            };
            setup._mockUserRepo.Setup((c) => c.GetUsers()).Returns(users);

            setup._mockUserRepo.Setup((c) => c.AddFunds(It.IsAny<int>(), It.IsAny<double>())).Returns(expectedFunds);
            var manager = setup.GetFundHandler();
            var funds = manager.AddFunds("ABC", 200000);
            Assert.Equal(expectedFunds, funds);

        }

        [Fact]
        public void ShouldSuccesfullyWithDrawFunds()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();
            double expectedFunds = 9980;
            var users = new List<User>()
            {
                new User {
                    UserId = 1,
                    UserName = "ABC",
                    FundsAvailable = 10000
                }
            };
            setup._mockUserRepo.Setup((c) => c.GetUsers()).Returns(users);

            setup._mockUserRepo.Setup((c) => c.WithDrawFunds(It.IsAny<int>(), It.IsAny<double>())).Returns(expectedFunds);
            var manager = setup.GetFundHandler();
            var funds = manager.WithDrawFunds("ABC", 20);
            Assert.Equal(expectedFunds, funds);
        }

        [Fact]
        public void ShouldShowExceptionIfWithdrawHigherAmountThanOwned()
        {
            //Arrange
            var setup = _fixture.GetNewInstance();
            double expectedFunds = 9980;
            var users = new List<User>()
            {
                new User {
                    UserId = 1,
                    UserName = "ABC",
                    FundsAvailable = 10000
                }
            };
            setup._mockUserRepo.Setup((c) => c.GetUsers()).Returns(users);
            setup._mockUserRepo.Setup((c) => c.WithDrawFunds(It.IsAny<int>(), It.IsAny<double>())).Returns(expectedFunds);
            var manager = setup.GetFundHandler();
            var exception = Assert.Throws<InsufficientFundsException>(() =>
            {
                var funds = manager.WithDrawFunds("ABC", 20000);
            });

            Mock.VerifyAll();
        }
    }
    public class FundHandlerFixture
    {
        public SetupFundHandlerDependency GetNewInstance()
        {
            return new SetupFundHandlerDependency();
        }
    }

    public class SetupFundHandlerDependency
    {
        private IFundHandler _fundHandler;

        public Mock<IUserRepository> _mockUserRepo { get; set; } = new Mock<IUserRepository>();
        public IFundHandler GetFundHandler()
        {
            _fundHandler =
                new FundHandler(
                    _mockUserRepo.Object);
            return _fundHandler;
        }
    }


}
