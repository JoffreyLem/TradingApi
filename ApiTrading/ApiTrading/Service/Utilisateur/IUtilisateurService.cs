using System.Threading.Tasks;
using ApiTrading.Modele.DTO.Request;
using ApiTrading.Modele.DTO.Response;

namespace ApiTrading.Service.Utilisateur
{
    public interface IUtilisateurService
    {
        public Task<BaseResponse<RegistrationResponse>> Register(UserRegistrationRequestDto user);
        public Task<BaseResponse<RegistrationResponse>> Login(UserLoginRequest user);


        public Task<BaseResponse> Update(UserUpdateRequest user);

        public Task<BaseResponse> Delete();

        public Task<BaseResponse<UserInfoReponse>> GetUsersInfo();
    }
}