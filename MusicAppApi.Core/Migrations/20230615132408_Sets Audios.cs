using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicAppApi.Core.Migrations
{
    /// <inheritdoc />
    public partial class SetsAudios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audios_Sets_SetId",
                table: "Audios");

            migrationBuilder.DropIndex(
                name: "IX_Audios_SetId",
                table: "Audios");

            migrationBuilder.DropColumn(
                name: "SetId",
                table: "Audios");

            migrationBuilder.CreateTable(
                name: "AudioSet",
                columns: table => new
                {
                    AudiosId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SetsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AudioSet", x => new { x.AudiosId, x.SetsId });
                    table.ForeignKey(
                        name: "FK_AudioSet_Audios_AudiosId",
                        column: x => x.AudiosId,
                        principalTable: "Audios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AudioSet_Sets_SetsId",
                        column: x => x.SetsId,
                        principalTable: "Sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AudioSet_SetsId",
                table: "AudioSet",
                column: "SetsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AudioSet");

            migrationBuilder.AddColumn<Guid>(
                name: "SetId",
                table: "Audios",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Audios_SetId",
                table: "Audios",
                column: "SetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Audios_Sets_SetId",
                table: "Audios",
                column: "SetId",
                principalTable: "Sets",
                principalColumn: "Id");
        }
    }
}
