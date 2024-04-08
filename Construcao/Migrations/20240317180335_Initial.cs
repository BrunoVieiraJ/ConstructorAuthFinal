using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Construcao.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    CategoriasId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cat_Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cat_Desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prioridade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Responsavel = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.CategoriasId);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    ProdutoId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pro_Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pro_Desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Preco = table.Column<int>(type: "int", nullable: false),
                    QntEstoque = table.Column<int>(type: "int", nullable: false),
                    FK_CategoriaId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.ProdutoId);
                    table.ForeignKey(
                        name: "FK_Produtos_Categorias_FK_CategoriaId",
                        column: x => x.FK_CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "CategoriasId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_FK_CategoriaId",
                table: "Produtos",
                column: "FK_CategoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Categorias");
        }
    }
}
