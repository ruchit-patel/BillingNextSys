using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillingNextSys.Models
{
    public class BillSeries
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name ="Series Name")]
       public string SeriesName { get; set; }

        public ICollection<Bill> Bills { get; set; }
    }
}
