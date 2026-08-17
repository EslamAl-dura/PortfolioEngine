using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioEngine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class techSkill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
