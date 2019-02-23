using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NonFactors.Mvc.Grid;
using OfficeOpenXml;

namespace BillingNextSys.Pages.Export.Format2
{
    [Authorize(Roles = "Admin")]
    public class CompanySortModel : PageModel
    {
       

            private readonly IHttpContextAccessor _httpContextAccessor;
            private ISession _session => _httpContextAccessor.HttpContext.Session;

            private readonly BillingNextSys.Models.BillingNextSysContext _context;

            public CompanySortModel(BillingNextSys.Models.BillingNextSysContext context, IHttpContextAccessor httpContextAccessor)
            {
                _context = context;
                _httpContextAccessor = httpContextAccessor;
            }
            public IGrid<Models.Report2> gridd;
        public int Cid;
        public void OnGet(int companyid)
            {
            Cid = companyid;
               ViewData["CompanyID"] = new SelectList(_context.Company, "CompanyID", "CompanyName");
               gridd = CreateExportableGrid(companyid);
            }

            public IActionResult OnPost()
            {
                int cid=Int32.Parse(Request.Form["compid"].ToString());
            using (ExcelPackage package = new ExcelPackage())
                {
                    Int32 row = 2;
                    Int32 col = 1;

                    package.Workbook.Worksheets.Add("Data");
                    IGrid<Models.Report2> grid = CreateExportableGrid(cid);
                    ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                    foreach (IGridColumn column in grid.Columns)
                    {
                        sheet.Cells[1, col].Value = column.Title;
                        sheet.Column(col++).Width = 18;

                        column.IsEncoded = false;
                    }

                    foreach (IGridRow<Models.Report2> gridRow in grid.Rows)
                    {
                        col = 1;
                        foreach (IGridColumn column in grid.Columns)
                            sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                        row++;
                    }

                    return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
                }
            }


            public IGrid<Models.Report2> CreateExportableGrid(int ccid)
            {
                var result = _context.Report2s.FromSql("SELECT \"Bill\".\"BilledTo\",\"Bill\".\"DebtorGroupID\",\"Bill\".\"InvoiceDate\",\"BillDetails\".\"BillNumber\",sum(\"BillDetails\".\"BillAmountOutstanding\") As \"OutstandingAmount\" FROM \"BillDetails\"  INNER JOIN \"Bill\" ON \"BillDetails\".\"BillNumber\"=\"Bill\".\"BillNumber\" where \"Bill\".\"CompanyID\"="+ccid+" GROUP BY \"BillDetails\".\"BillNumber\",\"Bill\".\"InvoiceDate\",\"Bill\".\"BilledTo\",\"Bill\".\"DebtorGroupID\" ;");

                IGrid<Models.Report2> grid = new Grid<Models.Report2>(result);
                grid.ViewContext = new ViewContext { HttpContext = HttpContext };
                grid.Query = Request.Query;

                grid.Columns.Add(model => model.BilledTo).Titled("Client Name");
                grid.Columns.Add(model => model.DebtorGroupID).Titled("Client Code");
                grid.Columns.Add(model => model.OutstandingAmount).Titled("Outstanding Amount");
                grid.Columns.Add(model => model.BillNumber).Titled("Invoice Reference Number");
                grid.Columns.Add(model => model.InvoiceDate).Titled("Invoice date").Formatted("{0:d}");
       
            foreach (IGridColumn column in grid.Columns)
                {
                    column.Filter.IsEnabled = false;
                    column.Sort.IsEnabled = false;
                }

                return grid;
            }
        }

}
