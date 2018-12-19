using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BillingNextSys.Models
{
    public enum YearInfo
    {
        [Display(Name = "F.Y.")]
        FY,
        Q1,
        Q2,
        Q3,
        Q4
    }
    public class BillDetails
    {
        public int BillDetailsID { get; set; }

        [Required(ErrorMessage = "Please Specify Particulars")]
        [Display(Name = "Particulars")]
        public string ParticularsName { get; set; }

        [Required(ErrorMessage = "Please Specify Financial Year")]
        [Display(Name = "Period")]
        public string Period { get; set; }

        [Required(ErrorMessage = "Please Specify Year")]
        [Display(Name = "Year")]
        public YearInfo? YearInfo { get; set; }

        [Required(ErrorMessage = "Please Specify Amount for the respective particulars")]
        [Display(Name = "Amount")]
        [DataType(DataType.Currency)]
        public double Amount { get; set; }

        [Display(Name = "CGST")]
        [DataType(DataType.Currency)]
        public double CGSTAmount { get; set; }

        [Display(Name = "SGST")]
        [DataType(DataType.Currency)]
        public double SGSTAmount { get; set; }

        [Display(Name = "Taxable Value")]
        [DataType(DataType.Currency)]
        public double TaxableValue { get; set; }

        [Display(Name = "Bill Amount Outstanding")]
        [DataType(DataType.Currency)]
        public double BillAmountOutstanding { get; set; }

        public int CompanyID { get; set; }

        public Company Company { get; set; }

        public int ParticularsID { get; set; }
        public Particulars Particulars { get; set; }

        public string BillNumber { get; set; }
        public Bill Bill { get; set; }

        public int DebtorID { get; set; }
        public Debtor Debtor { get; set; }

        public int DebtorGroupID { get; set; }
        public DebtorGroup DebtorGroup { get; set; }

        public ICollection<Received> Receiveds { get; set; }

    }
}
