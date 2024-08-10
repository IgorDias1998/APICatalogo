using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
                "Values('Coca-Cola','Refrigerante de cola 350ml',5.45,'cocacola.jpg',50,now(),1)");

            mb.Sql("insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
                "Values('Lanche de frango','Lance de frango com requijão',8.45,'fgo.jpg',20,now(),2)");

            mb.Sql("insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
                "Values('Pudim 100 g','Doce pudim com leite condensado',6.45,'pudim.jpg',10,now(),1)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Produtos");
        }
    }
}
