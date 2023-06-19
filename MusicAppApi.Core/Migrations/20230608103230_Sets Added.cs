using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicAppApi.Core.Migrations
{
    /// <inheritdoc />
    public partial class SetsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SetId",
                table: "Audios",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Sets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PosterUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Audios_SetId",
                table: "Audios",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_UserId",
                table: "Sets",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Audios_Sets_SetId",
                table: "Audios",
                column: "SetId",
                principalTable: "Sets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audios_Sets_SetId",
                table: "Audios");

            migrationBuilder.DropTable(
                name: "Sets");

            migrationBuilder.DropIndex(
                name: "IX_Audios_SetId",
                table: "Audios");

            migrationBuilder.DropColumn(
                name: "SetId",
                table: "Audios");
        }
    }
}
