using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
        private readonly IMapper _mapper;

        public ProdutosController(ILogger<ProdutosController> logger,
                                    IUnitOfWork uof,
                                    IMapper mapper)
        {
            _logger = logger;
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet("produtosporcategoria/{id}")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPorCategoria(int id)
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorCategoria(id);
            if (produtos is null)
                return NotFound();

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDto);
        }

        //[HttpGet("{valor:alpha:lenght(5)}")] exemplo passando o minimo de caracteres que deve ter para atender a requisição
        //[HttpGet("{valor:alpha}")] exemplo de restrição para aceitar parâmetros apenas de alphanumericos
        [HttpGet("produtos")]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            var listaProdutos = _uof.ProdutoRepository.GetAll();

            if (listaProdutos is null)
            {
                _logger.LogWarning("Produtos não encontrados..");
                return NotFound("Produtos não encontrados..");
            }

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(listaProdutos);

            return Ok(produtosDto);
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> GetByIdAsync([FromQuery] int id)
        {
            var produtos = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if(produtos is null)
            {
                _logger.LogWarning("Produto não encontrado..");
                return NotFound("Produto não encontrado..");
            }

            var produto = _mapper.Map<ProdutoDTO>(produtos);

            return Ok(produto);
        }

        [HttpPost]
        public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDTO)
        {
            if (produtoDTO is null)
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoDTO);

            var novoProduto = _uof.ProdutoRepository.Create(produto);
            _uof.Commit();

            var novoProdutoDto = _mapper.Map<ProdutoDTO>(produto);

            return new CreatedAtRouteResult("ObterProduto",
                new { id = novoProdutoDto.ProdutoId }, novoProdutoDto);
        }

        [HttpPatch("{id}/UpdatePartial")]
        public ActionResult<ProdutoDTOUpdateResponse> Patch(int id, 
            JsonPatchDocument<ProdutoDTOUpdateRequestcs> patchProdutoDTO)
        {
            if (patchProdutoDTO is null || id <= 0)
                return BadRequest();

            var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto is null)
                return NotFound();

            var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequestcs>(produto);
            patchProdutoDTO.ApplyTo(produtoUpdateRequest, ModelState);

            if (!ModelState.IsValid || TryValidateModel(produtoUpdateRequest))
                return BadRequest(ModelState);

            _mapper.Map(produtoUpdateRequest, produto);

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();

            return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDTO)
        {
            if(id != produtoDTO.ProdutoId)
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoDTO);

            var produtoAtualizado = _uof.ProdutoRepository.Update(produto);
            _uof.Commit();

            var produtoAtualizadoDto = _mapper.Map<ProdutoDTO>(produtoAtualizado);

            return Ok(produtoAtualizadoDto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);
         
            if (produto is null)
            {
                _logger.LogWarning("Produto não localizado");
                return NotFound("Produto não localizado");
            }

            _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();
            
            var produtoDeletadoDto = _mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoDeletadoDto);
        }
    }
}