using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieApp.API.Migrations
{
    public partial class AddMoviesModelToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Rating = table.Column<int>(nullable: false),
                    GenreId = table.Column<Guid>(nullable: false),
                    SubGenreId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movies_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Movies_SubGenres_SubGenreId",
                        column: x => x.SubGenreId,
                        principalTable: "SubGenres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movies_GenreId",
                table: "Movies",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_SubGenreId",
                table: "Movies",
                column: "SubGenreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movies");
        }
    }
}
