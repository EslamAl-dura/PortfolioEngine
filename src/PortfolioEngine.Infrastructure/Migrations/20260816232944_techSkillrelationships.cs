using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioEngine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class techSkillrelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Technologies_Skills_SkillId",
                table: "Technologies");

            migrationBuilder.DropIndex(
                name: "IX_Technologies_SkillId",
                table: "Technologies");

            migrationBuilder.DropColumn(
                name: "SkillId",
                table: "Technologies");

            migrationBuilder.CreateTable(
                name: "SkillTechnologies",
                columns: table => new
                {
                    SkillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TechnologiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillTechnologies", x => new { x.SkillId, x.TechnologiesId });
                    table.ForeignKey(
                        name: "FK_SkillTechnologies_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SkillTechnologies_Technologies_TechnologiesId",
                        column: x => x.TechnologiesId,
                        principalTable: "Technologies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SkillTechnologies_TechnologiesId",
                table: "SkillTechnologies",
                column: "TechnologiesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SkillTechnologies");

            migrationBuilder.AddColumn<Guid>(
                name: "SkillId",
                table: "Technologies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Technologies_SkillId",
                table: "Technologies",
                column: "SkillId");

            migrationBuilder.AddForeignKey(
                name: "FK_Technologies_Skills_SkillId",
                table: "Technologies",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id");
        }
    }
}
