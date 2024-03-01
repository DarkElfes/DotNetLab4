using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class initial6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_BaseChats_BaseChatId",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "ChatMessages");

            migrationBuilder.AlterColumn<int>(
                name: "BaseChatId",
                table: "ChatMessages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_BaseChats_BaseChatId",
                table: "ChatMessages",
                column: "BaseChatId",
                principalTable: "BaseChats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_BaseChats_BaseChatId",
                table: "ChatMessages");

            migrationBuilder.AlterColumn<int>(
                name: "BaseChatId",
                table: "ChatMessages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ChatId",
                table: "ChatMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_BaseChats_BaseChatId",
                table: "ChatMessages",
                column: "BaseChatId",
                principalTable: "BaseChats",
                principalColumn: "Id");
        }
    }
}
