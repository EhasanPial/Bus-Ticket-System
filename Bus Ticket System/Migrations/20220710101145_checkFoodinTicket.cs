using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bus_Ticket_System.Migrations
{
    public partial class checkFoodinTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "checkFood",
                table: "Tickets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "checkFood",
                table: "Tickets");
        }
    }
}
