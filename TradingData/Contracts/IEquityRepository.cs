using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingData.Models;

namespace TradingData.Contracts
{
    public interface IEquityRepository : IDisposable
    {
        Equity GetEquityByName(string equityName);
    }
}
