using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asset_Management.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDecimalPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d3c3caa6-1cb9-41eb-9809-7c78f765798d", "AQAAAAIAAYagAAAAEGmTCyibKYW6LGNzQ9DpUKYrNTEY2M1aHzSGhYWXeeQSJOJoA9Q5kbtB49mJIUAfrg==", "5bd28d33-1ba0-4e37-b91c-f22ec8797b51" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a884807d-d620-4588-9fa8-7842126c2620", "AQAAAAIAAYagAAAAEAfj7fUJWI71krK9CS5P/AU1sX41eNyapNJv+/979XFruxdrcpRq01iRgb6JRVJPcg==", "255369ce-3a49-4c14-ae28-2019b17ea2d9" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ef6ba552-3149-4e82-a335-770ca02d6d93", "AQAAAAIAAYagAAAAECsoXwVUbddQUQtyJjQQOCqLMfyiDLknpZDHM9QgH4+odapFV81rCCyq6oe87D/HxA==", "a7808894-5936-48d0-b2f5-cb420e1ce06e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a2cc2fac-fe96-4064-bc2b-6b956dd9d20c", "AQAAAAIAAYagAAAAEHtobEUwXMlA0Xpx6V3ZsQB5dv+mr5hg5keGdGpjzctCPoTOnyL0syf/dPwf/B/tyw==", "0834f404-16fb-4d57-a0b7-91aa113e1aa7" });
        }
    }
}
