using Microsoft.EntityFrameworkCore.Migrations;

namespace BillingNextSys.Migrations.BillingNextSys
{
    public partial class rmfk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillDetails_Debtor_DebtorID",
                table: "BillDetails");

            migrationBuilder.AlterColumn<int>(
                name: "DebtorID",
                table: "BillDetails",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_BillDetails_Debtor_DebtorID",
                table: "BillDetails",
                column: "DebtorID",
                principalTable: "Debtor",
                principalColumn: "DebtorID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillDetails_Debtor_DebtorID",
                table: "BillDetails");

            migrationBuilder.AlterColumn<int>(
                name: "DebtorID",
                table: "BillDetails",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BillDetails_Debtor_DebtorID",
                table: "BillDetails",
                column: "DebtorID",
                principalTable: "Debtor",
                principalColumn: "DebtorID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
