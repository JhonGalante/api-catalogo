using Microsoft.EntityFrameworkCore.Migrations;

namespace APICatalogo.Migrations
{
    public partial class Populadb : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert INTO Categorias(Nome, ImagemUrl) VALUES('Bebidas', 'http://macoratti.net/Imagens/1.jpg')");
            mb.Sql("Insert INTO Categorias(Nome, ImagemUrl) VALUES('Lanches', 'http://macoratti.net/Imagens/2.jpg')");
            mb.Sql("Insert INTO Categorias(Nome, ImagemUrl) VALUES('Sobremesas', 'http://macoratti.net/Imagens/3.jpg')");

            mb.Sql("Insert INTO Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) VALUES('Coca-Cola Diet','Refrigerante de Cola 350 ml', '5.45','http://macoratti.net/Imagens/coca.jpg', 50, now(), (Select CategoriaId from Categorias where Nome = 'Bebidas'))");
            mb.Sql("Insert INTO Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) VALUES('Lanche de Atum','Lanche de Atum com Maionese', '8.50','http://macoratti.net/Imagens/atum.jpg', 10, now(), (Select CategoriaId from Categorias where Nome = 'Lanches'))");
            mb.Sql("Insert INTO Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) VALUES('Pudim 100 g','Pudim de leite condensado 100g', '6.75','http://macoratti.net/Imagens/pudim.jpg', 20, now(), (Select CategoriaId from Categorias where Nome = 'Sobremesas'))");

        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Categorias");
            mb.Sql("Delete from Produtos");
        }
    }
}
