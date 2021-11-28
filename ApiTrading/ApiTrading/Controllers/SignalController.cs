namespace ApiTrading.Controllers
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Filter;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Modele.DTO.Request;
    using Modele.DTO.Response;
    using Service.Strategy;

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

        /// <summary>
        ///     Récupération de toutes les stratégies disponible dans le système
        /// </summary>
        /// <returns></returns>
        [HttpGet("Strategy")]
        [ProducesResponseType(typeof(StrategyResponse), 200)]
        public async Task<IActionResult> GetAllStrategy()
        {
            return Ok(await _strategyService.GetAllStrategy());
        }

        /// <summary>
        ///     Récupération des signaux de l'utilisateur si indiquer sinon du systeme
        /// </summary>
        /// <param name="strategy"></param>
        /// <param name="symbol"></param>
        /// <param name="timeframe"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpGet]
        [TypeFilter(typeof(XtbCheckConnectorFilter))]
        [ProducesResponseType(typeof(BaseResponse<SignalResponse>), 200)]
        public async Task<IActionResult> GetSignals(
            [FromQuery] [Required] string symbol,
            [FromQuery] [Required] string timeframe,
            [FromQuery] string strategy,
            [FromQuery] string user)
        {
            return Ok(await _strategyService.GetSignals(strategy, symbol, timeframe, user));
        }

        /// <summary>
        ///     Sauvegarde d'un signal dans la base de données
        /// </summary>
        /// <param name="infoRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse), 200)]
        public async Task<IActionResult> PostSignal([FromBody] SignalInfoRequest infoRequest)
        {
            var user = HttpContext.GetCurrentUser();
            return CreatedAtAction(nameof(PostSignal), await _strategyService.PostSignal(infoRequest, user));
        }

        [HttpGet("UserGiverSignals")]
        [ProducesResponseType(typeof(BaseResponse<List<string>>), 200)]
        public async Task<IActionResult> GetAllUserGiverSignal()
        {
            return Ok(await _strategyService.GetUsersGiverSignal());
        }
    }
}