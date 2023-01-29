using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Typo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMovieActivity_Movie_MoviesId",
                table: "UserMovieActivity");

            migrationBuilder.RenameColumn(
                name: "MoviesId",
                table: "UserMovieActivity",
                newName: "MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_UserMovieActivity_MoviesId",
                table: "UserMovieActivity",
                newName: "IX_UserMovieActivity_MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMovieActivity_Movie_MovieId",
                table: "UserMovieActivity",
                column: "MovieId",
                principalTable: "Movie",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMovieActivity_Movie_MovieId",
                table: "UserMovieActivity");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "UserMovieActivity",
                newName: "MoviesId");

            migrationBuilder.RenameIndex(
                name: "IX_UserMovieActivity_MovieId",
                table: "UserMovieActivity",
                newName: "IX_UserMovieActivity_MoviesId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMovieActivity_Movie_MoviesId",
                table: "UserMovieActivity",
                column: "MoviesId",
                principalTable: "Movie",
                principalColumn: "Id");
        }
    }
}
