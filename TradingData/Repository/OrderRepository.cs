using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingData.Contracts;
using TradingData.DBContext;
using TradingData.Models;

namespace TradingData.Repository
{
    public class OrderRepository:IOrderRepository
    {
        private readonly TraderDbContext _orderDbContext;

        public OrderRepository(TraderDbContext _dbContext)
        {
            _orderDbContext = _dbContext;
        }


        public List<Orders> GetAllEquityRelatedToAUser(int userId)
        {
            return _orderDbContext.Orders.Where(c => c.UserId == userId).ToList();

        }

        public void AddOrderRelatedToUser(Orders order)
        {
            _orderDbContext.Orders.Add(order);
            _orderDbContext.SaveChanges();
        }

        public void UpdateExistingOrderForUser(Orders order, int quantity)
        {
            var orders = _orderDbContext.Orders.Where(c=>c.OrderId==order.OrderId).FirstOrDefault();
            if (orders != null)
            {
                orders.QuantityPurchased +=quantity;
            }
            _orderDbContext.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
