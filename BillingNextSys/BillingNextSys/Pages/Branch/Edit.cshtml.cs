using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BillingNextSys.Models;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace BillingNextSys.Pages.Branch
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public EditModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Branch Branch { get; set; }

        public static byte[] Img;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Branch = await _context.Branch
                .Include(b => b.Company).FirstOrDefaultAsync(m => m.BranchID == id);

            if (Branch == null)
            {
                return NotFound();
            }
            Img = Branch.BranchManagerSign;
            ViewData["CompanyID"] = new SelectList(_context.Company, "CompanyID", "CompanyName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
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
            else
            {
                Branch.BranchManagerSign = Img;
            }

            _context.Attach(Branch).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BranchExists(Branch.BranchID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool BranchExists(int id)
        {
            return _context.Branch.Any(e => e.BranchID == id);
        }
    }
}
