using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BillingNextSys.Models
{
    public class Particulars
    {
        public int ParticularsID { get; set; }

        [Required(ErrorMessage = "Please Specify Particulars Name"), StringLength(150)]
        [Display(Name = "Particulars")]
        public string ParticularsName { get; set; }
       
       [Display(Name = "SAC Code")]
       [Required(ErrorMessage ="Please Specify SAC Code for the Particular")]
        public int SACCode { get; set; }

        [Display(Name = "Amount For the Particular (Optional. For Auto Fill.)")]
        public double Amount { get; set; }

        public ICollection<BillDetails> BillDetails { get; set; }
    }
}
