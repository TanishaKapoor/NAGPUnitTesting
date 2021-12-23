using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingApp.Models
{
    public class TradingResponse
    {
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
    }
    public static class Constants
    {
        public const string Successful = "successful";
        public const string UnSuccessful = "Unsuccessful";
        public const string ErrorMessage = "Please try after some time.";
        public const string ModelStateInValid = "Model State is InValid";
    }
}
