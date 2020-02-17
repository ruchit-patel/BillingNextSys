using NonFactors.Mvc.Lookup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillingNextSys.Models
{
    public class DebtorGroup
    {
        [Key]
        public int DebtorGroupID { get; set; }

        [NotMapped]
        public Int32 Id { get; set; }

        [LookupColumn]
        [Required(ErrorMessage = "Please Specify Debtor Group Name"), StringLength(150)]
        [Display(Name = "Debtor Group Name")]
        public string DebtorGroupName { get; set; }

        [LookupColumn]
        [Required(ErrorMessage = "Please Specify Debtor Group Address"), StringLength(350)]
        [Display(Name = "Debtor Group Address")]
        [DataType(DataType.MultilineText)]
        public string DebtorGroupAddress { get; set; }

        [LookupColumn]
        [Required(ErrorMessage = "Please Specify Debtor Group Email Address")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Debtor Group Email Address")]
        public string DebtorGroupMail { get; set; }

        [LookupColumn]
        [Required(ErrorMessage = "Please Specify Debtor Group Contact Number. Messages will be sent via this number")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Debtor Group Contact Number.")]
        public string DebtorGroupPhoneNumber { get; set; }

        [Required(ErrorMessage = "Please Specify Debtor Group City.")]
        [Display(Name = "Debtor Group City.")]
        public string DebtorGroupCity { get; set; }

        [LookupColumn]
        [RegularExpression(@"\d{2}[A-Z]{5}\d{4}[A-Z]{1}\d[Z]{1}[A-Z\d]{1}", ErrorMessage = "Not A Valid GST Identification Number")]
        [Display(Name = "Debtor GSTIN")]
        public string DebtorGSTIN { get; set; }

        [LookupColumn]
        [Required(ErrorMessage = "Please Specify Debtor Outstanding Amount.Fill 0 if none outstanding")]
        [DataType(DataType.Currency)]
        [Display(Name = "Debtor Outstanding Amount")]
        public double DebtorOutstanding { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "AdvancePay Amount")]
        public double AdvancePayAmount { get; set; }

        public int BranchID { get; set; }

        [ForeignKey("BranchID")]
        public Branch Branch { get; set; }

        public ICollection<Debtor> Debtors { get; set; }
        public ICollection<Bill> Bills { get; set; }
        public ICollection<Received> Receiveds { get; set; }
        public ICollection<BillDetails> BillDetails { get; set; }
        public ICollection<AdvancePay> AdvancePays { get; set; }
        public ICollection<AdvancePayDeduct> AdvancePayDeducts { get; set; }
    }
}
