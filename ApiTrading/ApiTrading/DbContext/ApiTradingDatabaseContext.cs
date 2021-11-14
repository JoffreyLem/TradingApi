using ApiTrading.Modele;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiTrading.DbContext
{
    public class ApiTradingDatabaseContext :  IdentityDbContext<IdentityUser<int>,IdentityRole<int>,int>
    {
     
        public virtual DbSet<RefreshToken> RefreshTokens {get;set;}
        public ApiTradingDatabaseContext(DbContextOptions<ApiTradingDatabaseContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        
 
        base.OnConfiguring(optionsBuilder);

        }
        
        
        
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            base.OnModelCreating(modelBuilder);
        }
    }
}