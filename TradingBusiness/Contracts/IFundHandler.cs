using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBusiness.Contracts
{
    public interface IFundHandler
    {
        double AddFunds(string userName, double amount);
        double WithDrawFunds(string userName, double amount);
    }
}
