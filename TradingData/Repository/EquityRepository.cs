using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingData.Contracts;
using TradingData.DBContext;
using TradingData.Models;

namespace TradingData.Repository
{
    public class EquityRepository : IEquityRepository
    {
        private readonly TraderDbContext _equityContext;

        public EquityRepository(TraderDbContext dbContext)
        {
            _equityContext = dbContext;
        }

        public Equity GetEquityByName(string equityName)
        {
            return _equityContext.Equities.Where(c=>c.EquityName==equityName).FirstOrDefault();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
