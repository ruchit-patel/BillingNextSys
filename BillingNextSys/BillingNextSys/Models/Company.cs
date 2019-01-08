using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace BillingNextSys.Models
{
    public enum BillFormat
    {
        Format1,
        Format2
    }

    public enum AccountType
    {
        [Display(Name = "Current Account")]
        CurrentAcc,

        [Display(Name = "Saving Account")]
        SavingAcc
    }


    public class Company
    {
        public int CompanyID { get; set; }

        [Required(ErrorMessage = "Please Specify Company Name"), StringLength(100)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreationDate { get; set; }

      
        public byte[] CompanyLogoImg { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        [Display(Name = "Company Logo")]
        public IFormFile CompLogo { get; set; }

        public BillFormat? BillFormat { get; set; }

        [Display(Name = "GST Identification Number")]
        [Required(ErrorMessage = "Please Specify GST Identification Number")]
        [RegularExpression(@"\d{2}[A-Z]{5}\d{4}[A-Z]{1}\d[Z]{1}[A-Z\d]{1}", ErrorMessage = "Not A Valid GST Identification Number")]
        public string GSTIN { get; set; }

        [Required(ErrorMessage = "Please Specify Bank Name... It will be printed on bill"), StringLength(100)]
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [Required(ErrorMessage = "Please Specify Account Type... It will be printed on bill")]
        [Display(Name = "Account Type")]
        public AccountType? AccountType { get; set; }

        [RegularExpression(@"^\d{9,18}$", ErrorMessage = "Not A Valid Bank Account Number")]
        [Required(ErrorMessage = "Please Specify Account Number... It will be printed on bill")]
        [Display(Name = "Bank Account Number")]
        public string AccountNumber { get; set; }

        [RegularExpression(@"^[A-Za-z]{4}[a-zA-Z0-9]{7}$", ErrorMessage = "Not A Valid IFSC Code")]
        [Display(Name = "IFSC Code")]
        [Required(ErrorMessage = "Please Specify IFSC Code... It will be printed on bill")]
        public string IFSCcode { get; set; }

        [RegularExpression(@"^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$", ErrorMessage = "Not A Valid PAN")]
        [Display(Name = "Company's PAN")]
        [Required(ErrorMessage = "Please Specify PAN.")]
        public string PAN { get; set; }

        [Display(Name = "Company's Owner")]
        [Required(ErrorMessage = "Please Specify Company Owner."),StringLength(100)]
        public string CompanyOwner { get; set; }

        public ICollection<Branch> Branches { get; set; }
        public ICollection<Bill> Bills { get; set; }
        public ICollection<BillDetails> BillDetails { get; set; }
        public ICollection<Received> Receiveds { get; set; }
    }
}
