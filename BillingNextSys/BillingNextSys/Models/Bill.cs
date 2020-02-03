using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillingNextSys.Models
{
    public enum PlaceOfSupply
    {
        Gujarat,

        [Display(Name = "Andhra Pradesh")]
        AndhraPradesh,

        [Display(Name = "Arunachal Pradesh")]
        ArunachalPradesh,

        Assam,
        Bihar,
        Chhattisgarh,
        Goa,
       
        Haryana,

        [Display(Name = "Himachal Pradesh")]
        HimachalPradesh,

        [Display(Name = "Jammu And Kashmir")]
        JammuAndKashmir,

        Jharkhand,
        Karnataka,
        Kerala,

        [Display(Name = "Madhya Pradesh")]
        MadhyaPradesh,

        Maharashtra,
        Manipur,
        Meghalaya,
        Mizoram,
        Nagaland,
        Odisha,
        Punjab,
        Rajasthan,
        Sikkim,

        [Display(Name = "Tamil Nadu")]
        TamilNadu,

        Telangana,
        Tripura,

        [Display(Name = "Uttar Pradesh")]
        UttarPradesh,

        Uttarakhand,
        WestBengal,

        [Display(Name = "Andaman and Nicobar Islands")]
        AndamanAndNicobar,

        Chandigarh,

        [Display(Name = "Dadar and Nagar Haveli")]
        Dadar,

        [Display(Name = "Daman And Diu")]
        Daman,

        Delhi,
        Lakshadweep,
        Puducherry
    }
    public class Bill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required(ErrorMessage = "Please Specify Invoice Number")]
        [Display(Name = "Invoice Number")]
        public string BillNumber { get; set; } // change to int if plain number

        [Required(ErrorMessage = "Please Specify whom are you billing to")]
        [Display(Name = "Billed To")]
        public string BilledTo { get; set; }

        [Display(Name = "Total Bill Amount")]
        [DataType(DataType.Currency)]
        public double BillAmount{ get; set; }

        [StringLength(500)]
        [Display(Name = "Note:")]
        public string Note { get; set; }

        [Display(Name = "Invoice Date")]
        [DataType(DataType.Date)]
        public DateTime InvoiceDate { get; set; }

        [Display(Name = "Place Of Supply")]
        public PlaceOfSupply? PlaceOfSupply { get; set; }

        [DataType(DataType.Date)]
        public DateTime BillDate { get; set; }

        public int? BillActNum { get; set; }

        public int DebtorGroupID { get; set; }

        [ForeignKey("DebtorGroupID")]
        public DebtorGroup DebtorGroup { get; set; }

        public int CompanyID { get; set; }

        [ForeignKey("CompanyID")]
        public Company Company { get; set; }

        public int BranchID { get; set; }

        [ForeignKey("BranchID")]
        public Branch Branch { get; set; }

        public string SeriesName { get; set; }

        [ForeignKey("SeriesName")]
        public BillSeries BillSeries { get; set; }

        public int SecretUnlockCode { get; set; }

        public bool BillDelivered { get; set; }

        public bool? MessageSent { get; set; }

        public ICollection<BillDetails> BillDetails { get; set; }
    }
}
