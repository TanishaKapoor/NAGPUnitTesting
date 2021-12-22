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
    public class OrderRepositoryMockTests : IClassFixture<OrderDatabaseFixture>
    {
        OrderDatabaseFixture fixture;
        public OrderRepositoryMockTests(OrderDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void ShouldGetAllOrdersRelatedToUsers()
        {
            using (var context = new TraderDbContext(this.fixture.option))
            {
                OrderRepository repo = new OrderRepository(context);
                int expectedResult = 1;
                var orders = repo.GetAllEquityRelatedToAUser(1);
                Assert.Equal(expectedResult, orders.Count());

            }
        }

        [Fact]
        public void ShouldReturnZeroIfnoOrderF0rUsers()
        {
            using (var context = new TraderDbContext(this.fixture.option))
            {
                OrderRepository repo = new OrderRepository(context);
                int expectedResult = 0;
                var orders = repo.GetAllEquityRelatedToAUser(5);
                Assert.Equal(expectedResult, orders.Count());
            }
        }

        [Fact]
        public void ShouldOrdersRelatedToUsers()
        {
            using (var context = new TraderDbContext(this.fixture.option))
            {
                Orders order = new Orders
                {
                    UserId = 1,
                    EquityId = 2,
                    QuantityPurchased = 20
                };
                OrderRepository repo = new OrderRepository(context);
                repo.AddOrderRelatedToUser(order);
                Assert.Equal(2, context.Orders.ToList().Count);
            }
        }

        [Fact]
        public void ShouldUpdateBuyQuantityForExistingEquity()
        {
            using (var context = new TraderDbContext(this.fixture.option))
            {
                OrderRepository repo = new OrderRepository(context);
                
                repo.UpdateExistingOrderForUser(context.Orders.First(),10);
                Assert.Equal(20, context.Orders.First().QuantityPurchased);
            }
        }

        [Fact]
        public void ShouldUpdateSellQuantityForExistingEquity()
        {
            using (var context = new TraderDbContext(this.fixture.option))
            {
                OrderRepository repo = new OrderRepository(context);

                repo.UpdateExistingOrderForUser(context.Orders.First(), -10);
                Assert.Equal(10, context.Orders.First().QuantityPurchased);
            }
        }
    }

    public class OrderDatabaseFixture : IDisposable
    {
        public DbContextOptions option;

        public OrderDatabaseFixture()
        {
            option = new DbContextOptionsBuilder<TraderDbContext>().UseInMemoryDatabase(databaseName: "OrderDatabase").Options;
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
