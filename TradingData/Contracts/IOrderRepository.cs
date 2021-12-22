using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingData.Models;

namespace TradingData.Contracts
{
    public interface IOrderRepository:IDisposable
    {
        List<Orders> GetAllEquityRelatedToAUser(int userId);
        void AddOrderRelatedToUser(Orders order);
        void UpdateExistingOrderForUser(Orders order, int quantity);
    }
}
