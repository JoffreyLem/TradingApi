using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ApiTrading.Modele.DTO.Request;
using ApiTrading.Modele.DTO.Response;
using ApiTrading.Service.Strategy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XtbLibrairie.commands;
using ApiTrading.Filter;
namespace ApiTrading.Controllers
{
    

    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesErrorResponseType(typeof(ErrorModel))]
    [ProducesResponseType(500)]
    [ProducesResponseType(415)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  
    [Route("api/[controller]")]
    [ApiController]
    public class SignalController : ControllerBase
    {
        private readonly IStrategyService _strategyService;

        public SignalController(IStrategyService strategyService)
        {
            _strategyService = strategyService;
        }
        
        [HttpGet]
        [Route("Strategy")]
        [ProducesResponseType(typeof(StrategyResponse),200)]
        public async Task<IActionResult> GetAllStrategy()
        {
            return Ok(await _strategyService.GetAllStrategy());
        }

        [HttpGet]
        [Route("Timeframes")]
        [ProducesResponseType(typeof(TimeframeResponse),200)]
        public async Task<IActionResult> Timeframes()
        {
            return Ok(await _strategyService.GetAllTimeframe());
        }

        [HttpGet]
        [TypeFilter(typeof(XtbCheckConnectorFilter))]
        [ProducesResponseType(typeof(BaseResponse<SignalResponse>),200)]
        public async Task<IActionResult> GetSignals([FromQuery][Required] string strategy,[FromQuery][Required] string symbol,[FromQuery][Required] string timeframe)
        {
            return Ok(await _strategyService.GetSignals(strategy,symbol,timeframe));
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<SignalResponse>),200)]
        public async Task<IActionResult> PostSignal([FromQuery][Required] string strategy,[FromQuery][Required] string symbol,[FromQuery][Required] string timeframe)
        {
            return Ok(await _strategyService.GetSignals(strategy,symbol,timeframe));
        }
    }
}