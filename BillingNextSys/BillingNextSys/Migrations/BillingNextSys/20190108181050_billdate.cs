﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BillingNextSys.Migrations.BillingNextSys
{
    public partial class billdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BillDate",
                table: "Bill",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillDate",
                table: "Bill");
        }
    }
}