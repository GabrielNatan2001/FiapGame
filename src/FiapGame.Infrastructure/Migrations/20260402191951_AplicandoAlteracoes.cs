using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiapGame.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AplicandoAlteracoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "titulo",
                table: "jogos",
                newName: "nome");

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "usuarios",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "categoria",
                table: "jogos",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "jogos",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "categoria",
                table: "jogos");

            migrationBuilder.DropColumn(
                name: "status",
                table: "jogos");

            migrationBuilder.RenameColumn(
                name: "nome",
                table: "jogos",
                newName: "titulo");
        }
    }
}
