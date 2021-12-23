using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBusiness.Contracts;
using TradingData.Contracts;

namespace TradingBusiness
{
    public class FundHandler : IFundHandler
    {
        private IUserRepository _userRepository;

        public FundHandler(IUserRepository userRepo) {
            _userRepository = userRepo;
        }
        
        public double AddFunds(string userName, double amount)
        {
            var addFundCharges = CalculateFundsAddCharges(amount);
            var addFundAmountPostCharges = amount - addFundCharges;
            var user = _userRepository.GetUsers().Where(c => c.UserName == userName).FirstOrDefault();
           var fundAvailable = _userRepository.AddFunds(user.UserId, addFundAmountPostCharges);
            return fundAvailable;
        }
        
        public double WithDrawFunds(string userName, double amount)
        {
            var user = _userRepository.GetUsers().Where(c => c.UserName == userName).FirstOrDefault();
            if (user.FundsAvailable > amount)
            {
                var fundAvailable = _userRepository.WithDrawFunds(user.UserId, amount);
                return fundAvailable;
            }
            else
            {
                throw new InsufficientFundsException(amount);
            }
        }
        
        private double CalculateFundsAddCharges(double amount)
        {
            return amount > 100000 ? 0.0005 * amount : 0;
        }
    }
}

