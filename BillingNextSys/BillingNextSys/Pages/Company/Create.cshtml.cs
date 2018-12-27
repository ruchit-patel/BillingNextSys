using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BillingNextSys.Models;
using System.IO;

namespace BillingNextSys.Pages.Company
{
    public class CreateModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public CreateModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Models.Company Company { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            Company.CreationDate = DateTime.Now;
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Company.CompLogo != null)
            {
                if (Company.CompLogo.Length > 0)

                //Convert Image to byte and save to database

                {
                    byte[] p1 = null;
                    using (var fs1 = Company.CompLogo.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    Company.CompanyLogoImg= p1;

                }
            }
            _context.Company.Add(Company);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}