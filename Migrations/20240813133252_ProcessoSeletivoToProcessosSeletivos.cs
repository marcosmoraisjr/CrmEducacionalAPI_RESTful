using Microsoft.EntityFrameworkCore.Migrations;

namespace CrmEducacional.Migrations
{
    public partial class ProcessoSeletivoToProcessosSeletivos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
