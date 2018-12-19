using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BillingNextSys.Migrations.BillingNextSys
{
    public partial class ModelMigrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BillSeries",
                columns: table => new
                {
                    SeriesName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillSeries", x => x.SeriesName);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    CompanyID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CompanyName = table.Column<string>(maxLength: 100, nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    CompanyLogo = table.Column<string>(nullable: false),
                    BillFormat = table.Column<int>(nullable: true),
                    GSTIN = table.Column<string>(nullable: false),
                    BankName = table.Column<string>(maxLength: 100, nullable: false),
                    AccountType = table.Column<int>(nullable: false),
                    AccountNumber = table.Column<string>(nullable: false),
                    IFSCcode = table.Column<string>(nullable: false),
                    PAN = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.CompanyID);
                });

            migrationBuilder.CreateTable(
                name: "Particulars",
                columns: table => new
                {
                    ParticularsID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ParticularsName = table.Column<string>(maxLength: 150, nullable: false),
                    SACCode = table.Column<int>(nullable: false),
                    Amount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Particulars", x => x.ParticularsID);
                });

            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    BranchID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    BranchName = table.Column<string>(maxLength: 100, nullable: false),
                    BranchAddress = table.Column<string>(maxLength: 200, nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    CompanyID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.BranchID);
                    table.ForeignKey(
                        name: "FK_Branch_Company_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Company",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DebtorGroup",
                columns: table => new
                {
                    DebtorGroupID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DebtorGroupName = table.Column<string>(maxLength: 150, nullable: false),
                    DebtorGroupAddress = table.Column<string>(maxLength: 350, nullable: false),
                    DebtorGroupMail = table.Column<string>(nullable: false),
                    DebtorGroupPhoneNumber = table.Column<string>(nullable: false),
                    DebtorGroupCity = table.Column<string>(nullable: false),
                    DebtorGSTIN = table.Column<string>(nullable: true),
                    DebtorOutstanding = table.Column<double>(nullable: false),
                    BranchID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DebtorGroup", x => x.DebtorGroupID);
                    table.ForeignKey(
                        name: "FK_DebtorGroup_Branch_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branch",
                        principalColumn: "BranchID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bill",
                columns: table => new
                {
                    BillNumber = table.Column<string>(nullable: false),
                    BilledTo = table.Column<string>(nullable: false),
                    BillAmount = table.Column<double>(nullable: false),
                    Note = table.Column<string>(maxLength: 500, nullable: true),
                    InvoiceDate = table.Column<DateTime>(nullable: false),
                    PlaceOfSupply = table.Column<int>(nullable: true),
                    DebtorGroupID = table.Column<int>(nullable: false),
                    CompanyID = table.Column<int>(nullable: false),
                    SeriesName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bill", x => x.BillNumber);
                    table.ForeignKey(
                        name: "FK_Bill_Company_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Company",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bill_DebtorGroup_DebtorGroupID",
                        column: x => x.DebtorGroupID,
                        principalTable: "DebtorGroup",
                        principalColumn: "DebtorGroupID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bill_BillSeries_SeriesName",
                        column: x => x.SeriesName,
                        principalTable: "BillSeries",
                        principalColumn: "SeriesName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Debtor",
                columns: table => new
                {
                    DebtorID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DebtorName = table.Column<string>(maxLength: 150, nullable: false),
                    DebtorOutstanding = table.Column<double>(nullable: false),
                    DebtorGroupID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Debtor", x => x.DebtorID);
                    table.ForeignKey(
                        name: "FK_Debtor_DebtorGroup_DebtorGroupID",
                        column: x => x.DebtorGroupID,
                        principalTable: "DebtorGroup",
                        principalColumn: "DebtorGroupID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillDetails",
                columns: table => new
                {
                    BillDetailsID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ParticularsName = table.Column<string>(nullable: false),
                    Period = table.Column<string>(nullable: false),
                    YearInfo = table.Column<int>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    CGSTAmount = table.Column<double>(nullable: false),
                    SGSTAmount = table.Column<double>(nullable: false),
                    TaxableValue = table.Column<double>(nullable: false),
                    BillAmountOutstanding = table.Column<double>(nullable: false),
                    CompanyID = table.Column<int>(nullable: false),
                    ParticularsID = table.Column<int>(nullable: false),
                    BillNumber = table.Column<string>(nullable: true),
                    DebtorID = table.Column<int>(nullable: false),
                    DebtorGroupID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillDetails", x => x.BillDetailsID);
                    table.ForeignKey(
                        name: "FK_BillDetails_Bill_BillNumber",
                        column: x => x.BillNumber,
                        principalTable: "Bill",
                        principalColumn: "BillNumber",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BillDetails_Company_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Company",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillDetails_DebtorGroup_DebtorGroupID",
                        column: x => x.DebtorGroupID,
                        principalTable: "DebtorGroup",
                        principalColumn: "DebtorGroupID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillDetails_Debtor_DebtorID",
                        column: x => x.DebtorID,
                        principalTable: "Debtor",
                        principalColumn: "DebtorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillDetails_Particulars_ParticularsID",
                        column: x => x.ParticularsID,
                        principalTable: "Particulars",
                        principalColumn: "ParticularsID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Received",
                columns: table => new
                {
                    ReceivedID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ReceivedAmount = table.Column<double>(nullable: false),
                    ReceivedDate = table.Column<DateTime>(nullable: false),
                    ChequePaymet = table.Column<bool>(nullable: false),
                    ChequeNumber = table.Column<string>(nullable: true),
                    CompanyID = table.Column<int>(nullable: false),
                    BillDetailsID = table.Column<int>(nullable: false),
                    DebtorGroupID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Received", x => x.ReceivedID);
                    table.ForeignKey(
                        name: "FK_Received_BillDetails_BillDetailsID",
                        column: x => x.BillDetailsID,
                        principalTable: "BillDetails",
                        principalColumn: "BillDetailsID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Received_Company_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Company",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Received_DebtorGroup_DebtorGroupID",
                        column: x => x.DebtorGroupID,
                        principalTable: "DebtorGroup",
                        principalColumn: "DebtorGroupID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bill_CompanyID",
                table: "Bill",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Bill_DebtorGroupID",
                table: "Bill",
                column: "DebtorGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_Bill_SeriesName",
                table: "Bill",
                column: "SeriesName");

            migrationBuilder.CreateIndex(
                name: "IX_BillDetails_BillNumber",
                table: "BillDetails",
                column: "BillNumber");

            migrationBuilder.CreateIndex(
                name: "IX_BillDetails_CompanyID",
                table: "BillDetails",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_BillDetails_DebtorGroupID",
                table: "BillDetails",
                column: "DebtorGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_BillDetails_DebtorID",
                table: "BillDetails",
                column: "DebtorID");

            migrationBuilder.CreateIndex(
                name: "IX_BillDetails_ParticularsID",
                table: "BillDetails",
                column: "ParticularsID");

            migrationBuilder.CreateIndex(
                name: "IX_Branch_CompanyID",
                table: "Branch",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Debtor_DebtorGroupID",
                table: "Debtor",
                column: "DebtorGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_DebtorGroup_BranchID",
                table: "DebtorGroup",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_Received_BillDetailsID",
                table: "Received",
                column: "BillDetailsID");

            migrationBuilder.CreateIndex(
                name: "IX_Received_CompanyID",
                table: "Received",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Received_DebtorGroupID",
                table: "Received",
                column: "DebtorGroupID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Received");

            migrationBuilder.DropTable(
                name: "BillDetails");

            migrationBuilder.DropTable(
                name: "Bill");

            migrationBuilder.DropTable(
                name: "Debtor");

            migrationBuilder.DropTable(
                name: "Particulars");

            migrationBuilder.DropTable(
                name: "BillSeries");

            migrationBuilder.DropTable(
                name: "DebtorGroup");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropTable(
                name: "Company");
        }
    }
}
