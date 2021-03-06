using ApiTrading.Modele;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Modele;

namespace ApiTrading.DbContext
{
    public class ApiTradingDatabaseContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {
        public ApiTradingDatabaseContext(DbContextOptions<ApiTradingDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<SignalInfoStrategy> SignalInfoStrategies { get; set; }

        public virtual DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            var stringMaxLength = 100;
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