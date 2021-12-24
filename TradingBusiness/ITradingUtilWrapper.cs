using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingData.Models;

namespace TradingBusiness
{
    public interface ITradingUtilWrapper
    {
        double CalculateBrokerage(Equity equity, int quantity);
        bool UserHasEnoughFunds(double funds, double requiredFundsForTrading);
        bool IsTradingOpen(DateTime currTime);
    }
}
