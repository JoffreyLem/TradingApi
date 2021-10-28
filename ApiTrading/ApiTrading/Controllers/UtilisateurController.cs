using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApiTrading.Configuration;
using ApiTrading.DbContext;
using ApiTrading.Domain;
using ApiTrading.Modele;
using ApiTrading.Modele.DTO.Request;
using ApiTrading.Modele.DTO.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ApiTrading.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateurController : ControllerBase
    {
        private readonly JwtConfig _jwtConfig;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly ApiTradingDatabaseContext _apiDbContext;
        
        public UtilisateurController(UserManager<IdentityUser> userManager,
                                    IOptionsMonitor<JwtConfig> optionsMonitor,
                                    TokenValidationParameters tokenValidationParameters,
                                    ApiTradingDatabaseContext apiDbContext)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
            _tokenValidationParameters = tokenValidationParameters;
            _apiDbContext = apiDbContext;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);
                if (existingUser != null)
                    return BadRequest(new RegistrationResponse
                    {
                        Result = false,
                        Errors = new List<string>
                        {
                            "Email already exist"
                        }
                    });

                var newUser = new IdentityUser {Email = user.Email, UserName = user.Email};
                var isCreated = await _userManager.CreateAsync(newUser, user.Password);
                if (isCreated.Succeeded)
                {
                    var jwtToken = await GenerateJwtToken(newUser);

                    return Ok(new RegistrationResponse
                    {
                        Result = true,
                        Token = jwtToken.Token
                    });
                }

                return new JsonResult(new RegistrationResponse
                    {
                        Result = false,
                        Errors = isCreated.Errors.Select(x => x.Description).ToList()
                    }
                ) {StatusCode = 500};
            }

            return BadRequest(new RegistrationResponse
            {
                Result = false,
                Errors = new List<string>
                {
                    "Invalid payload"
                }
            });
        }
        
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            if(ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);
                if(existingUser == null) 
                {
               
                    return BadRequest(new RegistrationResponse() {
                        Result = false,
                        Errors = new List<string>(){
                            "Invalid authentication request"
                        }});
                }

         
                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

                if(isCorrect)
                {
                    var jwtToken = await GenerateJwtToken(existingUser);

                    return Ok(new RegistrationResponse() {
                        Result = true, 
                        Token =  jwtToken.Token
                    });
                }
                else 
                {
                
                    return BadRequest(new RegistrationResponse() {
                        Result = false,
                        Errors = new List<string>(){
                            "Invalid authentication request"
                        }});
                }
            }

            return BadRequest(new RegistrationResponse() {
                Result = false,
                Errors = new List<string>(){
                    "Invalid payload"
                }});
        }

        private async Task<AutResult> GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new []
                {
                    new Claim("Id", user.Id), 
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken(){
                JwtId = token.Id,
                IsUsed = false,
                UserId = user.Id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddYears(1),
                IsRevoked = false,
                Token = RandomString(25) + Guid.NewGuid()
            };

            await _apiDbContext.RefreshTokens.AddAsync(refreshToken);
            await _apiDbContext.SaveChangesAsync();

            return new AutResult() {
                Token = jwtToken,
                Result = true,
                RefreshToken = refreshToken.Token
            };
        }

        private  string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}