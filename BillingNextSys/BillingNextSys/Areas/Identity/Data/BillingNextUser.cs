using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BillingNextSys.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the BillingNextUser class
    public class BillingNextUser : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }
    }
}
