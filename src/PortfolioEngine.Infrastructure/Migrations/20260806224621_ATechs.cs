using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioEngine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ATechs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "TeckCatagories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "TeckCatagories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "TeckCatagories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "TeckCatagories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TeckCatagories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "TeckCatagories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "TeckCatagories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "Technologies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Technologies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "Technologies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Technologies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Technologies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "Technologies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Technologies",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "TeckCatagories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TeckCatagories");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "TeckCatagories");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "TeckCatagories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TeckCatagories");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "TeckCatagories");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "TeckCatagories");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "Technologies");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Technologies");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "Technologies");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Technologies");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Technologies");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "Technologies");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Technologies");
        }
    }
}
