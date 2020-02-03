using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillingNextSys.Models
{
    public class Debtor
    {
        public int DebtorID { get; set; }

        [Required(ErrorMessage = "Please Specify Debtor Name"), StringLength(150)]
        [Display(Name = "Debtor Name")]
        public string DebtorName { get; set; }

        [Required(ErrorMessage = "Please Specify Debtor Outstanding Amount. Fill 0 if not outstanding")]
        [Display(Name = "Debtor Outstanding Amount")]
        [DataType(DataType.Currency)]
        public double DebtorOutstanding { get; set; }

        public int DebtorGroupID { get; set; }

        [ForeignKey("DebtorGroupID")]
        public DebtorGroup DebtorGroup { get; set; }

        public ICollection<BillDetails> BillDetails { get; set; }

    }
}
