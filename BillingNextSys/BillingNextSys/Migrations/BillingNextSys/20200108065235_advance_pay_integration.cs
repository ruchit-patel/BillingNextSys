using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BillingNextSys.Migrations.BillingNextSys
{
    public partial class advance_pay_integration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Received_DebtorGroup_DebtorGroupID",
                table: "Received");

            migrationBuilder.AlterColumn<int>(
                name: "DebtorGroupID",
                table: "Received",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "AdvancePay",
                columns: table => new
                {
                    AdvancePayID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AdvanceAmount = table.Column<double>(nullable: false),
                    ChequePaymet = table.Column<bool>(nullable: false),
                    ChequeNumber = table.Column<string>(nullable: true),
                    ReceivedDate = table.Column<DateTime>(nullable: false),
                    DebtorGroupID = table.Column<int>(nullable: false),
                    CompanyID = table.Column<int>(nullable: false),
                    BranchID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvancePay", x => x.AdvancePayID);
                    table.ForeignKey(
                        name: "FK_AdvancePay_Branch_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branch",
                        principalColumn: "BranchID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdvancePay_Company_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Company",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdvancePay_DebtorGroup_DebtorGroupID",
                        column: x => x.DebtorGroupID,
                        principalTable: "DebtorGroup",
                        principalColumn: "DebtorGroupID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvancePay_BranchID",
                table: "AdvancePay",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_AdvancePay_CompanyID",
                table: "AdvancePay",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_AdvancePay_DebtorGroupID",
                table: "AdvancePay",
                column: "DebtorGroupID");

            migrationBuilder.AddForeignKey(
                name: "FK_Received_DebtorGroup_DebtorGroupID",
                table: "Received",
                column: "DebtorGroupID",
                principalTable: "DebtorGroup",
                principalColumn: "DebtorGroupID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Received_DebtorGroup_DebtorGroupID",
                table: "Received");

            migrationBuilder.DropTable(
                name: "AdvancePay");

            migrationBuilder.AlterColumn<int>(
                name: "DebtorGroupID",
                table: "Received",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Received_DebtorGroup_DebtorGroupID",
                table: "Received",
                column: "DebtorGroupID",
                principalTable: "DebtorGroup",
                principalColumn: "DebtorGroupID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
