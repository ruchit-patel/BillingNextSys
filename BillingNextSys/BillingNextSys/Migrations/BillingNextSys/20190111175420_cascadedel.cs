using Microsoft.EntityFrameworkCore.Migrations;

namespace BillingNextSys.Migrations.BillingNextSys
{
    public partial class cascadedel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillDetails_Bill_BillNumber",
                table: "BillDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_BillDetails_Bill_BillNumber",
                table: "BillDetails",
                column: "BillNumber",
                principalTable: "Bill",
                principalColumn: "BillNumber",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillDetails_Bill_BillNumber",
                table: "BillDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_BillDetails_Bill_BillNumber",
                table: "BillDetails",
                column: "BillNumber",
                principalTable: "Bill",
                principalColumn: "BillNumber",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
