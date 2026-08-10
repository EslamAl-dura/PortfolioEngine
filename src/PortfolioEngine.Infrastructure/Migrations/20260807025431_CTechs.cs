using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioEngine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CTechs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Technologies_TeckCatagories_TeckCatagoryId",
                table: "Technologies");

            migrationBuilder.DropTable(
                name: "TeckCatagories");

            migrationBuilder.RenameColumn(
                name: "TeckCatagoryId",
                table: "Technologies",
                newName: "TechCatagoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Technologies_TeckCatagoryId",
                table: "Technologies",
                newName: "IX_Technologies_TechCatagoryId");

            migrationBuilder.CreateTable(
                name: "TechCatagories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IconClass = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechCatagories", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Technologies_TechCatagories_TechCatagoryId",
                table: "Technologies",
                column: "TechCatagoryId",
                principalTable: "TechCatagories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Technologies_TechCatagories_TechCatagoryId",
                table: "Technologies");

            migrationBuilder.DropTable(
                name: "TechCatagories");

            migrationBuilder.RenameColumn(
                name: "TechCatagoryId",
                table: "Technologies",
                newName: "TeckCatagoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Technologies_TechCatagoryId",
                table: "Technologies",
                newName: "IX_Technologies_TeckCatagoryId");

            migrationBuilder.CreateTable(
                name: "TeckCatagories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IconClass = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeckCatagories", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Technologies_TeckCatagories_TeckCatagoryId",
                table: "Technologies",
                column: "TeckCatagoryId",
                principalTable: "TeckCatagories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
