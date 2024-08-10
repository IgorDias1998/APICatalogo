using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace APICatalogo.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context) { }

        public IEnumerable<Produto> GetProdutosPorCategoria(int idCategoria)
        {
            return GetAll().Where(c => c.CategoriaId == idCategoria);
        }
    }
}
