using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CrudEmissoras.Migrations
{
    public partial class CriacaoBD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Emissoras",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emissoras", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Audiencias",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Pontos_audiencia = table.Column<int>(nullable: false),
                    Data_hora_audiencia = table.Column<DateTime>(nullable: false),
                    Emissora_audienciaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audiencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Audiencias_Emissoras_Emissora_audienciaId",
                        column: x => x.Emissora_audienciaId,
                        principalTable: "Emissoras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Audiencias_Emissora_audienciaId",
                table: "Audiencias",
                column: "Emissora_audienciaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Audiencias");

            migrationBuilder.DropTable(
                name: "Emissoras");
        }
    }
}
