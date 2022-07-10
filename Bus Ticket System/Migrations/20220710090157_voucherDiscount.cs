using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bus_Ticket_System.Migrations
{
    public partial class voucherDiscount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "discount_percent",
                table: "Vouchers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "discount_percent",
                table: "Vouchers");
        }
    }
}
