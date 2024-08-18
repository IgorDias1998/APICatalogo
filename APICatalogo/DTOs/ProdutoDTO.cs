using APICatalogo.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APICatalogo.DTOs
{
    public class ProdutoDTO
    {
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "Nome obrigatório")]
        [StringLength(80)]
        public string? Nome { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres")]
        public string? Descricao { get; set; }

        [Required]
        public decimal Preco { get; set; }

        [Required]
        [StringLength(300)]
        public string? ImagemUrl { get; set; }

        public int CategoriaId { get; set; }
    }
}
