using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingData.Models;

namespace TradingBusiness
{
    public class Util
    {
        public static double CalculateBrokerage(Equity equity, int quantity)
        {
            return Math.Max((equity.SellPrice * quantity) * 0.0005, 20);
        }

        public static bool UserHasEnoughFunds(int userId, double requiredFundsForTrading)
        {
            double availableFunds = 0;
            return availableFunds > requiredFundsForTrading;
        }

        public static bool IsTradingOpen()
        {
            var currTime = DateTime.UtcNow;
            if (currTime.DayOfWeek != DayOfWeek.Saturday && currTime.DayOfWeek != DayOfWeek.Sunday)
            {
                return currTime.Hour >= 9 && currTime.Hour <= 3;
            }

            return false;
        }
    }
}
