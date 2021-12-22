using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingBusiness.Contracts;
using TradingData.Models;

namespace TradingApp.Controllers
{
    [ApiController]
    [Route("trading")]
    public class Trading : ControllerBase
    {
        

        private readonly IEquityHandler _equityHandler;
        private readonly IFundHandler _fundsHandler;

        public Trading(IEquityHandler equityHandler,IFundHandler fundHandler)
        {
            _equityHandler = equityHandler;
            _fundsHandler = fundHandler;
        }

        [HttpGet]
        [Route("buy")]
        public List<Orders> BuySellEquity(string userName,string equityName,int quantity)
        {
            return _equityHandler.BuySellEquity(userName, equityName, quantity);
        }

        [HttpGet]
        public double AddFunds(string userName, double funds)
        {
            return _fundsHandler.AddFunds(userName, funds);
        }
    }
}
