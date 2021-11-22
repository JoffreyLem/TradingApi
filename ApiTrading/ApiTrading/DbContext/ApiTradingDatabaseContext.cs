using System.Security.Principal;
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






        protected override void OnModelCreating(ModelBuilder builder)
        {
            int stringMaxLength =100 /* something like 100*/;
            // User IdentityRole and IdentityUser in case you haven't extended those classes
            builder.Entity<IdentityRole>(x => x.Property(m => m.Name).HasMaxLength(stringMaxLength));
            builder.Entity<IdentityRole>(x => x.Property(m => m.NormalizedName).HasMaxLength(stringMaxLength));
            builder.Entity<IdentityUser>(x => x.Property(m => m.NormalizedUserName).HasMaxLength(stringMaxLength));

            // We are using int here because of the change on the PK
            builder.Entity<IdentityUserLogin<int>>(x => x.Property(m => m.LoginProvider).HasMaxLength(stringMaxLength));
            builder.Entity<IdentityUserLogin<int>>(x => x.Property(m => m.ProviderKey).HasMaxLength(stringMaxLength));

            // We are using int here because of the change on the PK
            builder.Entity<IdentityUserToken<int>>(x => x.Property(m => m.LoginProvider).HasMaxLength(stringMaxLength));
            builder.Entity<IdentityUserToken<int>>(x => x.Property(m => m.Name).HasMaxLength(stringMaxLength));
            base.OnModelCreating(builder);
        }
    }
}