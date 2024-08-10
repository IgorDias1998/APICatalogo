using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly ILogger<ProdutosController> _logger;

        public ProdutosController(ILogger<ProdutosController> logger,
                                    IUnitOfWork uof)
        {
            _logger = logger;
            _uof = uof;
        }

        [HttpGet("produtosporcategoria/{id}")]
        public ActionResult GetProdutosPorCategoria(int id)
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorCategoria(id);
            if (produtos is null)
                return NotFound();
            return Ok(produtos);
        }

        //[HttpGet("{valor:alpha:lenght(5)}")] exemplo passando o minimo de caracteres que deve ter para atender a requisição
        //[HttpGet("{valor:alpha}")] exemplo de restrição para aceitar parâmetros apenas de alphanumericos
        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var listaProdutos = _uof.ProdutoRepository.GetAll();

            if (listaProdutos is null)
            {
                _logger.LogWarning("Produtos não encontrados..");
                return NotFound("Produtos não encontrados..");
            }
            return Ok(listaProdutos);
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<Produto> GetByIdAsync([FromQuery] int id)
        {
            var produtos = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if(produtos is null)
            {
                _logger.LogWarning("Produto não encontrado..");
                return NotFound("Produto não encontrado..");
            }

            return Ok(produtos);
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
            {
                return BadRequest();
            }

            var novoProduto = _uof.ProdutoRepository.Create(produto);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterProduto",
                new { id = novoProduto.ProdutoId }, novoProduto);
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult Put(int id, Produto produto)
        {
            if(id != produto.ProdutoId)
            {
                return BadRequest();
            }

            var produtoAtualizado = _uof.ProdutoRepository.Update(produto);
            _uof.Commit();

            return Ok(produtoAtualizado);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);
         
            if (produto is null)
            {
                _logger.LogWarning("Produto não localizado");
                return NotFound("Produto não localizado");
            }

            _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();
            
            return Ok(produto);
        }
    }
}