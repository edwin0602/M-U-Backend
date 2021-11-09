using Microsoft.EntityFrameworkCore.Migrations;

namespace RestBackend.Data.Migrations
{
    public partial class AddTaxTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tax",
                schema: "edm",
                columns: table => new
                {
                    IdTax = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tax", x => x.IdTax);
                });

            migrationBuilder.InsertData(
                schema: "edm",
                table: "Tax",
                columns: new[] { "IdTax", "Enabled", "Value" },
                values: new object[] { 1, true, 0.19m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tax",
                schema: "edm");
        }
    }
}
