using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BillingNextSys.Models
{
    public class AdvancePay
    {
        [Key]
        [Display(Name = "Advance Pay ID")]
        public int AdvancePayID { get; set; }
        
        [Required(ErrorMessage ="Please provide advance Amount")]
        [DataType(DataType.Currency)]
        public double AdvanceAmount { get;set; }

        [Display(Name = "Is It a Cheque Payment?")]
        public bool ChequePaymet { get; set; }

        [Display(Name = "Cheque Number")]
        public string ChequeNumber { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage ="Please provide received date")]
        public DateTime ReceivedDate { get; set; }

        public int DebtorGroupID { get; set; }

        public DebtorGroup DebtorGroup { get; set; }

        public int CompanyID { get; set; }

        public Company Company { get; set; }

        public int BranchID { get; set; }

        public Branch Branch { get; set; }

    }
}
