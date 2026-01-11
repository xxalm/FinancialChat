using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialChat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "ChatMessages");

            migrationBuilder.AddColumn<Guid>(
                name: "ChatRoomId1",
                table: "ChatMessages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ChatMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ChatMessages");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "ChatMessages",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
