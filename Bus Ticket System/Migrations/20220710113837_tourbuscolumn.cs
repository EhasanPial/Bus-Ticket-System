using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bus_Ticket_System.Migrations
{
    public partial class tourbuscolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TourBus",
                table: "Buses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TourBus",
                table: "Buses");
        }
    }
}
