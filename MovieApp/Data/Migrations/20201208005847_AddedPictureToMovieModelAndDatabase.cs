using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieApp.API.Migrations
{
    public partial class AddedPictureToMovieModelAndDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Picture",
                table: "Movies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Movies");
        }
    }
}
