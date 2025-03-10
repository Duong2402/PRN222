using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMusicProject.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Songs_SongGenres_songGenresGenreId",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Songs_songGenresGenreId",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "songGenresGenreId",
                table: "Songs");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_GenreId",
                table: "Songs",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_SongGenres_GenreId",
                table: "Songs",
                column: "GenreId",
                principalTable: "SongGenres",
                principalColumn: "GenreId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Songs_SongGenres_GenreId",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Songs_GenreId",
                table: "Songs");

            migrationBuilder.AddColumn<Guid>(
                name: "songGenresGenreId",
                table: "Songs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Songs_songGenresGenreId",
                table: "Songs",
                column: "songGenresGenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_SongGenres_songGenresGenreId",
                table: "Songs",
                column: "songGenresGenreId",
                principalTable: "SongGenres",
                principalColumn: "GenreId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
