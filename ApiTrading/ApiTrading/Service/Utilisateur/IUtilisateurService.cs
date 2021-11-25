using System.Threading.Tasks;
using ApiTrading.Modele.DTO.Request;
using ApiTrading.Modele.DTO.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiTrading.Service.Utilisateur
{
    public interface IUtilisateurService
    {
        
     
        public Task<BaseResponse<RegistrationResponse>> Register(UserRegistrationRequestDto user);
        public Task<BaseResponse<RegistrationResponse>> Login(UserLoginRequest user);
        public Task<BaseResponse<TokenResponse>> GetId(string email);
        public Task<BaseResponse> Update(UserUpdateRequest user,IdentityUser<int> userCurrent);
        public Task<BaseResponse> Delete(IdentityUser<int> userCurrent);
    }
}