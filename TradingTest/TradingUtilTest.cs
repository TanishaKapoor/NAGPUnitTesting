using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBusiness;
using Xunit;
using Moq;
using TradingData.Models;

namespace TradingTest
{
    public class TradingUtilTest
    {
        [Fact]
        public void ShouldReturnMinimumBrokerageTwenty()
        {
            var expectedBrokerage = 20;
            TradingUtilWrapper wrapper = new TradingUtilWrapper();
            var equity = new Equity()
            {
                Id = 1,
                EquityName = "Stock",
                BuyPrice = 1,
                SellPrice = 1.5
            };
            double brokerge= wrapper.CalculateBrokerage(equity, 20);
            Assert.Equal(brokerge, expectedBrokerage);
        }


        [Fact]
        public void ShouldReturnBrokerageBasedOnQuantity()
        {
            var expectedBrokerage = 40;
            TradingUtilWrapper wrapper = new TradingUtilWrapper();
            var equity = new Equity()
            {
                Id = 1,
                EquityName = "Stock",
                BuyPrice = 1,
                SellPrice = 10
            };
            double brokerge = wrapper.CalculateBrokerage(equity, 80);
            Assert.Equal(brokerge, expectedBrokerage);
        }


        [Fact]
        public void ShouldReturnTrueIfUserHaveEnoughFunds()
        {
            var expected = true;
            TradingUtilWrapper wrapper = new TradingUtilWrapper();
            bool answer = wrapper.UserHasEnoughFunds(20, 10);
            Assert.Equal(answer, expected);
        }


        [Fact]
        public void ShouldReturnFalseIfUserDoNotHaveEnoughFunds()
        {
            var expected = false;
            TradingUtilWrapper wrapper = new TradingUtilWrapper();
            bool answer = wrapper.UserHasEnoughFunds(10, 100);
            Assert.Equal(answer, expected);
        }

        [Fact]
        public void ShouldCheckFalseForTradingOpen()
        {
            bool answer = false;
            TimeSpan start = new TimeSpan(9, 0, 0); //10 o'clock
            TimeSpan end = new TimeSpan(15, 0, 0); //12 o'clock
            var currTime = DateTime.UtcNow;
            if (currTime.DayOfWeek != DayOfWeek.Saturday && currTime.DayOfWeek != DayOfWeek.Sunday)
            {
                answer =  ((currTime.TimeOfDay > start) && (currTime.TimeOfDay < end));
            }

            TradingUtilWrapper wrapper = new TradingUtilWrapper();
            bool actual = wrapper.IsTradingOpen();
            Assert.Equal(actual, answer);
        }
    }
}
