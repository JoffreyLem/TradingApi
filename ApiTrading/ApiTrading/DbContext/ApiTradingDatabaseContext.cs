using ApiTrading.Modele;
using Microsoft.EntityFrameworkCore;

namespace ApiTrading.DbContext
{
    public class ApiTradingDatabaseContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Test> Tests { get; set; }
        
        public ApiTradingDatabaseContext(DbContextOptions<ApiTradingDatabaseContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "server=172.18.0.2;user=root;password=root1;database=ApiTrading";
 
    

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Test>().HasNoKey();
        }
    }
}