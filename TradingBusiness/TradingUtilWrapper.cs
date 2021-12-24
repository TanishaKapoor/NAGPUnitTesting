using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingData.Models;

namespace TradingBusiness
{
    public class TradingUtilWrapper : ITradingUtilWrapper
    {
        public TradingUtilWrapper()
        {

        }
        public double CalculateBrokerage(Equity equity, int quantity)
        {
            return Util.CalculateBrokerage(equity, quantity);
        }

        public bool IsTradingOpen(DateTime currTime)
        {
            return Util.IsTradingOpen(currTime);
        }

        public bool UserHasEnoughFunds(double funds, double requiredFundsForTrading)
        {
            return Util.UserHasEnoughFunds(funds, requiredFundsForTrading);
        }
    }
}
