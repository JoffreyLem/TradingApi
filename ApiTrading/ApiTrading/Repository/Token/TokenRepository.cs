namespace ApiTrading.Repository.Token
{
    using System.Threading.Tasks;
    using DbContext;
    using Modele;

    public class TokenRepository : GenericRepository<RefreshToken>, ITokenRepository
    {
     

        public TokenRepository(ApiTradingDatabaseContext context) : base(context)
        {
        }
        
        public async Task AddToken(RefreshToken token)
        {
           await _context.RefreshTokens.AddAsync(token);
        }
    }
}