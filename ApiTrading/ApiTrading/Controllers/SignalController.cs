using System.Threading.Tasks;
using ApiTrading.Modele.DTO.Request;
using ApiTrading.Service.Strategy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Route("GetAllStrategy")]
        public async Task<IActionResult> GetAllStrategy()
        {
            return Ok(await _strategyService.GetAllStrategy());
        }
    }
}