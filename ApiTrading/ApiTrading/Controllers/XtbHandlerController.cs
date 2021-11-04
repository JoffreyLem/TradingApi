using System.Threading.Tasks;
using APIhandler;
using ApiTrading.Modele.DTO.Request;
using ApiTrading.Modele.DTO.Response;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class XtbHandlerController : ControllerBase
    {
        private  IApiHandler _apiHandler;

        public XtbHandlerController(IApiHandler apiHandler)
        {
            _apiHandler = apiHandler;
        }
        /// <summary>
        /// Connection à l'API Externe XTB
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel),200)]
        [HttpPost]
        [Route("Connect")]
   
        public async Task<IActionResult> Connect([FromBody] ApiConnectionRequestDto user)
        {
            return Ok(await _apiHandler.Login(user.Account, user.Password));
        }
        /// <summary>
        /// Deconnection de l'API Externe XTB
        /// </summary>
        [ProducesResponseType(typeof(ResponseModel),200)]
        [HttpPost]
        [Route("Logout")]
    
        public async Task<IActionResult> Logout()
        {
            return Ok(await _apiHandler.Logout());
        }
    }
}