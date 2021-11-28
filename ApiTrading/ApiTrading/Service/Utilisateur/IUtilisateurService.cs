namespace ApiTrading.Service.Utilisateur
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Modele.DTO.Request;
    using Modele.DTO.Response;

    public interface IUtilisateurService
    {
        public Task<BaseResponse<RegistrationResponse>> Register(UserRegistrationRequestDto user);
        public Task<BaseResponse<RegistrationResponse>> Login(UserLoginRequest user);
       

        public Task<BaseResponse> Update(UserUpdateRequest user);

        public Task<BaseResponse> Delete();

        public Task<BaseResponse<UserInfoReponse>> GetUsersInfo();
    }
}