using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMusicProject.Migrations
{
    /// <inheritdoc />
    public partial class FixHistories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAlbum",
                table: "Histories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAlbum",
                table: "Histories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
