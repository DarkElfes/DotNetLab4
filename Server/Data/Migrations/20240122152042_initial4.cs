﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class initial4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_BaseChats_ChatId",
                table: "ChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessages_ChatId",
                table: "ChatMessages");

            migrationBuilder.AlterColumn<int>(
                name: "ChatId",
                table: "ChatMessages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BaseChatId",
                table: "ChatMessages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_BaseChatId",
                table: "ChatMessages",
                column: "BaseChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_BaseChats_BaseChatId",
                table: "ChatMessages",
                column: "BaseChatId",
                principalTable: "BaseChats",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_BaseChats_BaseChatId",
                table: "ChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessages_BaseChatId",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "BaseChatId",
                table: "ChatMessages");

            migrationBuilder.AlterColumn<int>(
                name: "ChatId",
                table: "ChatMessages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ChatId",
                table: "ChatMessages",
                column: "ChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_BaseChats_ChatId",
                table: "ChatMessages",
                column: "ChatId",
                principalTable: "BaseChats",
                principalColumn: "Id");
        }
    }
}
