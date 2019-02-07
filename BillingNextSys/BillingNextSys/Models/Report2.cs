using System;
namespace BillingNextSys.Models
{
    public class Report2
    {
        public string BilledTo { get; set; }

        public int DebtorGroupID { get; set; }

        public double OutstandingAmount { get; set; }

        public string BillNumber { get; set; }

        public DateTime InvoiceDate { get; set; }
    }
}
