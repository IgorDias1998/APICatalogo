using APICatalogo.Models;
using AutoMapper;

namespace APICatalogo.DTOs.Mappings
{
    public class ProdutoDTOMappingProfile : Profile
    {
        public ProdutoDTOMappingProfile() 
        { 
            CreateMap<Produto, ProdutoDTO>().ReverseMap();
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            CreateMap<Produto, ProdutoDTOUpdateRequestcs>().ReverseMap();
            CreateMap<Produto, ProdutoDTOUpdateResponse>().ReverseMap();
        }
    }
}
