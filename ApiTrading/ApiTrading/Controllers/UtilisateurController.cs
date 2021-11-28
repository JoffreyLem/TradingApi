namespace ApiTrading.Controllers
{
    using System.Threading.Tasks;
    using Filter;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Modele.DTO.Request;
    using Modele.DTO.Response;
    using Service.Utilisateur;

    /// <response code="415">Header Content-type manquant/Invalide</response>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesErrorResponseType(typeof(ErrorModel))]
    [ProducesResponseType(500)]
    [ProducesResponseType(415)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateurController : ControllerBase
    {
        private readonly IUtilisateurService _utilisateurService;

        public UtilisateurController(IUtilisateurService utilisateurService)
        {
            _utilisateurService = utilisateurService;
        }

        /// <summary>
        ///     Creation compte utilisateur
        /// </summary>
        /// <response code="409">L'email existe déja</response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<RegistrationResponse>), 201)]
        [ProducesResponseType(409)]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto user)
        {
            return CreatedAtAction(nameof(Register), await _utilisateurService.Register(user));
        }

        /// <summary>
        ///     Connexion de l'utilisateur
        /// </summary>
        /// <remarks>Connexion de l'utilisateur</remarks>
        /// <response code="403">Informations utilisateur incorrect</response>
        [ProducesResponseType(403)]
        [ProducesResponseType(typeof(BaseResponse<RegistrationResponse>), 200)]
        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            return Ok(await _utilisateurService.Login(user));
        }

        /// <summary>
        /// Récupération des infos de l'utilisateur
        /// </summary>
        /// <returns></returns>
        [HttpGet("Info")]
        [ProducesResponseType(typeof(BaseResponse<UserInfoReponse>), 200)]
        public async Task<IActionResult> GetUsersInfo()
        {
            
            return Ok(await _utilisateurService.GetUsersInfo());
        }

        /// <summary>
        ///     Mise à jour des informations de l'utilisateur
        /// </summary>
        /// <response code="200">Utilisateurs mit à jour</response>
        [HttpPut]
        [ProducesResponseType(403)]
        [ProducesResponseType(typeof(BaseResponse), 200)]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] UserUpdateRequest userUpdate)
        {
          
            return Ok(await _utilisateurService.Update(userUpdate));
        }
        


        /// <summary>
        ///     Suppression de l'utilisateur
        /// </summary>
        /// <response code="403">Token bearer incorrect</response>
        /// <response code="200">Utilisateurs deleted</response>
        [HttpDelete]
        [ProducesResponseType(403)]
        [ProducesResponseType(typeof(BaseResponse), 200)]
        [Route("Delete")]
        public async Task<IActionResult> Delete()
        {

            return Ok(await _utilisateurService.Delete());
        }
    }
}