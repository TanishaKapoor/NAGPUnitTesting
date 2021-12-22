using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBusiness
{
    [Serializable]
    public class InvalidUserNameException : Exception
    {
        public InvalidUserNameException(string name)
            : base(String.Format("Invalid UserName: {0}", name))
        {

        }
    }

    [Serializable]
    public class InvalidEquityNameException : Exception
    {

        public InvalidEquityNameException(string name)
            : base(String.Format("Invalid EquityName: {0}", name))
        {

        }
    }

    [Serializable]
    public class InsufficientFundsException : Exception
    {

        public InsufficientFundsException(double funds)
            : base(String.Format("InSufficient Funds: {0}", funds))
        {

        }
    }

    [Serializable]
    public class TradingCloseException : Exception
    {

        public TradingCloseException()
            : base(String.Format("Trading is close now"))
        {

        }
    }

    [Serializable]
    public class EquityNotOwnedException: Exception
    {
        public EquityNotOwnedException(string userName,string equityName)
            : base(String.Format($"Equity {equityName} is not owned by {userName}"))
        {

        }
    }

    [Serializable]
    public class InsufficientEquityQuantityException : Exception
    {
        public InsufficientEquityQuantityException(int actualQuantity, int expectedQuantity)
           : base(String.Format($"Equity {expectedQuantity} is not owned. Owned Quantity is {actualQuantity}"))
        {

        }
    }

}
