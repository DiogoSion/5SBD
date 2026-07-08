using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Viacao.Infrastructure.Context.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Motoristas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR(150)", nullable: false),
                    Cnh = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    HorasDirigidasNoTurno = table.Column<decimal>(type: "DECIMAL(5,2)", nullable: false),
                    KmRodadosNoTurno = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    UltimoFimDeTurno = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    EmTurno = table.Column<bool>(type: "BIT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motoristas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Onibus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Placa = table.Column<string>(type: "VARCHAR(8)", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Capacidade = table.Column<int>(type: "INT", nullable: false),
                    QuilometragemAtual = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    QuilometragemUltimaRevisao = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Onibus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rotas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    CidadeOrigem = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    CidadeDestino = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    DistanciaTotalKm = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rotas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR(150)", nullable: false),
                    Cpf = table.Column<string>(type: "VARCHAR(11)", nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(150)", nullable: false),
                    SenhaHash = table.Column<string>(type: "VARCHAR(MAX)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Viagens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RotaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OnibusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MotoristaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataPartida = table.Column<DateTime>(type: "DATETIME2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Viagens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Paradas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RotaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cidade = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    Ordem = table.Column<int>(type: "INT", nullable: false),
                    PermiteVenda = table.Column<bool>(type: "BIT", nullable: false),
                    PontoTrocaMotorista = table.Column<bool>(type: "BIT", nullable: false),
                    QuilometroTrecho = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paradas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paradas_Rotas_RotaId",
                        column: x => x.RotaId,
                        principalTable: "Rotas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Passagens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ViagemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PassageiroId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParadaOrigemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParadaDestinoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumeroAssento = table.Column<int>(type: "INT", nullable: false),
                    ValorBase = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    ValorFinal = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    DataCompra = table.Column<DateTime>(type: "DATETIME2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passagens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Passagens_Viagens_ViagemId",
                        column: x => x.ViagemId,
                        principalTable: "Viagens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pagamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PassagemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Metodo = table.Column<int>(type: "int", nullable: false),
                    Origem = table.Column<int>(type: "int", nullable: false),
                    ValorPago = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "DATETIME2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagamentos_Passagens_PassagemId",
                        column: x => x.PassagemId,
                        principalTable: "Passagens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_PassagemId",
                table: "Pagamentos",
                column: "PassagemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Paradas_RotaId",
                table: "Paradas",
                column: "RotaId");

            migrationBuilder.CreateIndex(
                name: "IX_Passagens_ViagemId",
                table: "Passagens",
                column: "ViagemId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Cpf",
                table: "Usuarios",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Motoristas");

            migrationBuilder.DropTable(
                name: "Onibus");

            migrationBuilder.DropTable(
                name: "Pagamentos");

            migrationBuilder.DropTable(
                name: "Paradas");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Passagens");

            migrationBuilder.DropTable(
                name: "Rotas");

            migrationBuilder.DropTable(
                name: "Viagens");
        }
    }
}
