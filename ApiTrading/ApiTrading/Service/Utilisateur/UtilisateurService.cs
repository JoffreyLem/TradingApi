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
    using Domain;
    using Exception;
    using Mail;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using Modele;
    using Modele.DTO.Request;
    using Modele.DTO.Response;
    using Repository.Token;
    using Repository.Utilisateurs;

    public class UtilisateurService : IUtilisateurService
    {
        private readonly JwtConfig _jwtConfig;
        private readonly IMail _mailService;
        private readonly ITokenRepository _tokenRepository;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IUserRepository _userRepository;


        public UtilisateurService(
            IOptionsMonitor<JwtConfig> optionsMonitor,
            TokenValidationParameters tokenValidationParameters,
            IMail mailService)
        {
            _jwtConfig = optionsMonitor.CurrentValue;
            _tokenValidationParameters = tokenValidationParameters;

            _mailService = mailService;
        }

        public async Task<BaseResponse<RegistrationResponse>> Register(UserRegistrationRequestDto user)
        {
            List<string> ErrorExistMessage = new List<string>();
            var existingUser = await _userRepository.FindByEmailAsync(user.Email);
            if (existingUser != null)
                ErrorExistMessage.Add("L'email existe déja");
                

            var existingEmail = await _userRepository.FindByNameAsync(user.UserName);
            if (existingEmail != null)
            {
                ErrorExistMessage.Add("L'username existe déja");
            }

            if (ErrorExistMessage.Count > 0)
            {
                throw new AlreadyExistException(ErrorExistMessage);
            }

            var newUser = new IdentityUser<int> { Email = user.Email, UserName = user.UserName };

            var isCreated = await _userRepository.CreateAsync(newUser, user.Password);

            if (isCreated.Succeeded)
            {
                var jwtToken = await GenerateJwtToken(newUser);
                var message = new StringBuilder();
                message.Append("Inscription réussi\n");
                message.Append($"Le compte {user.Email} a été crée");
                await _mailService.Send(user.Email, "Inscription réussi !", message.ToString());
                var registration = new RegistrationResponse
                {
                    Token = jwtToken.Token,
                    Id = newUser.Id
                };

                return new BaseResponse<RegistrationResponse>(registration);
            }

            var messageErrorList = isCreated.Errors.Select(x => x.Description).ToList();
            throw new AppException(messageErrorList);
        }

        public async Task<BaseResponse<RegistrationResponse>> Login(UserLoginRequest user)
        {
            var existingUser = await _userRepository.FindByEmailAsync(user.Email);

            if (existingUser != null)
            {
                var isCorrect = await _userRepository.CheckPasswordAsync(existingUser, user.Password);

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
                var checkPasswd = await _userRepository.CheckPasswordAsync(userCurrent, user.OldPassword);

                if (!checkPasswd)
                    throw new UpdateException("Password incorrect");
                if (user.OldPassword == user.NewPassword)
                    throw new UpdateException("L'ancien mot de passe doit être changer");

                if (string.IsNullOrWhiteSpace(user.NewPassword))
                    throw new UpdateException("Le nouveau mot de passe ne peut pas être vide");


                var updatePwd =
                    await _userRepository.UpdatePasswordAsync(userCurrent, user.OldPassword, user.NewPassword);

                if (!updatePwd.Succeeded)
                {
                    var messageErrorList = updatePwd.Errors.Select(x => x.Description).ToList();
                    throw new AppException(messageErrorList);
                }
            }
            else if (string.IsNullOrEmpty(user.OldPassword) && !string.IsNullOrEmpty(user.NewPassword) ||
                     !string.IsNullOrEmpty(user.OldPassword) && string.IsNullOrEmpty(user.NewPassword))
            {
                throw new UpdateException("Veuillez indiquer l'ancien et le nouveau mot de passe");
            }

            if (!string.IsNullOrEmpty(user.Email))
            {
                var test = await _userRepository.FindByEmailAsync(user.Email);

                if (test != null)
                {
                    throw new AlreadyExistException("L'email existe déjà");
                }

                userCurrent.Email = user.Email;

              

            }
            
            if(!string.IsNullOrEmpty(user.UserName))
            {
                var test = await _userRepository.FindByNameAsync(user.UserName);

                if (test != null)
                {
                    throw new AlreadyExistException("L'username existe déjà");
                }
                
                userCurrent.UserName = user.UserName;
            }
            
            var test2 = await _userRepository.UpdateUser(userCurrent);

            if (!test2.Succeeded)
            {
                throw new UpdateException("Erreur de mise à jour de ");
            }


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
            var deleteUser = await _userRepository.DeleteUser(userCurrent);

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

            await _tokenRepository.AddToken(refreshToken);


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