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

namespace BillingNextSys.Pages.Export.Format1
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
        public IGrid<Models.Report1> gridd;
       
        public void OnGet()
        {
            
            gridd= CreateExportableGrid();

        }

        public IActionResult OnPost()
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<Models.Report1> grid = CreateExportableGrid();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<Models.Report1> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }


        public  IGrid<Models.Report1> CreateExportableGrid()
        {
            var result = _context.Report1s.FromSqlRaw(" SELECT \"Bill\".\"BillNumber\",\"Bill\".\"InvoiceDate\",\"BillDetails\".\"ParticularsName\",\"Bill\".\"BilledTo\",\"Bill\".\"DebtorGroupID\",\"DebtorGroup\".\"DebtorGSTIN\",\"BillDetails\".\"YearInfo\",\"BillDetails\".\"TaxableValue\",\"BillDetails\".\"CGSTAmount\",\"BillDetails\".\"SGSTAmount\",\"BillDetails\".\"Amount\",\"BillDetails\".\"CompanyID\",\"BillDetails\".\"Period\" FROM \"Bill\" INNER JOIN \"BillDetails\" ON \"Bill\".\"BillNumber\" = \"BillDetails\".\"BillNumber\" INNER JOIN \"DebtorGroup\" ON \"BillDetails\".\"DebtorGroupID\" = \"DebtorGroup\".\"DebtorGroupID\" ");

            IGrid<Models.Report1> grid = new Grid<Models.Report1>(result);
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };
            grid.Query = Request.Query;

            grid.Columns.Add(model => model.BillNumber).Titled("Invoice Number");
            grid.Columns.Add(model => model.InvoiceDate).Titled("Invoice date").Formatted("{0:d}");
            grid.Columns.Add(model => model.ParticularsName).Titled("Service Particulars");
            grid.Columns.Add(model => model.BilledTo).Titled("Billed To");
            grid.Columns.Add(model => model.DebtorGroupID).Titled("Client Code");
            grid.Columns.Add(model => model.DebtorGSTIN).Titled("GSTIN No.");
            grid.Columns.Add(model => model.YearInfo).Titled("Period");
            grid.Columns.Add(model => model.CGSTAmount).Titled("CGST");
            grid.Columns.Add(model => model.SGSTAmount).Titled("SGST");
            grid.Columns.Add(model => model.Amount).Titled("Invoice Amount");
            grid.Columns.Add(model => model.CompanyID).Titled("Company Id");
            grid.Columns.Add(model => model.Period).Titled("Period");

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }
    }

}
