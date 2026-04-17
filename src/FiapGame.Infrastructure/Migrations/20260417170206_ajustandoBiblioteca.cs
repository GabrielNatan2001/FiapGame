using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiapGame.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ajustandoBiblioteca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_itens_biblioteca_BibliotecaId",
                table: "itens_biblioteca");

            migrationBuilder.CreateIndex(
                name: "IX_itens_biblioteca_BibliotecaId_jogo_id",
                table: "itens_biblioteca",
                columns: new[] { "BibliotecaId", "jogo_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_itens_biblioteca_BibliotecaId_jogo_id",
                table: "itens_biblioteca");

            migrationBuilder.CreateIndex(
                name: "IX_itens_biblioteca_BibliotecaId",
                table: "itens_biblioteca",
                column: "BibliotecaId");
        }
    }
}
