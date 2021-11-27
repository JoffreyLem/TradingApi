namespace ApiTrading.Controllers
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Filter;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Modele.DTO.Response;
    using Service.ExternalAPIHandler;
    using Utility;


    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesErrorResponseType(typeof(ErrorModel))]
    [ProducesResponseType(500)]
    [ProducesResponseType(415)]
    [ProducesResponseType(400)]
    [TypeFilter(typeof(XtbCheckConnectorFilter))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]/{symbol}/{timeframe}")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private  IApiHandler _apiHandler;
        
        public DataController(IApiHandler apiHandler)
        {
            _apiHandler = apiHandler;
        }
        
        /// <summary>
        /// Récupération du symbol par apport au timeframe spécifié
        /// </summary>
        /// <param name="symbol">truc</param>
        /// <param name="timeframe"></param>
        /// <returns></returns>
         [ProducesResponseType(typeof(BaseResponse<CandleListResponse>),200)]
         [HttpGet]
         [Route("GetFullChart")]
         public async Task<IActionResult> GetSymbolChart([Required][FromRoute] string symbol,[Required][FromRoute] string timeframe)
         {
             return Ok(await _apiHandler.GetAllChart(symbol, timeframe, true));
         }
        
        
        [ProducesResponseType(typeof(BaseResponse<CandleListResponse>),200)]
        [HttpGet]
        [Route("GetPartialChart")]
        public async Task<IActionResult> GetPartialSymbolChart(
            [Required][FromRoute] string symbol,
            [Required][FromRoute] string timeframe,
            [FromQuery][DataType(DataType.DateTime)] string start,
            [FromQuery][DataType(DataType.DateTime)] string end)
        {
            

            
            return Ok(await _apiHandler.GetPartialChart(symbol, timeframe, start,end));
        }
        
        
      
  
    }
}