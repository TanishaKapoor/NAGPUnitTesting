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
    public class UserRepository:IUserRepository
    {
        private readonly TraderDbContext _userDbContext;

        public UserRepository(TraderDbContext _dbcontext)
        {
            _userDbContext = _dbcontext;
        }
        public List<User> GetUsers()
        {
            return _userDbContext.Users.ToList();
        }
        public User GetUserById(int id)
        {
            return _userDbContext.Users.Where(user=>user.UserId==id).FirstOrDefault();
        }
        public double AddFunds(int userId,double amount)
        {
            User user= _userDbContext.Users.Find(userId);
            user.FundsAvailable += amount;
            _userDbContext.SaveChanges();
            return user.FundsAvailable;
        }
        public double WithDrawFunds(int userId,double amount)
        {
            User user = _userDbContext.Users.Find(userId);
            user.FundsAvailable -= amount;
            _userDbContext.SaveChanges();
            return user.FundsAvailable;
        }

        public void Dispose()
        {
            _userDbContext.Database.EnsureDeleted();
            _userDbContext.Dispose();
        }
    }
}
