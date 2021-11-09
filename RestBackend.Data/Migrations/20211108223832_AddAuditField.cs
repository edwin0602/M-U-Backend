using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RestBackend.Data.Migrations
{
    public partial class AddAuditField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Entity",
                schema: "edm",
                table: "Audit",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Entity",
                schema: "edm",
                table: "Audit");
        }
    }
}
