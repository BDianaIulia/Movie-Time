using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class LanguageExtension : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Language_OriginalLanguageId",
                table: "Movie");

            migrationBuilder.DropIndex(
                name: "IX_Movie_OriginalLanguageId",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "OriginalLanguageId",
                table: "Movie");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OriginalLanguageId",
                table: "Movie",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movie_OriginalLanguageId",
                table: "Movie",
                column: "OriginalLanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_Language_OriginalLanguageId",
                table: "Movie",
                column: "OriginalLanguageId",
                principalTable: "Language",
                principalColumn: "Id");
        }
    }
}
