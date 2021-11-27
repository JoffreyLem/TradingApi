using System.Security.Principal;
using ApiTrading.Modele;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiTrading.DbContext
{
    using global::Modele;

    public class ApiTradingDatabaseContext :  IdentityDbContext<IdentityUser<int>,IdentityRole<int>,int>
    {
     
        public virtual DbSet<RefreshToken> RefreshTokens {get;set;}
        public virtual DbSet<SignalInfoStrategy> SignalInfoStrategies { get; set; }
        public ApiTradingDatabaseContext(DbContextOptions<ApiTradingDatabaseContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        
 
        base.OnConfiguring(optionsBuilder);

        }






        protected override void OnModelCreating(ModelBuilder builder)
        {
            int stringMaxLength = 100;
            builder.Entity<IdentityRole>(x => x.Property(m => m.Name).HasMaxLength(stringMaxLength));
            builder.Entity<IdentityRole>(x => x.Property(m => m.NormalizedName).HasMaxLength(stringMaxLength));
            builder.Entity<IdentityUser>(x => x.Property(m => m.NormalizedUserName).HasMaxLength(stringMaxLength));

           
            builder.Entity<IdentityUserLogin<int>>(x => x.Property(m => m.LoginProvider).HasMaxLength(stringMaxLength));
            builder.Entity<IdentityUserLogin<int>>(x => x.Property(m => m.ProviderKey).HasMaxLength(stringMaxLength));

          
            builder.Entity<IdentityUserToken<int>>(x => x.Property(m => m.LoginProvider).HasMaxLength(stringMaxLength));
            builder.Entity<IdentityUserToken<int>>(x => x.Property(m => m.Name).HasMaxLength(stringMaxLength));
            base.OnModelCreating(builder);
        }
    }
}