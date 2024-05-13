namespace DerivcoAssessment.Data
{
    using DerivcoAssessment.Models;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Bet> Bets => Set<Bet>();

        public DbSet<Spin> Spins => Set<Spin>();

        public DbSet<Payout> Payouts => Set<Payout>();
    }
}
