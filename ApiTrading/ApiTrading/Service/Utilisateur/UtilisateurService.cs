namespace ApiTrading.Service.Utilisateur
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Configuration;
    using DbContext;
    using Domain;
    using Exception;
    using Mail;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using Modele;
    using Modele.DTO.Request;
    using Modele.DTO.Response;

    public class UtilisateurService : IUtilisateurService
    {
        private readonly ApiTradingDatabaseContext _apiDbContext;

        private readonly JwtConfig _jwtConfig;
        private readonly IMail _mailService;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public UtilisateurService(UserManager<IdentityUser<int>> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            IOptionsMonitor<JwtConfig> optionsMonitor,
            TokenValidationParameters tokenValidationParameters,
            ApiTradingDatabaseContext apiDbContext,
            IMail mailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtConfig = optionsMonitor.CurrentValue;
            _tokenValidationParameters = tokenValidationParameters;
            _apiDbContext = apiDbContext;
            _mailService = mailService;
        }

        public async Task<BaseResponse<RegistrationResponse>> Register(UserRegistrationRequestDto user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if (existingUser != null)
                throw new AlreadyExistException("L'email existe déja");

            var newUser = new IdentityUser<int> { Email = user.Email, UserName = user.Email };

            var isCreated = await _userManager.CreateAsync(newUser, user.Password);

            if (isCreated.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync("User").Result)
                {
                    var identityRole = new IdentityRole<int>();
                    identityRole.Name = "User";
                    var roleResult = _roleManager.CreateAsync(identityRole).Result;
                    if (!roleResult.Succeeded) throw new AppException("Création du role imposible");
                }


                var claim = new List<Claim>();
                claim.Add(new Claim(ClaimTypes.NameIdentifier, newUser.Id.ToString()));
                claim.Add(new Claim(ClaimTypes.Name, newUser.UserName));

                await _userManager.AddClaimsAsync(newUser, claim);

                await _userManager.AddToRoleAsync(newUser, "User");
                var userretrived = await _userManager.FindByEmailAsync(user.Email);
                var jwtToken = await GenerateJwtToken(newUser);
                var message = new StringBuilder();
                message.Append("Inscription réussi\n");
                message.Append($"Le compte {user.Email} a été crée");
                await _mailService.Send(user.Email, "Inscription réussi !", message.ToString());
                var registration = new RegistrationResponse
                {
                    Token = jwtToken.Token,
                    Id = userretrived.Id
                };

                return new BaseResponse<RegistrationResponse>(registration);
            }

            var messageErrorList = isCreated.Errors.Select(x => x.Description).ToList();
            throw new AppException(messageErrorList);
        }

        public async Task<BaseResponse<RegistrationResponse>> Login(UserLoginRequest user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Login);

            if (existingUser != null)
            {
                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

                if (isCorrect)
                {
                    var jwtToken = await GenerateJwtToken(existingUser);

                    var registration = new RegistrationResponse
                    {
                        Token = jwtToken.Token,
                        Id = existingUser.Id
                    };

                    return new BaseResponse<RegistrationResponse>(registration);
                }
            }

            throw new AuthException("Echec de l'authentication, utilisateur/mdp incorrect");
        }

        public Task<BaseResponse<TokenResponse>> GetId(string email)
        {
            throw new NotImplementedException();
        }


        public async Task<BaseResponse> Update(UserUpdateRequest user, IdentityUser<int> userCurrent,
            ClaimsPrincipal claimsPrincipal)
        {
            if (!string.IsNullOrEmpty(user.OldPassword) && !string.IsNullOrEmpty(user.NewPassword))
            {
                var checkPasswd = await _userManager.CheckPasswordAsync(userCurrent, user.OldPassword);

                if (!checkPasswd)
                    throw new PasswordUpdateException("Password incorrect");
                if (user.OldPassword == user.NewPassword)
                    throw new PasswordUpdateException("L'ancien mot de passe doit être changer");

                if (string.IsNullOrWhiteSpace(user.NewPassword))
                    throw new PasswordUpdateException("Le nouveau mot de passe ne peut pas être vide");


                var updatePwd = await _userManager.ChangePasswordAsync(userCurrent, user.OldPassword, user.NewPassword);

                if (!updatePwd.Succeeded)
                {
                    var messageErrorList = updatePwd.Errors.Select(x => x.Description).ToList();
                    throw new AppException(messageErrorList);
                }
            }
            else if (string.IsNullOrEmpty(user.OldPassword) && !string.IsNullOrEmpty(user.NewPassword) ||
                     !string.IsNullOrEmpty(user.OldPassword) && string.IsNullOrEmpty(user.NewPassword))
            {
                throw new PasswordUpdateException("Veuillez indiquer l'ancien et le nouveau mot de passe");
            }

            var claim = await _userManager.GetClaimsAsync(userCurrent);
            userCurrent.Email = user.Email;
            userCurrent.UserName = user.Email;
            var update = await _userManager.UpdateAsync(userCurrent);
            if (!update.Succeeded)
            {
                var messageErrorList = update.Errors.Select(x => x.Description).ToList();
                throw new AppException(messageErrorList);
            }

            var claimToUpdate = claim.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
            var newClaim = new Claim(ClaimTypes.NameIdentifier, user.Email);
            var test = await _userManager.ReplaceClaimAsync(userCurrent, claimToUpdate, newClaim);


            var message = new StringBuilder();
            message.Append($"Le compte {user.Email} a été mit à jour");
            await _mailService.Send(user.Email, "Compte mit à jour !", message.ToString());

            return new BaseResponse("Update réussi")
            {
                Message = "Update réussi"
            };
        }

        public async Task<BaseResponse> Delete(IdentityUser<int> userCurrent)
        {
            var deleteUser = await _userManager.DeleteAsync(userCurrent);

            if (deleteUser.Succeeded)
            {
                var message = new StringBuilder();
                message.Append($"Le compte {userCurrent.Email} a été supprilé");
                await _mailService.Send(userCurrent.Email, "Compte supprimé !", message.ToString());
                return new BaseResponse("Delete réussi");
            }

            var messageErrorList = deleteUser.Errors.Select(x => x.Description).ToList();
            throw new AppException(messageErrorList);
        }

        private async Task<AutResult> GenerateJwtToken(IdentityUser<int> user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken
            {
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

            return new AutResult
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token
            };
        }

        private string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}