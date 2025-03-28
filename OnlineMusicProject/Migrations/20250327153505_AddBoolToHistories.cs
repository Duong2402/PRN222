using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMusicProject.Migrations
{
    /// <inheritdoc />
    public partial class AddBoolToHistories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Playlists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAlbum",
                table: "Histories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Albums",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Albums_UserId",
                table: "Albums",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_AspNetUsers_UserId",
                table: "Albums",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_AspNetUsers_UserId",
                table: "Albums");

            migrationBuilder.DropIndex(
                name: "IX_Albums_UserId",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "IsAlbum",
                table: "Histories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Albums");
        }
    }
}
