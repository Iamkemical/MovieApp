using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieApp.API.Migrations
{
    public partial class AddedAudiencePropToMovieModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Audience",
                table: "Movies",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Audience",
                table: "Movies");
        }
    }
}
