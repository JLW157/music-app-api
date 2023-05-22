using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicAppApi.Core.Migrations
{
    /// <inheritdoc />
    public partial class PlayedCountfieldadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlayedCount",
                table: "Audios",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayedCount",
                table: "Audios");
        }
    }
}
