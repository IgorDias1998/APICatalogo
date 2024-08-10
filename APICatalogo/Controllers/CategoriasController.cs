﻿using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
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
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            var categorias = _uof.CategoriaRepository.GetAll();
            if(categorias is null)
            {
                _logger.LogWarning("Categorias não encontrada..");
                return NotFound("Categorias não encontrada..");
            }

            var categoriasDTO = categorias.ToListaCategoriaDTO();

            return Ok(categoriasDTO);
        }

        [HttpGet("{id:int}", Name ="ObterCategoria")]
        public ActionResult<CategoriaDTO> GetById(int id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);
            
            if(categoria is null)
            {
                _logger.LogWarning($"Categoria com o id = {id} não encontrada...");
                return NotFound("Categoria não encontrada");
            }

            var categoriaDTO = categoria.ToCategoriaDTO();

            return Ok(categoriaDTO);
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
            {
                _logger.LogWarning("Categoria não encontrada..");
                return BadRequest("Categoria não encontrada..");
            }

            var categoria = categoriaDto.ToCategoria();

            var categoriaCriada =_uof.CategoriaRepository.Create(categoria);
            _uof.Commit();

            var novaCategoriaDto = categoria.ToCategoriaDTO();

            return new CreatedAtRouteResult("ObterCategoria",
                    new { id = novaCategoriaDto.CategoriaId }, novaCategoriaDto);
        }

        [HttpPut("{id:int}")]
        public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDto)
        {
            if(id != categoriaDto.CategoriaId)
            {
                _logger.LogWarning("BadRequest...");
                return BadRequest();
            }

            var categoria = categoriaDto.ToCategoria();
            var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();

            var categoriaAtualizadaDto = categoria.ToCategoriaDTO();

            return Ok(categoriaAtualizada);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categoriaId = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);
            if (categoriaId is null)
            {
                _logger.LogWarning("Categoria não localizada..");
                return NotFound("Categoria não localizada..");
            }

           var categoriaExcluida = _uof.CategoriaRepository.Delete(categoriaId);
            _uof.Commit();

            var categoriaExcluidaDTO = categoriaExcluida.ToCategoriaDTO();

            return Ok(categoriaExcluidaDTO);
        }
    }
}
