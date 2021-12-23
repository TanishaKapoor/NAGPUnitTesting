using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingApp.Models;
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
        public TradingResponse BuySellEquity(string userName,string equityName,int quantity)
        {
            TradingResponse response = new TradingResponse();
            try
            {
                List<Orders>orders = _equityHandler.BuySellEquity(userName, equityName, quantity);
                if (orders.Count > 0)
                {
                    response.Status = Constants.Successful;

                }
                else
                {
                    response.Status = Constants.UnSuccessful;
                    response.ErrorMessage = Constants.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                response.Status = Constants.UnSuccessful;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        [HttpGet]
        [Route("addFunds")]
        public TradingResponse AddFunds(string userName, double funds)
        {
            TradingResponse response = new TradingResponse();
            try
            {
                var fundsAdded = _fundsHandler.AddFunds(userName, funds);
                if (fundsAdded > 0)
                {
                    response.Status = Constants.Successful;

                }
                else
                {
                    response.Status = Constants.UnSuccessful;
                    response.ErrorMessage = Constants.ErrorMessage;
                }
            }
            catch(Exception ex)
            {
                response.Status = Constants.UnSuccessful;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }
    }
}
