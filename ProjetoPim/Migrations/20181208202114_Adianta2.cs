using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetoPim.Migrations
{
    public partial class Adianta2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Assunto",
                table: "Chamado",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Assunto",
                table: "Chamado",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
