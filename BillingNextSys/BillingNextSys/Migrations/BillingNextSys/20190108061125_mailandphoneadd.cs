using Microsoft.EntityFrameworkCore.Migrations;

namespace BillingNextSys.Migrations.BillingNextSys
{
    public partial class mailandphoneadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BranchEmail",
                table: "Branch",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BranchPhone",
                table: "Branch",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchEmail",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "BranchPhone",
                table: "Branch");
        }
    }
}
