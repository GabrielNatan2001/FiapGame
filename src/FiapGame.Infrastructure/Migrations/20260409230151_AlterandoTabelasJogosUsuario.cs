using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiapGame.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterandoTabelasJogosUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "usuario_jogos");

            migrationBuilder.CreateTable(
                name: "bibliotecas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    dt_cadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dt_atualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bibliotecas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "itens_biblioteca",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    jogo_id = table.Column<Guid>(type: "uuid", nullable: false),
                    dt_aquisicao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BibliotecaId = table.Column<Guid>(type: "uuid", nullable: false),
                    dt_cadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dt_atualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_itens_biblioteca", x => x.Id);
                    table.ForeignKey(
                        name: "FK_itens_biblioteca_bibliotecas_BibliotecaId",
                        column: x => x.BibliotecaId,
                        principalTable: "bibliotecas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_itens_biblioteca_jogos_jogo_id",
                        column: x => x.jogo_id,
                        principalTable: "jogos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bibliotecas_usuario_id",
                table: "bibliotecas",
                column: "usuario_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_itens_biblioteca_BibliotecaId",
                table: "itens_biblioteca",
                column: "BibliotecaId");

            migrationBuilder.CreateIndex(
                name: "IX_itens_biblioteca_jogo_id",
                table: "itens_biblioteca",
                column: "jogo_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "itens_biblioteca");

            migrationBuilder.DropTable(
                name: "bibliotecas");

            migrationBuilder.CreateTable(
                name: "usuario_jogos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    jogo_id = table.Column<Guid>(type: "uuid", nullable: false),
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    dt_atualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    dt_cadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
    }
}
