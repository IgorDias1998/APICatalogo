using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repositories;
using APICatalogo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly ILogger<CategoriasController> _logger;
        public CategoriasController(IUnitOfWork uof,
                                    ILogger<CategoriasController> logger)
        {
            _uof = uof;
            _logger = logger;
        }

        //[HttpGet("LerArquivosConfiguracao")]
        //public string GetValores()
        //{
        //    var valor1 = _configuration["chave1"];
        //    var valor2 = _configuration["chave2"];

        //    var secao1 = _configuration["secao1:chave2"];

        //    var conexao = _configuration["ConnectionStrings:DefaultConnection"];

        //    return $"Chave 1 = {valor1} \nChave2 = {valor2} \nSeção1 => Chave2 = {secao1} \nConexao = {conexao}";
        //}

        [HttpGet("UsandoFromServices/{nome}")]
        public ActionResult<string> GetSaudacaoFromServico([FromServices]IMeuServico meuServico,
                                                           string nome)
        {
            return meuServico.Saudacao(nome);
        }

        [HttpGet("UsandoSemFromServices/{nome}")]
        public ActionResult<string> GetSaudacaoSemFromServico(IMeuServico meuServico,
                                                   string nome)
        {
            return meuServico.Saudacao(nome);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categorias = _uof.CategoriaRepository.GetAll();
            if(categorias is null)
            {
                _logger.LogWarning("Categorias não encontrada..");
                return NotFound("Categorias não encontrada..");
            }

            return Ok(categorias);
        }

        [HttpGet("{id:int}", Name ="ObterCategoria")]
        public ActionResult<Categoria> GetById(int id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);
            
            if(categoria is null)
            {
                _logger.LogWarning($"Categoria com o id = {id} não encontrada...");
                return NotFound("Categoria não encontrada");
            }

            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (categoria is null)
            {
                _logger.LogWarning("Categoria não encontrada..");
                return BadRequest("Categoria não encontrada..");
            }

            _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterCategoria",
                    new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if(id != categoria.CategoriaId)
            {
                _logger.LogWarning("BadRequest...");
                return BadRequest();
            }
            
            _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoriaId = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);
            if (categoriaId is null)
            {
                _logger.LogWarning("Categoria não localizada..");
                return NotFound("Categoria não localizada..");
            }

            _uof.CategoriaRepository.Delete(categoriaId);
            _uof.Commit();

            return Ok(categoriaId);
        }
    }
}
