using System;
namespace BillingNextSys.Models
{
    public class Report1
    {

        public string BillNumber { get; set; } // change to int if plain number

        public DateTime InvoiceDate { get; set; }

        public string ParticularsName { get; set; }

        public string BilledTo { get; set; }

        public int DebtorGroupID { get; set; }

        public string DebtorGSTIN { get; set; }

        public YearInfo? YearInfo { get; set; }

        public double TaxableValue { get; set; }

        public double CGSTAmount { get; set; }

        public double SGSTAmount { get; set; }

        public double Amount { get; set; }

        public int CompanyID { get; set; }
    }
}
