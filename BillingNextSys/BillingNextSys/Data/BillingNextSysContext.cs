using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BillingNextSys.Models;

namespace BillingNextSys.Models
{
    public class BillingNextSysContext : DbContext
    {
        public BillingNextSysContext (DbContextOptions<BillingNextSysContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bill>()
            .HasIndex(p => new { p.BillActNum}).IsUnique();

            modelBuilder.Entity<BillDetails>()
             .HasOne(p => p.Bill)
             .WithMany(b => b.BillDetails)
             .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<Report1>().ToView("vw_Report1").HasNoKey();
            modelBuilder.Entity<Report2>().ToView("vw_Report2").HasNoKey();
        }

        public DbSet<BillingNextSys.Models.Company> Company { get; set; }

        public DbSet<BillingNextSys.Models.Branch> Branch { get; set; }

        public DbSet<BillingNextSys.Models.DebtorGroup> DebtorGroup { get; set; }

        public DbSet<BillingNextSys.Models.Debtor> Debtor { get; set; }

        public DbSet<BillingNextSys.Models.Bill> Bill { get; set; }

        public DbSet<BillingNextSys.Models.BillDetails> BillDetails { get; set; }

        public DbSet<BillingNextSys.Models.Received> Received { get; set; }

        public DbSet<BillingNextSys.Models.Particulars> Particulars { get; set; }

        public DbSet<BillingNextSys.Models.BillSeries> BillSeries { get; set; }

        public DbSet<BillingNextSys.Models.AdvancePay> AdvancePay { get; set; }

        public DbSet<BillingNextSys.Models.AdvancePayDeduct> AdvancePayDeduct { get; set; }

        public DbSet<BillingNextSys.Models.Report1> Report1s { get; set; }

        public DbSet<BillingNextSys.Models.Report2> Report2s { get; set; }
    }
}
