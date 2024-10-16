using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        //IEnumerable<Produto> GetProdutos(ProdutosParameters parameters);
        PagedList<Produto> GetProdutos(ProdutosParameters parameters);
        PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFiltroPreco);
        IEnumerable<Produto> GetProdutosPorCategoria(int idCategoria);
    }
}
