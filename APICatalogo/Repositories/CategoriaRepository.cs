﻿using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository : Repository<Categoria> , ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context) { }

        public PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParams)
        {
            var categorias = GetAll().OrderBy(c => c.CategoriaId).AsQueryable();
            var categoriasOrdenadas = PagedList<Categoria>.ToPagedList(categorias, categoriasParams.PageNumber, categoriasParams.PageSize);

            return categoriasOrdenadas;
        }

        public PagedList<Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome categoriasFiltroNome)
        {
            var categorias = GetAll().AsQueryable();
            if (!string.IsNullOrEmpty(categoriasFiltroNome.Nome))
            {
                categorias = categorias.Where(c => c.Nome.Contains(categoriasFiltroNome.Nome));
            }

            var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias, categoriasFiltroNome.PageNumber, categoriasFiltroNome.PageSize);
            return categoriasFiltradas;
        }
    }
}
