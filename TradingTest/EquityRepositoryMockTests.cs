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
    public class EquityRepositoryMockTests : IClassFixture<EquityDatabaseFixture>
    {
        EquityDatabaseFixture fixture;
        public EquityRepositoryMockTests(EquityDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void ShouldGetEquityByName()
        {
            using (var context = new TraderDbContext(this.fixture.option))
            {
                EquityRepository repo = new EquityRepository(context);
                int expectedResult = 1;
                var equity = repo.GetEquityByName("Stock");
                Assert.Equal(expectedResult, equity.Id);

            }
        }

    }

    public class EquityDatabaseFixture : IDisposable
    {
        public DbContextOptions option;

        public EquityDatabaseFixture()
        {
            option = new DbContextOptionsBuilder<TraderDbContext>().UseInMemoryDatabase(databaseName: "EquityDatabase").Options;
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
