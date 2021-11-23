using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Runtime.Versioning;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApiTrading.Configuration;
using ApiTrading.DbContext;
using ApiTrading.Domain;
using ApiTrading.Filter;
using ApiTrading.Modele;
using ApiTrading.Modele.DTO.Request;
using ApiTrading.Modele.DTO.Response;
using ApiTrading.Service.Mail;
using ApiTrading.Service.Utilisateur;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ApiTrading.Controllers
{
 
    /// <response code="500" cref="ErrorModel">Service indisponible</response>
    ///  <response code="400">Requête incorrecte</response>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesErrorResponseType(typeof(ErrorModel))]
    [ProducesResponseType(500)]
    [ProducesResponseType(400)]
    [ProducesResponseType(415)]
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
       /// Creation compte utilisateur
       /// </summary>
       /// <response code="409">Informations utilisateur déja existant</response>
       [HttpPost]
       [AllowAnonymous]
       [ProducesResponseType(typeof(RegistrationResponse),201)]
       [ProducesResponseType(409)]
       [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto user)
       {
           return CreatedAtAction(nameof(Register), await _utilisateurService.Register(user));
       }
       
       /// <summary>
       /// Connexion de l'utilisateur
       /// </summary>
       /// <remarks>Connexion de l'utilisateur</remarks>
       /// <response code="404">Informations utilisateur introuvable</response>
       /// <response code="403">Informations utilisateur incorrect</response>
       [ProducesResponseType(404)]
       [ProducesResponseType(403)]
       [ProducesResponseType(typeof(RegistrationResponse),200)]
       [HttpPost]
       [ValidateAntiForgeryToken]
       [AllowAnonymous]
       [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            return Ok(await _utilisateurService.Login(user));
        }

       /// <summary>
       /// Récupération de l'ID par l'adresse mail
       /// </summary>
       /// <response code="404">Email introuvable</response>
       /// <response code="403">Token incorrect</response>
       [ProducesResponseType(typeof(TokenResponse),200)]
       [ProducesResponseType(404)]
       [ProducesResponseType(403)]
       [HttpGet]
       [ClaimUserFilter]
       [Route("GetId")]
       public async Task<IActionResult> GetId([FromQuery(Name = "email")] string email)
       {
           var user = HttpContext.GetCurrentUser();
           return Ok(await _utilisateurService.GetId(email));
       }

       /// <summary>
       /// Mise à jour des informations de l'utilisateur
       /// </summary>
       /// <response code="404">ID Introuvable</response>
       /// <response code="403">Token incorrect</response>
       [HttpPut]
       [ProducesResponseType(typeof(ResponseModel),200)]
    
       [Route("Update")]
       public async Task<IActionResult> Update([FromBody] UserUpdateRequest userUpdate)
       {
           var user = HttpContext.GetCurrentUser();
           return Ok(await _utilisateurService.Update(userUpdate, user));
       }

       /// <summary>
       /// Suppression de l'utilisateur
       /// </summary>
       /// <response code="404">ID Introuvable</response>
       /// <response code="403">Token incorrect</response>
       [ProducesResponseType(typeof(ResponseModel),200)]
       [HttpDelete]
       [Route("Delete/{id}")]
       public async Task<IActionResult> Delete([FromRoute] [Required] int id)
       {
           var user = HttpContext.GetCurrentUser();
           return Ok(await _utilisateurService.Delete(user));
       }

   
    }
}