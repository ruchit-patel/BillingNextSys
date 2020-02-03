using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillingNextSys.Models
{
    public class Received
    {

       public int ReceivedID { get; set; }

        [Display(Name = "Received Amount")]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Please Specify Received Amount")]
        public double ReceivedAmount { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ReceivedDate { get; set; }

        [Display(Name ="Is It a Cheque Payment?")]
        public bool ChequePaymet { get; set; }

        [Display(Name = "Cheque Number")]
        public string ChequeNumber { get; set; }

        public int DebtorGroupID { get; set; }

        [ForeignKey("DebtorGroupID")]
        public DebtorGroup DebtorGroup { get; set; }

        public int CompanyID { get; set; }

        [ForeignKey("CompanyID")]
        public Company Company { get; set; }

        public int BillDetailsID { get; set; }

        [ForeignKey("BillDetailsID")]
        public BillDetails BillDetails { get; set; }
    }
}
