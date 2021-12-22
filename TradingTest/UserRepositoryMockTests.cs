using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingData;
using TradingData.DBContext;
using TradingData.Models;
using TradingData.Repository;
using Xunit;

namespace TradingTest
{
    public class UserRepositoryMockTests : IClassFixture<DatabaseFixture>
    {
        DatabaseFixture fixture;
        public UserRepositoryMockTests(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void ShouldGetAllUsersFromDataBase()
        {
            using (var context = new TraderDbContext(this.fixture.option))
            {
                UserRepository repo = new UserRepository(context);
                int expectedResult = 1;
                var users = repo.GetUsers();
                Assert.Equal(expectedResult, users.Count());

            }
        }

        [Fact]
        public void ShouldGetUserByIDFromDataBase()
        {
            using (var context = new TraderDbContext(this.fixture.option))
            {
                UserRepository repo = new UserRepository(context);
                string expectedResult = "ABC";
                var users = repo.GetUserById(1);
                Assert.Equal(expectedResult, users.UserName);
            }
        }

        [Fact]
        public void ShouldAddFundsToUser()
        {
            using (var context = new TraderDbContext(this.fixture.option))
            {
                UserRepository user = new UserRepository(context);
                var equity = user.AddFunds(1, 20);
                Assert.Equal(10020, equity);
            }
        }

        [Fact]
        public void ShouldWithdrawFundsFromUser()
        {
            using (var context = new TraderDbContext(this.fixture.option))
            {
                UserRepository repo = new UserRepository(context);
                var funds = repo.WithDrawFunds(1,20);
                repo.AddFunds(1, 20);
                Assert.Equal(9980, funds);
            }
        }
    }

    public class DatabaseFixture : IDisposable
    {
        public DbContextOptions option;

        public DatabaseFixture()
        {
            option = new DbContextOptionsBuilder<TraderDbContext>().UseInMemoryDatabase(databaseName: "UserDatabase").Options;
            using (var context = new TraderDbContext(option))
            {
                context.Users.Add(new User()
                {
                    UserId = 1,
                    UserName = "ABC",
                    FundsAvailable = 10000

                });
                context.Orders.Add(new Orders()
                {
                    OrderId = 1,
                    EquityId = 1,
                    UserId = 1,
                    QuantityPurchased = 20
                });

                context.Equities.Add(new Equity()
                {
                    Id = 1,
                    EquityName = "Stock",
                    BuyPrice = 1,
                    SellPrice = 1.5
                });
                context.SaveChanges();
            }

        }

        public void Dispose()
        {
            // ... clean up test data from the database ...
        }
    }
}
