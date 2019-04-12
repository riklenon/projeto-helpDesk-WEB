using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetoPim.Migrations
{
    public partial class hahaha255 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Departamentos",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Departamento",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Departamento",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Departamentos",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }
    }
}
