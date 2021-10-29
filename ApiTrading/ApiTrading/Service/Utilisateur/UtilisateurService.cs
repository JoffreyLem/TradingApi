using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApiTrading.Configuration;
using ApiTrading.DbContext;
using ApiTrading.Domain;
using ApiTrading.Exception;
using ApiTrading.Modele;
using ApiTrading.Modele.DTO.Request;
using ApiTrading.Modele.DTO.Response;
using ApiTrading.Service.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ApplicationException = System.ApplicationException;

namespace ApiTrading.Service.Utilisateur
{
    public class UtilisateurService : IUtilisateurService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly ApiTradingDatabaseContext _apiDbContext;
        private readonly IMail _mailService;
        public UtilisateurService(UserManager<IdentityUser> userManager,
            IOptionsMonitor<JwtConfig> optionsMonitor,
            TokenValidationParameters tokenValidationParameters,
            ApiTradingDatabaseContext apiDbContext,
            IMail mailService)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
            _tokenValidationParameters = tokenValidationParameters;
            _apiDbContext = apiDbContext;
            _mailService = mailService;
        }
        
        public Task SendMessageRegistration()
        {
            throw new System.NotImplementedException();
        }

        public async Task<RegistrationResponse> Register(UserRegistrationRequestDto user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if (existingUser != null)
                throw new ApplicationException("Email Already Exist");

            var newUser = new IdentityUser {Email = user.Email, UserName = user.Email};
            var isCreated = await _userManager.CreateAsync(newUser, user.Password);
            if (isCreated.Succeeded)
            {
                var jwtToken = await GenerateJwtToken(newUser);
                await _mailService.Send(user.Email, "test registrationOK", "test");
                return new RegistrationResponse
                {
                    Result = true,
                    Token = jwtToken.Token,
                    
                };
            }
            else
            {
                var messageErrorList = isCreated.Errors.Select(x => x.Description).ToList();
                string messageError ="";
                foreach (var s in messageErrorList)
                {
                    messageError += s;
                }
                throw new ApplicationException(messageError);
            }
        }

        public async Task<RegistrationResponse> Login(UserLoginRequest user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if(existingUser == null)
            {
                throw new ApplicationException("Invalid authentication request");
            }

         
            var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

            if(isCorrect)
            {
                var jwtToken = await GenerateJwtToken(existingUser);

                return new RegistrationResponse() {
                    Result = true, 
                    Token =  jwtToken.Token
                };
            }
            else 
            {
                throw new NotFoundException("Utilisateur introuvable");
                 
            }
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