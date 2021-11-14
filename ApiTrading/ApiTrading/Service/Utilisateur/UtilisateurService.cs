using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
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
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly SignInManager<IdentityUser<int>> _signInManager;
        private readonly JwtConfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly ApiTradingDatabaseContext _apiDbContext;
        private readonly IMail _mailService;
        public UtilisateurService(UserManager<IdentityUser<int>> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            IOptionsMonitor<JwtConfig> optionsMonitor,
            TokenValidationParameters tokenValidationParameters,
            ApiTradingDatabaseContext apiDbContext,
            IMail mailService, SignInManager<IdentityUser<int>> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtConfig = optionsMonitor.CurrentValue;
            _tokenValidationParameters = tokenValidationParameters;
            _apiDbContext = apiDbContext;
            _mailService = mailService;
            _signInManager = signInManager;
        }
        
        public Task SendMessageRegistration()
        {
            throw new System.NotImplementedException();
        }

        public async Task<RegistrationResponse> Register(UserRegistrationRequestDto user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if (existingUser != null)
                throw new AlreadyExistException("Email Already Exist");

            var newUser = new IdentityUser<int> {Email = user.Email, UserName = user.Email};
            var isCreated = await _userManager.CreateAsync(newUser, user.Password);
         
            if (isCreated.Succeeded)
            {
                if (! _roleManager.RoleExistsAsync("User").Result)
                {
                    IdentityRole<int> identityRole = new IdentityRole<int>();
                    identityRole.Name = "User";
                    IdentityResult roleResult = _roleManager.CreateAsync(identityRole).Result;
                    if (!roleResult.Succeeded)
                    {
                        throw new AppException("Création du role imposible");
                    }

                }
                await _userManager.AddToRoleAsync(newUser, "User");
                var userretrived = await _userManager.FindByEmailAsync(user.Email);
                var jwtToken = await GenerateJwtToken(newUser);
                await _mailService.Send(user.Email, "test registrationOK", "test");
                return new RegistrationResponse
                {
                    Message = "Inscription réussi",
                    StatusCode = 201,
                    Token = jwtToken.Token,
                    Id=userretrived.Id,
                    
                };
            }
            else
            {
                var messageErrorList = isCreated.Errors.Select(x => x.Description).ToList();
                throw new AppException(messageErrorList);
            }
        }

        public async Task<RegistrationResponse> Login(UserLoginRequest user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if(existingUser == null)
            {
                throw new NotFoundException("User not found");
            }

         
            var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

            if(isCorrect)
            {
               
   
                var jwtToken = await GenerateJwtToken(existingUser);

                return new RegistrationResponse() {
                    Message = "Connexion réussi",
                    Token =  jwtToken.Token,
                    Id = existingUser.Id
                };
            }
            else 
            {
                throw new AuthException("Echec de l'authentication, utilisateur/mdp incorrect");
            }
        }

        public async Task<TokenResponse> GetId(string email)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if(existingUser == null)
            {
                throw new NotFoundException("Mail not found");
            }
            else
            {
                var tokenResponse = new TokenResponse();
                tokenResponse.StatusCode = 200;
                tokenResponse.ID = existingUser.Id;

                return tokenResponse;
            }
        }

        public async Task<ResponseModel> Update(UserUpdateRequest user, int id)
        {
            var existingUser = await _userManager.FindByIdAsync(id.ToString());
            if(existingUser == null)
            {
                throw new NotFoundException("User not found");
            }

            if (user.OldPassword != null && user.NewPassword !=null)
            {
                var updatePwd = await _userManager.ChangePasswordAsync(existingUser, user.OldPassword, user.NewPassword);

                if (!updatePwd.Succeeded)
                {
                    var messageErrorList = updatePwd.Errors.Select(x => x.Description).ToList();
                    throw new AppException(messageErrorList);
                }
            }

            existingUser.Email = user.Email;
            var update = await _userManager.UpdateAsync(existingUser);
            if (update.Succeeded)
            {
                return new ResponseModel() {
                    StatusCode =(int) HttpStatusCode.OK,
                    Message = "Update réussi",
                };
            }
            else
            {
                var messageErrorList = update.Errors.Select(x => x.Description).ToList();
                throw new AppException(messageErrorList);
            }
        }

        public async Task<ResponseModel> Delete(int id)
        {
            var existingUser = await _userManager.FindByIdAsync(id.ToString());
            if(existingUser == null)
            {
                throw new NotFoundException("User not found");
            }

            var deleteUser = await _userManager.DeleteAsync(existingUser);

            if (deleteUser.Succeeded)
            {
                return new ResponseModel() {
                    Message = "Update réussi",
                };
            }
            else
            {
                var messageErrorList = deleteUser.Errors.Select(x => x.Description).ToList();
                throw new AppException(messageErrorList);
            }
        }

        private async Task<AutResult> GenerateJwtToken(IdentityUser<int> user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new []
                {
                    new Claim("Id", user.Id.ToString()), 
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