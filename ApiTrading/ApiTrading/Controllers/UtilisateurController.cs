using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    [ProducesResponseType(typeof(RegistrationResponse),200)]
    [ProducesErrorResponseType(typeof(BadRequestObjectResult))]
    [ProducesResponseType(500)]
    [ProducesResponseType(400)]
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
       /// <remarks>Création d'un utilisateur</remarks>
       [HttpPost]
       [ValidateModel]
       [AllowAnonymous]
       [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto user)
        {
            return Ok(await _utilisateurService.Register(user));
        }
       
       /// <summary>
       /// Connexion de l'utilisateur
       /// </summary>
       /// <remarks>Connexion de l'utilisateur</remarks>
       /// <response code="404">client introuvable pour avec le body spécifier</response>
       [ProducesResponseType(404)]
       [HttpPost]
       [AllowAnonymous]
       [ServiceFilter(typeof(ValidateModelAttribute))]
       [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            return Ok(await _utilisateurService.Login(user));
        }

   
    }
}