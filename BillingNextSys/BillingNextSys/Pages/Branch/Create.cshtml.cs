using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BillingNextSys.Models;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace BillingNextSys.Pages.Branch
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public CreateModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CompanyID"] = new SelectList(_context.Company, "CompanyID", "CompanyName");
            return Page();
        }

        [BindProperty]
        public Models.Branch Branch { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            Branch.CreationDate = DateTime.Now;
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Branch.BranchManaSign != null)
            {
                if (Branch.BranchManaSign.Length > 0)

                //Convert Image to byte and save to database

                {
                    byte[] p1 = null;
                    using (var fs1 = Branch.BranchManaSign.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    Branch.BranchManagerSign = p1;

                }
            }

            _context.Branch.Add(Branch);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}