using PPS.Data.Entities.Account;
using PPS.Data.Entities.Bank;
using PPS.Data.Entities.Client;
using PPS.Data.Entities.GroupOfCompany;
using PPS.Data.Entities.Ledger;
using PPS.Data.Entities.Transaction;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PPS.Data.DbContexts
{
    public class PPSDbContext : DbContext
    {
        public PPSDbContext() : base("PPSDbContext")
        { }

        public DbSet<Group> Group { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserStatus> UserStatus { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }

        public DbSet<ControlType> ControlType { get; set; }
        public DbSet<TransactionType> TransactionType { get; set; }
        public DbSet<TransactionStatus> TransactionStatus { get; set; }
        public DbSet<TransactionEntry> TransactionEntry { get; set; }
        public DbSet<TransactionDetail> TransactionDetail { get; set; }

        public DbSet<FiscalYear> FiscalYear { get; set; }

        public DbSet<BankInfo> BankInfo { get; set; }
        public DbSet<ClientInfo> ClientInfo { get; set; }
        
        public DbSet<AccountNature> AccountNature { get; set; }
        public DbSet<AccountType> AccountType { get; set; }
        public DbSet<AccountPrimaryHead> AccountPrimaryHead { get; set; }
        public DbSet<AccountHead> AccountHead { get; set; }
        public DbSet<AccountHeadOpening> AccountHeadOpening { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Entity<AccountHead>()
            //    .HasRequired(x => x.AccountPrimaryHead)
            //    .WithMany()
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<AccountHeadOpening>()
            //    .HasRequired(x => x.AccountHead)
            //    .WithMany()
            //    .WillCascadeOnDelete(false);

        }
    }
}