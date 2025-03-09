using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Asset_Management.Migrations
{
    /// <inheritdoc />
    public partial class seedUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", null, "Admin", "ADMIN" },
                    { "2", null, "User", "USER" }
                });

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
                columns: new[] { "ConcurrencyStamp", "Email", "FullName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "a2cc2fac-fe96-4064-bc2b-6b956dd9d20c", "user@example.com", "Regular User", "USER@EXAMPLE.COM", "USER", "AQAAAAIAAYagAAAAEHtobEUwXMlA0Xpx6V3ZsQB5dv+mr5hg5keGdGpjzctCPoTOnyL0syf/dPwf/B/tyw==", "0834f404-16fb-4d57-a0b7-91aa113e1aa7", "user" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "1", "1" },
                    { "2", "2" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1", "1" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2", "2" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2fa0a1c0-e35f-422e-8817-ccf002a0f77d", "AQAAAAIAAYagAAAAEJcIcFJGlrefquQ8BrpZb3NZR9hdlPsojb7epEiw92qX1X/nLzJepwBKHZ1dJ2Fb3w==", "0ac7abe7-e247-4508-9fae-45ded48e3065" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "Email", "FullName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "427f88c2-8d36-4595-9051-8258a903ed97", "user1@example.com", "User One", "USER1@EXAMPLE.COM", "USER1", "AQAAAAIAAYagAAAAEBHaJw0fN7gRDmIHL76wd0B6q3e8yN5iDfBdiaRn7C5XRvsqWJpNJyAZxfBr49E2zQ==", "2d142684-19a6-4db1-bd12-232568677692", "user1" });
        }
    }
}
