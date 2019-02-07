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

        [Required(ErrorMessage = "Please Specify Branch Email"), StringLength(200)]
        [Display(Name = "Branch Email Address")]
        public string BranchEmail { get; set; }

        [Required(ErrorMessage = "Please Specify Branch Contact Number")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Branch Contact Number")]
        public string BranchPhone { get; set; }

        [ScaffoldColumn(false)]
        [DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; }

        public int CompanyID { get; set; }

        public Company Company { get; set; }

        public ICollection<DebtorGroup> DebtorGroups { get; set; }

        public ICollection<Bill> Bills { get; set; }
    }
}
