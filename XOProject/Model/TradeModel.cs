using System.ComponentModel.DataAnnotations;

namespace XOProject
{
    public class TradeModel
    {
        private TradeModel() { }

        [Required]
        public string Symbol;

        [Required]
        public int NoOfShares { get; set; }

        [Required]
        public int PortfolioId { get; set; }

        [Required]
        [RegularExpression("BUY|SELL")]
        public string Action { get; set; }
    }
}