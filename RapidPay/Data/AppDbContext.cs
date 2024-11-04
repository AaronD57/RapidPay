namespace RapidPay.Data
{
    using Microsoft.EntityFrameworkCore;
    using RapidPay.Models;

    public class AppDbContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<FeeRate> FeeRates { get; set; } = null!;
        public DbSet<User> Users { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>().HasKey(c => c.CardId);
            modelBuilder.Entity<Transaction>().HasKey(t => t.TransactionId);
            modelBuilder.Entity<FeeRate>().HasKey(f => f.FeeRateId);

          
            modelBuilder.Entity<Card>().HasIndex(c => c.CardNumber).IsUnique();
        }
    }

}
