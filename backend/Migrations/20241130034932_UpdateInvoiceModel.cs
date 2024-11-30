using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    public partial class UpdateInvoiceModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Alterar a tabela Invoices
            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Invoices",
                newName: "Total");

            migrationBuilder.AddColumn<string>(
                name: "Chave",
                table: "Invoices",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ConsumidorId",
                table: "Invoices",
                type: "uuid",
                nullable: true); // Permitir nulo inicialmente

            migrationBuilder.AddColumn<DateTime>(
                name: "DataEmissao",
                table: "Invoices",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: DateTime.UtcNow);

            migrationBuilder.AddColumn<Guid>(
                name: "EmitenteId",
                table: "Invoices",
                type: "uuid",
                nullable: true); // Permitir nulo inicialmente

            migrationBuilder.AddColumn<bool>(
                name: "NotaValida",
                table: "Invoices",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Numero",
                table: "Invoices",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Serie",
                table: "Invoices",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Invoices",
                type: "text",
                nullable: false,
                defaultValue: "");

            // Criar tabela Consumidores
            migrationBuilder.CreateTable(
                name: "Consumidores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Cpf_Cnpj = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consumidores", x => x.Id);
                });

            // Criar tabela Emitentes
            migrationBuilder.CreateTable(
                name: "Emitentes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Cnpj = table.Column<string>(type: "text", nullable: false),
                    RazaoSocial = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emitentes", x => x.Id);
                });

            // Inserir Emitente padrão apenas se a tabela Emitentes já existir
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF EXISTS (SELECT 1 FROM pg_tables WHERE schemaname = 'public' AND tablename = 'emitentes') THEN
                        INSERT INTO Emitentes (Id, Cnpj, RazaoSocial)
                        VALUES ('00000000-0000-0000-0000-000000000000', '00.000.000/0000-00', 'Emitente Padrão');
                    END IF;
                END
                $$;
            ");

            // Criar tabela Products
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Item = table.Column<string>(type: "text", nullable: false),
                    Quantidade = table.Column<decimal>(type: "numeric", nullable: false),
                    Unidade = table.Column<string>(type: "text", nullable: false),
                    Subtotal = table.Column<decimal>(type: "numeric", nullable: false),
                    Total = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Adicionar índices
            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ConsumidorId",
                table: "Invoices",
                column: "ConsumidorId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_EmitenteId",
                table: "Invoices",
                column: "EmitenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_InvoiceId",
                table: "Products",
                column: "InvoiceId");

            // Adicionar relacionamentos
            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Consumidores_ConsumidorId",
                table: "Invoices",
                column: "ConsumidorId",
                principalTable: "Consumidores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Emitentes_EmitenteId",
                table: "Invoices",
                column: "EmitenteId",
                principalTable: "Emitentes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Consumidores_ConsumidorId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Emitentes_EmitenteId",
                table: "Invoices");

            migrationBuilder.DropTable(
                name: "Consumidores");

            migrationBuilder.DropTable(
                name: "Emitentes");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_ConsumidorId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_EmitenteId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Chave",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "ConsumidorId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "DataEmissao",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "EmitenteId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "NotaValida",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Serie",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "Invoices",
                newName: "Price");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "Invoices",
                type: "text",
                nullable: true);
        }
    }
}
