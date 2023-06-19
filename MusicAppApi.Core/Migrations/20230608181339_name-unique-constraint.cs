using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicAppApi.Core.Migrations
{
    /// <inheritdoc />
    public partial class nameuniqueconstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Sets",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_Name_UserId",
                table: "Sets",
                columns: new[] { "Name", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sets_Name_UserId",
                table: "Sets");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Sets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
