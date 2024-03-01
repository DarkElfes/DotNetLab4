using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class initial3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseChatApplicationUser_AspNetUsers_UsersId",
                table: "BaseChatApplicationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseChatApplicationUser_BaseChats_ChatsId",
                table: "BaseChatApplicationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BaseChatApplicationUser",
                table: "BaseChatApplicationUser");

            migrationBuilder.RenameTable(
                name: "BaseChatApplicationUser",
                newName: "ApplicationUserBaseChat");

            migrationBuilder.RenameIndex(
                name: "IX_BaseChatApplicationUser_UsersId",
                table: "ApplicationUserBaseChat",
                newName: "IX_ApplicationUserBaseChat_UsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserBaseChat",
                table: "ApplicationUserBaseChat",
                columns: new[] { "ChatsId", "UsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserBaseChat_AspNetUsers_UsersId",
                table: "ApplicationUserBaseChat",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserBaseChat_BaseChats_ChatsId",
                table: "ApplicationUserBaseChat",
                column: "ChatsId",
                principalTable: "BaseChats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserBaseChat_AspNetUsers_UsersId",
                table: "ApplicationUserBaseChat");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserBaseChat_BaseChats_ChatsId",
                table: "ApplicationUserBaseChat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserBaseChat",
                table: "ApplicationUserBaseChat");

            migrationBuilder.RenameTable(
                name: "ApplicationUserBaseChat",
                newName: "BaseChatApplicationUser");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserBaseChat_UsersId",
                table: "BaseChatApplicationUser",
                newName: "IX_BaseChatApplicationUser_UsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BaseChatApplicationUser",
                table: "BaseChatApplicationUser",
                columns: new[] { "ChatsId", "UsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BaseChatApplicationUser_AspNetUsers_UsersId",
                table: "BaseChatApplicationUser",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseChatApplicationUser_BaseChats_ChatsId",
                table: "BaseChatApplicationUser",
                column: "ChatsId",
                principalTable: "BaseChats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
