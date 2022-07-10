using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bus_Ticket_System.Migrations
{
    public partial class buscsAddBusSeatIDAddBusTotalSeat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ava_seat",
                table: "Buses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "busSeatId",
                table: "Buses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ava_seat",
                table: "Buses");

            migrationBuilder.DropColumn(
                name: "busSeatId",
                table: "Buses");
        }
    }
}
