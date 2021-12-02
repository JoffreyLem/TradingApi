using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ApiTrading.Filter;
using ApiTrading.Modele.DTO.Response;
using ApiTrading.Service.ExternalAPIHandler;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiTrading.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesErrorResponseType(typeof(ErrorModel))]
    [ProducesResponseType(500)]
    [ProducesResponseType(415)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [TypeFilter(typeof(XtbCheckConnectorFilter))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IApiHandler _apiHandler;

        public DataController(IApiHandler apiHandler)
        {
            _apiHandler = apiHandler;
        }

        /// <summary>
        ///     Return le graphique par apport au symbol et timeframe, full si pas de start/end Date sinon l'intervalle des
        ///     paramètres spécifiées.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="timeframe"></param>
        /// <param name="start">Récupere les données depuis cette date</param>
        /// <param name="end">Limite la récupération à cette date, nécessite le parametre start si utilisée</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(BaseResponse<CandleListResponse>), 200)]
        [HttpGet("Chart")]
        public async Task<IActionResult> GetSymbolChart(
            [Required] [FromQuery] string symbol,
            [Required] [FromQuery] string timeframe,
            [FromQuery] [DataType(DataType.DateTime)]
            string start,
            [FromQuery] [DataType(DataType.DateTime)]
            string end)
        {
            return Ok(await _apiHandler.GetChart(symbol, timeframe, start, end));
        }


        /// <summary>
        ///     Récupération de tous les symbols disponible dans l'API XTB
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(BaseResponse<List<SymbolResponse>>), 200)]
        [HttpGet("Symbols")]
        public async Task<IActionResult> GetAllSymbols()
        {
            return Ok(await _apiHandler.GetAllSymbol());
        }

        /// <summary>
        ///     Verification si le symbol existe dans l'API
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(BaseResponse<bool>), 200)]
        [HttpGet("CheckSymbol")]
        public async Task<IActionResult> CheckSymbol([Required] [FromQuery] string symbol)
        {
            return Ok(await _apiHandler.CheckIfSymbolExist(symbol));
        }
    }
}