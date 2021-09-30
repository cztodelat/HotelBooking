using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelBooking.Migrations
{
    public partial class RoomStatus_SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RoomStatuses",
                columns: new[] { "RoomStatusId", "StatusName" },
                values: new object[] { 1, 0 });

            migrationBuilder.InsertData(
                table: "RoomStatuses",
                columns: new[] { "RoomStatusId", "StatusName" },
                values: new object[] { 2, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoomStatuses",
                keyColumn: "RoomStatusId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RoomStatuses",
                keyColumn: "RoomStatusId",
                keyValue: 2);
        }
    }
}
