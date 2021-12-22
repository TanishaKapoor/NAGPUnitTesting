using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingData.Models;

namespace TradingData.Contracts
{
    public interface IUserRepository:IDisposable
    {
        List<User> GetUsers();
        User GetUserById(int id);
        double AddFunds(int userId,double amount);
        double WithDrawFunds(int userId,double amount);
    }
}
