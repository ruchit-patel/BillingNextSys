using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BillingNextSys.Models
{

    public class Branch
    {
        public int BranchID { get; set; }

        [Required(ErrorMessage = "Please Specify Branch Name"), StringLength(100)]
        [Display(Name = "Branch Name")]
        public string BranchName { get; set; }


        [Required(ErrorMessage = "Please Specify Branch Address"), StringLength(200)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Branch Address")]
        public string BranchAddress { get; set; }
      

        [ScaffoldColumn(false)]
        [DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; }

        public int CompanyID { get; set; }

        public Company Company { get; set; }

        public ICollection<DebtorGroup> DebtorGroups { get; set; }
    }
}
