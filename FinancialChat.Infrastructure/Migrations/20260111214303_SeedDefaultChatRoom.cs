using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialChat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultChatRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_ChatRooms_ChatRoomId1",
                table: "ChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessages_ChatRoomId1",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "ChatRoomId1",
                table: "ChatMessages");

            migrationBuilder.InsertData(
                table: "ChatRooms",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), "General" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ChatRooms",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.AddColumn<Guid>(
                name: "ChatRoomId1",
                table: "ChatMessages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ChatRoomId1",
                table: "ChatMessages",
                column: "ChatRoomId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_ChatRooms_ChatRoomId1",
                table: "ChatMessages",
                column: "ChatRoomId1",
                principalTable: "ChatRooms",
                principalColumn: "Id");
        }
    }
}
