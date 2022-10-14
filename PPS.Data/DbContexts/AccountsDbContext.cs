using PPS.Data.Entities.Bank;
using PPS.Data.Entities.Client;
using PPS.Data.Entities.GroupOfCompany;
using PPS.Data.Entities.Ledger;
using PPS.Data.Entities.Transaction;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace PPS.Data.DbContexts
{
    public class AccountsDbContext : DbContext
    {
        public AccountsDbContext() : base("UserDbContext")
        { }

        public DbSet<ControlType> ControlType { get; set; }
        public DbSet<TransactionType> TransactionType { get; set; }
        public DbSet<TransactionStatus> TransactionStatus { get; set; }
        public DbSet<TransactionEntry> TransactionEntry { get; set; }
        public DbSet<TransactionDetail> TransactionDetail { get; set; }

        public DbSet<Company> Company { get; set; }
        public DbSet<FiscalYear> FiscalYear { get; set; }

        public DbSet<BankInfo> BankInfo { get; set; }
        public DbSet<ClientInfo> ClientInfo { get; set; }

        public DbSet<LedgerEntry> LedgerEntry { get; set; }
        public DbSet<LedgerOpening> LedgerOpening { get; set; }
        public DbSet<SystemGroup> SystemGroup { get; set; }
        public DbSet<MainGroup> MainGroup { get; set; }
        public DbSet<SubGroup> SubGroup { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}