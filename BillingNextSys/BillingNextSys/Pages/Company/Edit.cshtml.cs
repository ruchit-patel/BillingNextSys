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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BillingNextSys.Pages.Company
{
    public class EditModel : PageModel
    {
        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public EditModel(BillingNextSys.Models.BillingNextSysContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Company Company { get; set; }

        public static byte[] Img;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Company = await _context.Company.FirstOrDefaultAsync(m => m.CompanyID == id);

            if (Company == null)
            {
                return NotFound();
            }
            Img = Company.CompanyLogoImg;
            return Page();
        }

        //public IEnumerable<Models.Company> Companies { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
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
                    Company.CompanyLogoImg = p1;
                }
            }
            else
            {
                Company.CompanyLogoImg = Img;
            }


            _context.Attach(Company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(Company.CompanyID))
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

        private bool CompanyExists(int id)
        {
            return _context.Company.Any(e => e.CompanyID == id);
        }
    }
}
