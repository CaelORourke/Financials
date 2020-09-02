namespace Financials.Data
{
    using Financials.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class FinancialsContext : DbContext
    {
        public FinancialsContext(DbContextOptions<FinancialsContext> options)
            : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>().Property(e => e.TransactionId).HasColumnName("id");
            modelBuilder.Entity<Transaction>().Property(e => e.TransactionValue).HasColumnName("transaction_value");
            modelBuilder.Entity<Transaction>().Property(e => e.SoftDelete).HasColumnName("soft_delete");
        }
    }
}
