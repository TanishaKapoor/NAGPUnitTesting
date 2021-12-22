using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingData.Models
{
    public class Equity
    {
        [Key]
        public int Id { get; set; }
        public string EquityName { get; set; }
        public double BuyPrice { get; set; }
        public double SellPrice { get; set; }
    }
}
