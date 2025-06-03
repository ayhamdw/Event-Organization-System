using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Event_Organization_System.Migrations
{
    /// <inheritdoc />
    public partial class RenameEventColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalSeat",
                table: "Events",
                newName: "TotalSeats");

            migrationBuilder.RenameColumn(
                name: "RemainingSeat",
                table: "Events",
                newName: "RemainingSeats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalSeats",
                table: "Events",
                newName: "TotalSeat");

            migrationBuilder.RenameColumn(
                name: "RemainingSeats",
                table: "Events",
                newName: "RemainingSeat");
        }
    }
}
