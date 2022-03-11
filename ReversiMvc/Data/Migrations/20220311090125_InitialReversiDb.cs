using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReversiMvc.Data.Migrations;

[ExcludeFromCodeCoverage]
public partial class InitialReversiDb : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Players",
            columns: table => new
            {
                Guid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Victories = table.Column<int>(type: "int", nullable: false),
                Losses = table.Column<int>(type: "int", nullable: false),
                Draws = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Players", x => x.Guid);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Players");
    }
}
