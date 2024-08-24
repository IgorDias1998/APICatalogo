using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutos(ProdutosParameters parameters);
        IEnumerable<Produto> GetProdutosPorCategoria(int idCategoria);
    }
}
