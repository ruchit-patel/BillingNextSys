using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillingNextSys.Models
{
    [NotMapped]
    public class AdHocReturn
    {
       public int last_value { get; set; }
    }
}
