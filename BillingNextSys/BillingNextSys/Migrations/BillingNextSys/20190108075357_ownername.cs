using Microsoft.EntityFrameworkCore.Migrations;

namespace BillingNextSys.Migrations.BillingNextSys
{
    public partial class ownername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyOwner",
                table: "Company",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyOwner",
                table: "Company");
        }
    }
}
