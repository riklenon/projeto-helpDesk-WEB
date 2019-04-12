using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetoPim.Migrations
{
    public partial class Prioridade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Prioridade",
                table: "Chamado",
                nullable: false,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Prioridade",
                table: "Chamado",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
