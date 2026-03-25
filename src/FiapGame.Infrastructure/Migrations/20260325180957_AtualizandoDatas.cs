using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiapGame.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AtualizandoDatas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DtCadastro",
                table: "usuarios",
                newName: "dt_cadastro");

            migrationBuilder.RenameColumn(
                name: "DtAtualizacao",
                table: "usuarios",
                newName: "dt_atualizacao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "dt_cadastro",
                table: "usuarios",
                newName: "DtCadastro");

            migrationBuilder.RenameColumn(
                name: "dt_atualizacao",
                table: "usuarios",
                newName: "DtAtualizacao");
        }
    }
}
