using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace XOProject.Controller
{
    [EnableCors("permitone")]
    [Route("api/Portfolio")]
    public class PortfolioController : ControllerBase
    {
        private IPortfolioRepository _portfolioRepository { get; set; }

        public PortfolioController(IShareRepository shareRepository, ITradeRepository tradeRepository, IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }
       
        [HttpGet("{portFolioid}")]
        public async Task<IActionResult> GetPortfolioInfo([FromRoute]int portFolioid)
        {
            var portfolio = _portfolioRepository.GetAll().Where(x => x.Id.Equals(portFolioid));

            // Code smell, Lack indication if fetching non-existed record
            if (portfolio.Count<Portfolio>() == 0)
                return NotFound();

            return Ok(portfolio);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Portfolio value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _portfolioRepository.InsertAsync(value);

            return Created($"Portfolio/{value.Id}", value);
        }

    }
}
