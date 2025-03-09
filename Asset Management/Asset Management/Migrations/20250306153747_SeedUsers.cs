using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Asset_Management.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1", 0, "cd5698bc-1849-4eb4-b6e7-4db0ed21dfb1", "admin@example.com", true, "Administrator", false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEHJplVpZEBsFpPENuUsnZpT6j5psTnrvDSjdQ2d/minqo/VcwyCs2CE+ScCu17aSew==", null, false, "74745c31-86bd-4e4c-b6ca-f7be089f72f8", false, "admin" },
                    { "2", 0, "3be52a5c-14a9-4af3-b435-d841639f7847", "user1@example.com", true, "User One", false, null, "USER1@EXAMPLE.COM", "USER1", "AQAAAAIAAYagAAAAEKCHXCj74ejmUH/g2Ye0F/ado8MpLdFU/cJW7olBllhxS6ol46YbWdHDBhwdQGk8kA==", null, false, "faf42e05-588e-4601-b06b-4ff120115dcb", false, "user1" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2");
        }
    }
}
