using Microsoft.EntityFrameworkCore.Migrations;

namespace CrmEducacional.Migrations
{
    public partial class RefatorandoProcessoSeletivo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessosSeletivos",
                table: "ProcessosSeletivos");

            migrationBuilder.RenameTable(
                name: "ProcessosSeletivos",
                newName: "ProcessoSeletivos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessoSeletivos",
                table: "ProcessoSeletivos",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Inscricoes_LeadId",
                table: "Inscricoes",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscricoes_OfertaId",
                table: "Inscricoes",
                column: "OfertaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inscricoes_Leads_LeadId",
                table: "Inscricoes",
                column: "LeadId",
                principalTable: "Leads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inscricoes_Ofertas_OfertaId",
                table: "Inscricoes",
                column: "OfertaId",
                principalTable: "Ofertas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inscricoes_Leads_LeadId",
                table: "Inscricoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Inscricoes_Ofertas_OfertaId",
                table: "Inscricoes");

            migrationBuilder.DropIndex(
                name: "IX_Inscricoes_LeadId",
                table: "Inscricoes");

            migrationBuilder.DropIndex(
                name: "IX_Inscricoes_OfertaId",
                table: "Inscricoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessoSeletivos",
                table: "ProcessoSeletivos");

            migrationBuilder.RenameTable(
                name: "ProcessoSeletivos",
                newName: "ProcessosSeletivos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessosSeletivos",
                table: "ProcessosSeletivos",
                column: "Id");
        }
    }
}
