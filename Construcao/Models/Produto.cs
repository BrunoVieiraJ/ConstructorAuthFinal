using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Construcao.Models
{
    public class Produto
    {
        [Key]
        public long? ProdutoId { get; set; }

        [Required]
        public string Pro_Nome { get; set; }

        [Required]
        public string Pro_Desc { get; set; }

        [Required]
        public int Preco { get; set; }

        [Required]
        public int QntEstoque { get; set; }

        [ForeignKey("Categorias")]
        public long? FK_CategoriaId { get; set; }

        public Categoria? Categorias { get; set; }
    }
}
