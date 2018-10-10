using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace XOProject.Controller
{
    [Route("api/Share")]
    public class ShareController : ControllerBase
    {
        public IShareRepository _shareRepository { get; set; }

        public ShareController(IShareRepository shareRepository)
        {
            _shareRepository = shareRepository;
        }

        [HttpPut("{symbol}")]
        public async void UpdateLastPrice([FromRoute]string symbol)
        {
            // Comment out the code which is not fetching the last share info
            // var share = await _shareRepository.Query().Where(x => x.Symbol.Equals(symbol)).OrderByDescending(x => x.Rate).FirstOrDefaultAsync();
            var share = await _shareRepository.Query().Where(x => x.Symbol.Equals(symbol)).OrderByDescending(x => x.TimeStamp).FirstOrDefaultAsync();
            share.Rate =+ 10;
            await _shareRepository.UpdateAsync(share);
        }


        [HttpGet("{symbol}")]
        public async Task<IActionResult> Get([FromRoute]string symbol)
        {
            var shares = _shareRepository.Query().Where(x => x.Symbol.Equals(symbol)).ToList();
            if (shares.Count >= 0)
            {
                return Ok(shares);
            }
            else
                return BadRequest();
        }


        [HttpGet("{symbol}/Latest")]
        public async Task<IActionResult> GetLatestPrice([FromRoute]string symbol)
        {  
            //Bug fix - comment out the code which not fetching the most recent share quote
            //var share = await _shareRepository.Query().Where(x => x.Symbol.Equals(symbol)).FirstOrDefaultAsync();
            var share = await _shareRepository.Query().Where(x => x.Symbol.Equals(symbol)).OrderByDescending(x=>x.TimeStamp).FirstOrDefaultAsync();
            return Ok(share?.Rate);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]HourlyShareRate value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _shareRepository.InsertAsync(value);

            return Created($"Share/{value.Id}", value);
        }
        
    }
}
