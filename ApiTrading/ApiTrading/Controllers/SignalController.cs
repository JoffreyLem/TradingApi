using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ApiTrading.Modele.DTO.Request;
using ApiTrading.Modele.DTO.Response;
using ApiTrading.Service.Strategy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XtbLibrairie.commands;

namespace ApiTrading.Controllers
{
    
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesErrorResponseType(typeof(ErrorModel))]
    [ProducesResponseType(500)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    [ProducesResponseType(415)]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [Route("signals/{strategy}/{symbol}/{timeframe}")]
        [ProducesResponseType(typeof(SignalResponse),200)]
        public async Task<IActionResult> GetSignals([FromRoute][Required] string strategy,[FromQuery][Required] string symbol,[FromQuery][Required] string timeframe)
        {
            return Ok();
        }
    }
}