using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BillingNextSys.Migrations.BillingNextSys
{
    public partial class AddedLock_AP_Bill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "BillLocked",
                table: "Bill",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AdvancePayDeductLock",
                table: "AdvancePayDeduct",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualDateTimeReceived",
                table: "AdvancePay",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "AdvancePayLock",
                table: "AdvancePay",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillLocked",
                table: "Bill");

            migrationBuilder.DropColumn(
                name: "AdvancePayDeductLock",
                table: "AdvancePayDeduct");

            migrationBuilder.DropColumn(
                name: "ActualDateTimeReceived",
                table: "AdvancePay");

            migrationBuilder.DropColumn(
                name: "AdvancePayLock",
                table: "AdvancePay");
        }
    }
}
