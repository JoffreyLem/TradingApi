using System.Threading.Tasks;
using ApiTrading.Modele.DTO.Request;
using ApiTrading.Modele.DTO.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiTrading.Service.Utilisateur
{
    public interface IUtilisateurService
    {
        
        public Task SendMessageRegistration();
        public Task<RegistrationResponse> Register(UserRegistrationRequestDto user);
        public Task<RegistrationResponse> Login(UserLoginRequest user);
        public Task<TokenResponse> GetId(string email);
        public Task<ResponseModel> Update(UserUpdateRequest user,IdentityUser<int> userCurrent);
        public Task<ResponseModel> Delete(IdentityUser<int> userCurrent);
    }
}