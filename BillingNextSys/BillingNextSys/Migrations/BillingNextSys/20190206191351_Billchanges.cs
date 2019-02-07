using Microsoft.EntityFrameworkCore.Migrations;

namespace BillingNextSys.Migrations.BillingNextSys
{
    public partial class Billchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "BillDelivered",
                table: "Bill",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "BranchID",
                table: "Bill",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecretUnlockCode",
                table: "Bill",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bill_BranchID",
                table: "Bill",
                column: "BranchID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bill_Branch_BranchID",
                table: "Bill",
                column: "BranchID",
                principalTable: "Branch",
                principalColumn: "BranchID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bill_Branch_BranchID",
                table: "Bill");

            migrationBuilder.DropIndex(
                name: "IX_Bill_BranchID",
                table: "Bill");

            migrationBuilder.DropColumn(
                name: "BillDelivered",
                table: "Bill");

            migrationBuilder.DropColumn(
                name: "BranchID",
                table: "Bill");

            migrationBuilder.DropColumn(
                name: "SecretUnlockCode",
                table: "Bill");
        }
    }
}
