using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class SeedGenres : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Genres (Name) VALUES ('Weapon')");
            migrationBuilder.Sql("INSERT INTO Genres (Name) VALUES ('Car')");
            migrationBuilder.Sql("INSERT INTO Genres (Name) VALUES ('Book')");
            migrationBuilder.Sql("INSERT INTO Genres (Name) VALUES ('Other')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP Genres");
        }
    }
}
