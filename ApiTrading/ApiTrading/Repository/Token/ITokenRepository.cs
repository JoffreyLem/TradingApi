namespace ApiTrading.Repository.Token
{
    using System.Threading.Tasks;
    using Modele;

    public interface ITokenRepository
    {
        public Task AddToken(RefreshToken token);
    }
}