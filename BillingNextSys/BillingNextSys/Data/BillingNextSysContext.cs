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

        public DbSet<BillingNextSys.Models.Company> Company { get; set; }

        public DbSet<BillingNextSys.Models.Branch> Branch { get; set; }

        public DbSet<BillingNextSys.Models.DebtorGroup> DebtorGroup { get; set; }

        public DbSet<BillingNextSys.Models.Debtor> Debtor { get; set; }

        public DbSet<BillingNextSys.Models.Bill> Bill { get; set; }

        public DbSet<BillingNextSys.Models.BillDetails> BillDetails { get; set; }

        public DbSet<BillingNextSys.Models.Received> Received { get; set; }

        public DbSet<BillingNextSys.Models.Particulars> Particulars { get; set; }

        public DbSet<BillingNextSys.Models.BillSeries> BillSeries { get; set; }
    }
}
