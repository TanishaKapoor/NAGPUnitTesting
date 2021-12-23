using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingData.Models;
using TradingData.Repository;

namespace TradingBusiness
{
    public class Util
    {
        public static double CalculateBrokerage(Equity equity, int quantity)
        {
            return Math.Max((equity.SellPrice * quantity) * 0.05, 20);
        }

        public static bool UserHasEnoughFunds(double availableFunds, double requiredFundsForTrading)
        {
            return availableFunds > requiredFundsForTrading;
        }

        public static bool IsTradingOpen()
        {
            TimeSpan start = new TimeSpan(9, 0, 0); //10 o'clock
            TimeSpan end = new TimeSpan(15, 0, 0); //12 o'clock
            var currTime = DateTime.UtcNow;
            if (currTime.DayOfWeek != DayOfWeek.Saturday && currTime.DayOfWeek != DayOfWeek.Sunday)
            {
                return ((currTime.TimeOfDay > start) && (currTime.TimeOfDay < end));
            }
            
            return false;
        }
    }
}
