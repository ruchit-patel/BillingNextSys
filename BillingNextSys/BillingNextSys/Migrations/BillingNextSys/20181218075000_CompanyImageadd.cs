using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BillingNextSys.Migrations.BillingNextSys
{
    public partial class CompanyImageadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyLogo",
                table: "Company");

            migrationBuilder.AddColumn<byte[]>(
                name: "CompanyLogoImg",
                table: "Company",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyLogoImg",
                table: "Company");

            migrationBuilder.AddColumn<string>(
                name: "CompanyLogo",
                table: "Company",
                nullable: false,
                defaultValue: "");
        }
    }
}
