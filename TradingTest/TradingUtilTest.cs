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
        public void ShouldReturnBrokerageBasedOnQuantityAndPrice()
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
            TradingUtilWrapper wrapper = new TradingUtilWrapper();
            bool actual = wrapper.IsTradingOpen(DateTime.Parse("25-Dec-2021"));
            Assert.Equal(actual, answer);
        }

        [Fact]
        public void ShouldCheckTrueForTradingOpen()
        {
            bool answer = true;
            TradingUtilWrapper wrapper = new TradingUtilWrapper();
            bool actual = wrapper.IsTradingOpen(DateTime.Parse("23-Dec-2021 09:50:48 AM"));
            Assert.Equal(actual, answer);
        }
    }
}
