using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BillingNextSys.Migrations.BillingNextSys
{
    public partial class BranchChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CompanyOwner",
                table: "Company",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "BranchManagerName",
                table: "Branch",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "BranchManagerSign",
                table: "Branch",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchManagerName",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "BranchManagerSign",
                table: "Branch");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyOwner",
                table: "Company",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
