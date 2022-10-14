using PPS.Data.Edmx;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PPS.Data.DbContexts
{
    public class LogContext : DbContext
    {
        public LogContext() : base("PPSDbContext")
        { }

        public DbSet<Log> Log { get; set; }
        public DbSet<UserActivityLog> UserActivityLog { get; set; }
        public DbSet<UserLoginLog> UserLoginLog { get; set; }
    }
}