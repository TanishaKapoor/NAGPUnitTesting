using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingData.Models;

namespace TradingBusiness.Contracts
{
    public interface IEquityHandler
    {
        /// <summary>
        /// Quantity will be -ve for sell and +ve for Buy
        /// </summary>
        ///<param name="equityName"></param>
        ///<param name="quantity"></param>
        ///<param name="time"></param>
        ///<param name="userName"></param>
        List<Orders> BuySellEquity(string userName,string equityName, int quantity);
    }
}
