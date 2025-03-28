using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asset_Management.Migrations
{
    /// <inheritdoc />
    public partial class InitChatMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReceiverId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0a18edd2-fffa-49ea-b1f4-7016ac950129", "AQAAAAIAAYagAAAAECeFuLbAwPYpn4YPl2WZOD/JCi3fhoJuiAavz2V6lMhTvsK36ZWE3uy1sTGt1h3MFg==", "316f4e65-fc03-4265-b1c1-d0c7f5173a49" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f5b6e75a-2f65-4353-8a72-8047a9ac605f", "AQAAAAIAAYagAAAAEIae8S3JjdHTvsgPCUa5M1ywBDyAvV8mgTkLJeQSEXdpqJnuc3t/pU/pOlD469YNOw==", "9d833f50-0024-4c65-bb31-2ca9ca7a0ba5" });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverId",
                table: "Messages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

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
    }
}
