using System;
using System.Collections.Generic;
using System.Linq;
using TradingBusiness.Contracts;
using TradingData.Contracts;
using TradingData.Models;

namespace TradingBusiness
{
    public class EquityHandler : IEquityHandler
    {
        readonly ITradingUtilWrapper _wrapper;
        private readonly IEquityRepository _equityRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private FundHandler fundHandler;

        public EquityHandler(IEquityRepository equityRepo, IOrderRepository orderRepo, IUserRepository userRepo, ITradingUtilWrapper wrapper)
        {
            _equityRepository = equityRepo;
            _orderRepository = orderRepo;
            _userRepository = userRepo;
            fundHandler = new FundHandler(_userRepository);
            _wrapper = wrapper;
        }

        /// <summary>
        /// Quantity will be -ve for sell and +ve for Buy
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="quantity"></param>
        /// <param name="equityName"></param>
        public List<Orders> BuySellEquity(string userName, string equityName, int quantity)
        {
            if (_wrapper.IsTradingOpen(DateTime.UtcNow))
            {
                List<Orders> usersOrders = new List<Orders>();
                Equity equity = _equityRepository.GetEquityByName(equityName);
                if (equity == null)
                    throw new InvalidEquityNameException(equityName);
                List<User> allUsers = _userRepository.GetUsers();
                User user = allUsers.Where(c => c.UserName == userName).FirstOrDefault();
                if (user == null)
                    throw new InvalidUserNameException(userName);
                List<Orders> orders = _orderRepository.GetAllEquityRelatedToAUser(user.UserId);

                // Sell Equity
                if (quantity < 0)
                {
                    var existingOrder = orders.Where(c => c.EquityId == equity.Id).FirstOrDefault();
                    if (existingOrder != null)
                    {
                        if (existingOrder.QuantityPurchased > Math.Abs(quantity))
                        {
                            _orderRepository.UpdateExistingOrderForUser(existingOrder, quantity);
                            var brokerageAmount = _wrapper.CalculateBrokerage(equity, Math.Abs(quantity));
                            var amount = Math.Abs(quantity) * equity.SellPrice - brokerageAmount;
                            fundHandler.AddFunds(userName, amount);
                            orders = _orderRepository.GetAllEquityRelatedToAUser(user.UserId);
                            return orders;
                        }
                        else
                        {
                            throw new InsufficientEquityQuantityException(existingOrder.QuantityPurchased, quantity);
                        }
                    }
                    else
                    {
                        throw new EquityNotOwnedException(userName, equityName);
                    }
                }
                else
                {
                    if (_wrapper.UserHasEnoughFunds(user.FundsAvailable, equity.BuyPrice * quantity))
                    {
                        // add order to order db
                        var existingOrder = orders.Where(c => c.EquityId == equity.Id).FirstOrDefault();
                        if (existingOrder != null)
                        {
                            _orderRepository.UpdateExistingOrderForUser(existingOrder, quantity);
                        }
                        else
                        {
                            _orderRepository.AddOrderRelatedToUser(new Orders
                            {
                                UserId = user.UserId,
                                EquityId = equity.Id,
                                QuantityPurchased = quantity
                            }); ;
                        }
                        var amount = quantity * equity.SellPrice;
                        fundHandler.WithDrawFunds(userName, amount);
                        orders = _orderRepository.GetAllEquityRelatedToAUser(user.UserId);
                        return orders.ToList();
                    }
                    else
                    {
                        throw new InsufficientFundsException(equity.BuyPrice * quantity);
                    }
                }
            }
            else
            {
                throw new TradingCloseException();
            }
        }
    }
}
