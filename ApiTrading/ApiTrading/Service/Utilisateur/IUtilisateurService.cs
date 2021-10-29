using System.Threading.Tasks;
using ApiTrading.Modele.DTO.Request;
using ApiTrading.Modele.DTO.Response;
using Microsoft.AspNetCore.Mvc;

namespace ApiTrading.Service.Utilisateur
{
    public interface IUtilisateurService
    {
        
        public Task SendMessageRegistration();
        public Task<RegistrationResponse> Register([FromBody] UserRegistrationRequestDto user);
        public Task<RegistrationResponse> Login([FromBody] UserLoginRequest user);
    }
}