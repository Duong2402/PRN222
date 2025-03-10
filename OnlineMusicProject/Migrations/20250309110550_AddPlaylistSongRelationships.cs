using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMusicProject.Migrations
{
    /// <inheritdoc />
    public partial class AddPlaylistSongRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistSongs_Playlists_PlaylistsPlaylistId",
                table: "PlaylistSongs");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistSongs_Songs_SongsSongId",
                table: "PlaylistSongs");

            migrationBuilder.DropIndex(
                name: "IX_PlaylistSongs_PlaylistsPlaylistId",
                table: "PlaylistSongs");

            migrationBuilder.DropIndex(
                name: "IX_PlaylistSongs_SongsSongId",
                table: "PlaylistSongs");

            migrationBuilder.DropColumn(
                name: "PlaylistsPlaylistId",
                table: "PlaylistSongs");

            migrationBuilder.DropColumn(
                name: "SongsSongId",
                table: "PlaylistSongs");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistSongs_SongId",
                table: "PlaylistSongs",
                column: "SongId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistSongs_Playlists_PlaylistId",
                table: "PlaylistSongs",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "PlaylistId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistSongs_Songs_SongId",
                table: "PlaylistSongs",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "SongId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistSongs_Playlists_PlaylistId",
                table: "PlaylistSongs");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistSongs_Songs_SongId",
                table: "PlaylistSongs");

            migrationBuilder.DropIndex(
                name: "IX_PlaylistSongs_SongId",
                table: "PlaylistSongs");

            migrationBuilder.AddColumn<Guid>(
                name: "PlaylistsPlaylistId",
                table: "PlaylistSongs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SongsSongId",
                table: "PlaylistSongs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistSongs_PlaylistsPlaylistId",
                table: "PlaylistSongs",
                column: "PlaylistsPlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistSongs_SongsSongId",
                table: "PlaylistSongs",
                column: "SongsSongId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistSongs_Playlists_PlaylistsPlaylistId",
                table: "PlaylistSongs",
                column: "PlaylistsPlaylistId",
                principalTable: "Playlists",
                principalColumn: "PlaylistId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistSongs_Songs_SongsSongId",
                table: "PlaylistSongs",
                column: "SongsSongId",
                principalTable: "Songs",
                principalColumn: "SongId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
