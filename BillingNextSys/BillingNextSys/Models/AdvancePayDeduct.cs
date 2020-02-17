using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BillingNextSys.Models
{
    public class AdvancePayDeduct
    {
        [Key]
        [Display(Name = "Advance Pay Deduct ID")]
        public int AdvancePayDeductID { get; set; }

        [Required(ErrorMessage = "Please provide advance deduct Amount")]
        [DataType(DataType.Currency)]
        public double AdvanceAmountDeducted{ get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Please provide deduct date")]
        public DateTime DeductDate { get; set; }

        public int DebtorGroupID { get; set; }

        [ForeignKey("DebtorGroupID")]
        public DebtorGroup DebtorGroup { get; set; }

        public int BillDetailsID { get; set; }

        [ForeignKey("BillDetailsID")]
        public BillDetails BillDetails { get; set; }

        public int CompanyID { get; set; }

        [ForeignKey("CompanyID")]
        public Company Company { get; set; }

        public int BranchID { get; set; }

        [ForeignKey("BranchID")]
        public Branch Branch { get; set; }
    }
}
