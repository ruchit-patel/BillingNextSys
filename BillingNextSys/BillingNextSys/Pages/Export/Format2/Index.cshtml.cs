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
using MoreLinq;
using NonFactors.Mvc.Grid;
using OfficeOpenXml;

namespace BillingNextSys.Pages.Export.Format2
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        private readonly BillingNextSys.Models.BillingNextSysContext _context;

        public IndexModel(BillingNextSys.Models.BillingNextSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IGrid<Models.Report2> gridd;

        public void OnGet()
        {
            ViewData["CompanyID"] = new SelectList(_context.Company, "CompanyID", "CompanyName");
            gridd = CreateExportableGrid();
        }

        public IActionResult OnPost()
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<Models.Report2> grid = CreateExportableGrid();
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


        public IGrid<Models.Report2> CreateExportableGrid()
        {
            //var result = _context.Report2s.FromSqlRaw("SELECT \"Bill\".\"BilledTo\",\"Bill\".\"DebtorGroupID\",\"Bill\".\"InvoiceDate\",\"BillDetails\".\"BillNumber\",sum(\"BillDetails\".\"BillAmountOutstanding\") As \"OutstandingAmount\" FROM \"BillDetails\"  INNER JOIN \"Bill\" ON \"BillDetails\".\"BillNumber\"=\"Bill\".\"BillNumber\" GROUP BY \"BillDetails\".\"BillNumber\",\"Bill\".\"InvoiceDate\",\"Bill\".\"BilledTo\",\"Bill\".\"DebtorGroupID\" ;");
            var result = _context.Bill.Join(
                _context.BillDetails,
                b => b.BillNumber,
                bd => bd.BillNumber,
                (b, bd) => new
                {
                    BilledTo = b.BilledTo,
                    DebtorGroupID = b.DebtorGroupID,
                    OutstandingAmount = _context.BillDetails.Where(x => x.BillNumber.Equals(b.BillNumber)).Sum(x => x.BillAmountOutstanding),
                    BillNumber = b.BillNumber,
                    InvoiceDate = b.InvoiceDate,
                    CompanyName = _context.Company.Where(x => x.CompanyID.Equals(b.CompanyID)).Select(x => x.CompanyName).FirstOrDefault()
                }).Select(x => new Models.Report2 { BilledTo = x.BilledTo, DebtorGroupID = x.DebtorGroupID, OutstandingAmount = x.OutstandingAmount, BillNumber = x.BillNumber, InvoiceDate = x.InvoiceDate, CompanyName = x.CompanyName }).DistinctBy(x=>x.BillNumber);

            IGrid<Models.Report2> grid = new Grid<Models.Report2>(result);
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };
            grid.Query = Request.Query;
        
            grid.Columns.Add(model => model.BilledTo).Titled("Client Name");
            grid.Columns.Add(model => model.DebtorGroupID).Titled("Client Code");
            grid.Columns.Add(model => model.OutstandingAmount).Titled("Outstanding Amount");
            grid.Columns.Add(model => model.BillNumber).Titled("Invoice Reference Number");
            grid.Columns.Add(model => model.InvoiceDate).Titled("Invoice date").Formatted("{0:d}");
            grid.Columns.Add(model => model.CompanyName).Titled("Company Name");

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }
    }

}
