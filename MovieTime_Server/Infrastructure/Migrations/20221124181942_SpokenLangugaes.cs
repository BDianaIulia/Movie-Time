using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class SpokenLangugaes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Language_Movie_MovieId",
                table: "Language");

            migrationBuilder.DropIndex(
                name: "IX_Language_MovieId",
                table: "Language");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "Language");

            migrationBuilder.CreateTable(
                name: "LanguageMovie",
                columns: table => new
                {
                    MoviesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpokenLanguagesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageMovie", x => new { x.MoviesId, x.SpokenLanguagesId });
                    table.ForeignKey(
                        name: "FK_LanguageMovie_Language_SpokenLanguagesId",
                        column: x => x.SpokenLanguagesId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LanguageMovie_Movie_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LanguageMovie_SpokenLanguagesId",
                table: "LanguageMovie",
                column: "SpokenLanguagesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LanguageMovie");

            migrationBuilder.AddColumn<Guid>(
                name: "MovieId",
                table: "Language",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Language_MovieId",
                table: "Language",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Language_Movie_MovieId",
                table: "Language",
                column: "MovieId",
                principalTable: "Movie",
                principalColumn: "Id");
        }
    }
}
