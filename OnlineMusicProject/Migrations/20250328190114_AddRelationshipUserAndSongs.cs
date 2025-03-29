using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMusicProject.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationshipUserAndSongs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Songs",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Songs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Songs_UserId",
                table: "Songs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_AspNetUsers_UserId",
                table: "Songs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Songs_AspNetUsers_UserId",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Songs_UserId",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Songs");
        }
    }
}
