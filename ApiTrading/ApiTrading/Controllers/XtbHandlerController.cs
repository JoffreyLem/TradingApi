using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using APIhandler;
using ApiTrading.Modele.DTO.Request;
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
        [ProducesResponseType(typeof(BaseResponse),200)]
        [HttpPost]
        [Route("Connect")]
        public async Task<IActionResult> Connect([FromBody] UserLoginRequest user)
        {
            return Ok(await _apiHandler.Login(user.Login, user.Password));
        }
        /// <summary>
        /// Deconnection de l'API Externe XTB
        /// </summary>
        [ProducesResponseType(typeof(BaseResponse),200)]
        [HttpPost]
        [Route("Logout")]
    
        public async Task<IActionResult> Logout()
        {
            return Ok(await _apiHandler.Logout());
        }
        
     
    }
}