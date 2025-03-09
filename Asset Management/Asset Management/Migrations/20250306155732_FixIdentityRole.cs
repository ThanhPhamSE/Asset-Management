using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asset_Management.Migrations
{
    /// <inheritdoc />
    public partial class FixIdentityRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "427f88c2-8d36-4595-9051-8258a903ed97", "AQAAAAIAAYagAAAAEBHaJw0fN7gRDmIHL76wd0B6q3e8yN5iDfBdiaRn7C5XRvsqWJpNJyAZxfBr49E2zQ==", "2d142684-19a6-4db1-bd12-232568677692" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cd5698bc-1849-4eb4-b6e7-4db0ed21dfb1", "AQAAAAIAAYagAAAAEHJplVpZEBsFpPENuUsnZpT6j5psTnrvDSjdQ2d/minqo/VcwyCs2CE+ScCu17aSew==", "74745c31-86bd-4e4c-b6ca-f7be089f72f8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3be52a5c-14a9-4af3-b435-d841639f7847", "AQAAAAIAAYagAAAAEKCHXCj74ejmUH/g2Ye0F/ado8MpLdFU/cJW7olBllhxS6ol46YbWdHDBhwdQGk8kA==", "faf42e05-588e-4601-b06b-4ff120115dcb" });
        }
    }
}
