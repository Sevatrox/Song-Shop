using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SongShop.DataAccess.Migrations.AuditDb
{
    /// <inheritdoc />
    public partial class AddingCurrentDatetime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CurrentDatetime",
                table: "AuditModels",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentDatetime",
                table: "AuditModels");
        }
    }
}
