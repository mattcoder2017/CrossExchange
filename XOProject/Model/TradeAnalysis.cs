using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XOProject
{
    public class TradeAnalysis
    {
        #region Constants
        public const string ACTION_BUY = "BUY";
        public const string ACTION_SELL = "SELL";
        #endregion

        public decimal Sum { get; set; }

        public decimal Average { get; set; }

        public decimal Maximum { get; set; }

        public decimal Minimum { get; set; }

        public string Action { get; set; }

        
    }
}
