using System.Threading.Tasks;
using ApiTrading.Modele;

namespace ApiTrading.Repository.Token
{
    public interface ITokenRepository
    {
        public Task AddToken(RefreshToken token);
    }
}