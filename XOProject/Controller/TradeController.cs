using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace XOProject.Controller
{
    [Route("api/Trade/")]
    public class TradeController : ControllerBase
    {
        private IShareRepository _shareRepository { get; set; }
        private ITradeRepository _tradeRepository { get; set; }
        private IPortfolioRepository _portfolioRepository { get; set; }

        public TradeController(IShareRepository shareRepository, ITradeRepository tradeRepository, IPortfolioRepository portfolioRepository)
        {
            _shareRepository = shareRepository;
            _tradeRepository = tradeRepository;
            _portfolioRepository = portfolioRepository;
        }


        [HttpGet("{portfolioid}")]
        public async Task<IActionResult> GetAllTradings([FromRoute]int portFolioid)
        {
            var trade = _tradeRepository.Query().Where(x => x.PortfolioId.Equals(portFolioid));
            return Ok(trade);
        }


        /// <summary>
        /// For a given symbol of share, get the statistics for that particular share calculating the maximum, minimum, average and Sum of all the trades that happened for that share. 
        /// Group statistics individually for all BUY trades and SELL trades separately.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>

        [HttpGet("Analysis/{symbol}")]
        public async Task<IActionResult> GetAnalysis([FromRoute]string symbol)
        {
            var trade = _tradeRepository.Query().Where(x => x.Symbol.Equals(symbol));
            if (trade.Count<Trade>() == 0)
                return NotFound();

            //Populate Buy and Sell list
            var tradeBuy = trade.Where(x => x.Action == TradeAnalysis.ACTION_BUY);
            var tradeSell = trade.Where(x => x.Action == TradeAnalysis.ACTION_SELL);

            var list = new List<TradeAnalysis>();

            //Add to the return list with the calculated group  
            if (tradeBuy.Count<Trade>() > 0)
            {
                list.Add(new TradeAnalysis
                {
                    Action = TradeAnalysis.ACTION_BUY,
                    Maximum = tradeBuy.Max(e => e.NoOfShares),
                    Minimum = tradeBuy.Min(e => e.NoOfShares),
                    Average = tradeBuy.Average(e => (decimal)e.NoOfShares),
                    Sum = tradeBuy.Sum(e => e.NoOfShares)
                });
            }

            if (tradeSell.Count<Trade>() > 0)
            {
                list.Add(new TradeAnalysis
            {
                Action = TradeAnalysis.ACTION_SELL,
                Maximum = tradeSell.Max(e => e.NoOfShares),
                Minimum = tradeSell.Min(e => e.NoOfShares),
                Average = tradeSell.Average(e => (decimal)e.NoOfShares),
                Sum = tradeSell.Sum(e => e.NoOfShares)
            });
                }
                       
            return Ok(list); 
        }


    }
}
