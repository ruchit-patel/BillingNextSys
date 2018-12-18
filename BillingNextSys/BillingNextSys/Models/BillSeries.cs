using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BillingNextSys.Models
{
    public class BillSeries
    {
        [Key]
        [Display(Name ="Series Name")]
       public string SeriesName { get; set; }

        public ICollection<Bill> Bills { get; set; }
    }
}
