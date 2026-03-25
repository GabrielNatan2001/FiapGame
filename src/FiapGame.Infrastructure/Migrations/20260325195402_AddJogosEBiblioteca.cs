using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiapGame.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddJogosEBiblioteca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "jogos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    titulo = table.Column<string>(type: "varchar(120)", nullable: false),
                    descricao = table.Column<string>(type: "varchar(1000)", nullable: false),
                    preco = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    dt_cadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dt_atualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jogos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "usuario_jogos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    jogo_id = table.Column<Guid>(type: "uuid", nullable: false),
                    dt_cadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dt_atualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario_jogos", x => x.id);
                    table.ForeignKey(
                        name: "FK_usuario_jogos_jogos_jogo_id",
                        column: x => x.jogo_id,
                        principalTable: "jogos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_usuario_jogos_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_usuario_jogos_jogo_id",
                table: "usuario_jogos",
                column: "jogo_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_jogos_usuario_id_jogo_id",
                table: "usuario_jogos",
                columns: new[] { "usuario_id", "jogo_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "usuario_jogos");

            migrationBuilder.DropTable(
                name: "jogos");
        }
    }
}
