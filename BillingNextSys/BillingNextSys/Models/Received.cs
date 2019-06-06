using System;
using System.ComponentModel.DataAnnotations;

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
        public DebtorGroup DebtorGroup { get; set; }

        public int CompanyID { get; set; }
        public Company Company { get; set; }

        public int BillDetailsID { get; set; }
        public BillDetails BillDetails { get; set; }
    }
}
