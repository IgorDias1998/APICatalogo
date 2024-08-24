using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace APICatalogo.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context) { }

        public IEnumerable<Produto> GetProdutos(ProdutosParameters parametersProduto)
        {
            return GetAll()
                .OrderBy(p => p.Nome)
                .Skip((parametersProduto.PageNumber - 1) * parametersProduto.PageSize)
                .Take(parametersProduto.PageSize).ToList();
        }

        public IEnumerable<Produto> GetProdutosPorCategoria(int idCategoria)
        {
            return GetAll().Where(c => c.CategoriaId == idCategoria);
        }
    }
}
