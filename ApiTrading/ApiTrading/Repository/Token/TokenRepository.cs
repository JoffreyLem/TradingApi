using System.Threading.Tasks;
using ApiTrading.DbContext;
using ApiTrading.Modele;

namespace ApiTrading.Repository.Token
{
    public class TokenRepository : GenericRepository<RefreshToken>, ITokenRepository
    {
        public TokenRepository(ApiTradingDatabaseContext context) : base(context)
        {
        }

        public async Task AddToken(RefreshToken token)
        {
            await Context.RefreshTokens.AddAsync(token);
            await SaveChangeAsync();
        }
    }
}